using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

namespace BlastCorpsEditor
{
   class BlastCorpsRom
   {
      public enum Type { Invalid, Vanilla, Extended };
      public enum Version { Invalid, Ver1p0, Ver1p1 };
      public enum Region { Invalid, Japan, US, Europe };

      private struct RomMeta
      {
         public UInt32 cksum1 { get; set; }
         public UInt32 cksum2 { get; set; }
         public Version version { get; set; }
         public Region region { get; set; }
      }

      // vanilla ROM types
      private List<RomMeta> romMeta = new List<RomMeta>()
      {
         new RomMeta { cksum1 = 0x7C647C25, cksum2 = 0xD9D901E6, version = Version.Ver1p0, region = Region.US },
             new RomMeta { cksum1 = 0x7C647E65, cksum2 = 0x1948D305, version = Version.Ver1p1, region = Region.US },
             new RomMeta { cksum1 = 0x65234451, cksum2 = 0xEBD3346F, version = Version.Ver1p0, region = Region.Japan },
             new RomMeta { cksum1 = 0x7C64E6DB, cksum2 = 0x55B924DB, version = Version.Ver1p0, region = Region.Europe },
      };

      public Type type { get; set; }
      public Version version { get; set;  }
      public Region region { get; set; }

      private byte[] romData;

      // swap every other byte: ABCD -> BADC
      private void swapBytes()
      {
         for (int i = 0; i < romData.Length; i += 2)
         {
            byte tmp = romData[i];
            romData[i] = romData[i+1];
            romData[i+1] = tmp;
         }
      }

      // convert from LE to BE: ABCD -> DCBA
      private void reverseEndian()
      {
         for (int i = 0; i < romData.Length; i += 4)
         {
            Array.Reverse(romData, i, 4);
         }
      }

      // force Z64 (BE) byte-ordering
      private void forceByteOrdering()
      {
         // detect based on PI BSD Domain 1 register
         UInt32 piBsd1Reg = BE.U32(romData, 0x0);

         switch (piBsd1Reg)
         {
            case 0x37804012: // v64 (byte-swapped)
               swapBytes();
               break;
            case 0x40123780: // n64 (LE)
               reverseEndian();
               break;
            case 0x80371240: // z64 (BE)
               break;
         }
      }

      private void collectRomData()
      {
         UInt32 cksum1, cksum2;
         cksum1 = BE.U32(romData, 0x10);
         cksum2 = BE.U32(romData, 0x14);
         foreach (RomMeta rom in romMeta)
         {
            if (cksum1 == rom.cksum1 && cksum2 == rom.cksum2)
            {
               version = rom.version;
               region = rom.region;
               type = Type.Vanilla;
               break;
            }
         }
      }

      private void SaveBinFile(string filePath, byte[] data, int start, int end)
      {
         FileStream outStream = File.OpenWrite(filePath);
         outStream.Write(data, start, end - start);
         outStream.Close();
      }

      public static byte[] GzipDeflate(byte[] data)
      {
         using (MemoryStream memory = new MemoryStream())
         {
            using (GZipStream gzip = new GZipStream(memory, CompressionMode.Compress, true))
            {
               gzip.Write(data, 0, data.Length);
            }
            return memory.ToArray();
         }
      }

      private byte[] GzipInflate(byte[] gzip)
      {
         using (GZipStream stream = new GZipStream(new MemoryStream(gzip), CompressionMode.Decompress))
         {
            const int size = 4096;
            byte[] buffer = new byte[size];
            using (MemoryStream memory = new MemoryStream())
            {
               int count = 0;
               do
               {
                  count = stream.Read(buffer, 0, size);
                  if (count > 0)
                  {
                     memory.Write(buffer, 0, count);
                  }
               }
               while (count > 0);
               return memory.ToArray();
            }
         }
      }

      public void ExtendRom(string outPath)
      {
         // TODO: support more ROM types

         string tmpDir = Path.GetTempPath();
         byte[] codeTextGzip;
         byte[] codeDataGzip;

         if (type == Type.Vanilla && region == Region.US && version == Version.Ver1p1)
         {
            // 1. extract hd_code_text and save copy of hd_code_data
            codeTextGzip = new byte[0x7D73B4 - 0x787FD0];
            Array.Copy(romData, 0x787FD0, codeTextGzip, 0, codeTextGzip.Length);
            codeDataGzip = new byte[0x7E3AD0 - 0x7D73B4];
            Array.Copy(romData, 0x7D73B4, codeDataGzip, 0, codeDataGzip.Length);

            // 2. inflate hd_code_text
            byte[] codeText = GzipInflate(codeTextGzip);

            // 3. apply patch to hd_code_text
            // start 0x119BC
            // fill 0x12240-pc()
            UInt32[] patch = new UInt32[] {
               0x000E70C0, 
                  0x3C01B080, 
                  0x002E0821, 
                  0x8C2EA000,
                  0xAFAE0024,
                  0x8C2FA004,
                  0x01EE7822,
                  0x8FAC0030,
                  0xAD8F0000,
                  0x10000217
            };
            int offset = 0x119BC;
            foreach (UInt32 word in patch)
            {
               BE.ToBytes(word, codeText, offset);
               offset += 4;
            }
            while (offset < 0x12240)
            {
               BE.ToBytes(0x00U, codeText, offset);
               offset += 4;
            }

            // 4. deflate hd_code_text
            codeTextGzip = GzipDeflate(codeText);

            // TODO: remove debug
            SaveBinFile(Path.Combine(tmpDir, "hd_code_text.raw"), codeText, 0, codeText.Length);
            SaveBinFile(Path.Combine(tmpDir, "hd_code_text.raw.gz"), codeTextGzip, 0, codeTextGzip.Length);

            // 5. extend ROM to 12MB

            // 6. copy hd_code_text and hd_code_data back into ROM

            // 7. copy all levels to end of ROM, saving start and end offsets to 0x7FA000
         }
      }

      public bool LoadRom(string romPath)
      {
         FileInfo info = new FileInfo(romPath);
         if (info.Exists && info.Length >= 8 * 1024 * 1024)
         {
            romData = System.IO.File.ReadAllBytes(romPath);
            forceByteOrdering();
            collectRomData();
            return true;
         }
         else
         {
            return false;
         }
      }
   }
}
