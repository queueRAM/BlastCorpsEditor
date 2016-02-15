using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

namespace BlastCorpsEditor
{
   public class BlastCorpsLevelMeta
   {
      public int id { get; set; }
      public string name { get; set; }
      public string filename { get; set; }
      public int levelStart { get; set; }
      public int dlStart { get; set; }
      public int end { get; set; }

      public override string ToString()
      {
         return id + ": " + name + " [ " + filename + " ]";
      }
   }

   public class BlastCorpsRom
   {
      public enum RomType { Invalid, Vanilla, Extended };
      public enum Version { Invalid, Ver1p0, Ver1p1 };
      public enum Region { Invalid, Japan, US, Europe };

      private struct RomMeta
      {
         public UInt32 cksum1 { get; set; }
         public UInt32 cksum2 { get; set; }
         public Version version { get; set; }
         public Region region { get; set; }
      }

      public struct LevelData
      {
         public byte[] levelDataGzip;
         public byte[] dlDataGzip;
      }

      // vanilla ROM types
      private List<RomMeta> romMeta = new List<RomMeta>()
      {
         new RomMeta { cksum1 = 0x7C647C25, cksum2 = 0xD9D901E6, version = Version.Ver1p0, region = Region.US },
         new RomMeta { cksum1 = 0x7C647E65, cksum2 = 0x1948D305, version = Version.Ver1p1, region = Region.US },
         new RomMeta { cksum1 = 0x65234451, cksum2 = 0xEBD3346F, version = Version.Ver1p0, region = Region.Japan },
         new RomMeta { cksum1 = 0x7C64E6DB, cksum2 = 0x55B924DB, version = Version.Ver1p0, region = Region.Europe },
      };

      // level data for US V1.1 ROM
      public static List<BlastCorpsLevelMeta> levelMeta = new List<BlastCorpsLevelMeta>()
      {
         new BlastCorpsLevelMeta { id =  0, levelStart = 0x4ACC10, dlStart = 0x4B71AB, end = 0x4B8960, filename = "chimp",   name = "Simian Acres" },
         new BlastCorpsLevelMeta { id =  1, levelStart = 0x4A5660, dlStart = 0x4ABF91, end = 0x4ACC10, filename = "lagp",    name = "Angel City" },
         new BlastCorpsLevelMeta { id =  2, levelStart = 0x4B8960, dlStart = 0x4BEDE5, end = 0x4BFD60, filename = "valley",  name = "Outland Farm" },
         new BlastCorpsLevelMeta { id =  3, levelStart = 0x4BFD60, dlStart = 0x4C3247, end = 0x4C3AC0, filename = "fact",    name = "Blackridge Works" },
         new BlastCorpsLevelMeta { id =  4, levelStart = 0x4C3AC0, dlStart = 0x4D3B76, end = 0x4D5F90, filename = "dip",     name = "Glory Crossing" },
         new BlastCorpsLevelMeta { id =  5, levelStart = 0x4D5F90, dlStart = 0x4E1838, end = 0x4E2F70, filename = "beetle",  name = "Shuttle Gully" },
         new BlastCorpsLevelMeta { id =  6, levelStart = 0x4E2F70, dlStart = 0x4E48E4, end = 0x4E4E80, filename = "bonus1",  name = "Salvage Wharf" },
         new BlastCorpsLevelMeta { id =  7, levelStart = 0x4E4E80, dlStart = 0x4E7458, end = 0x4E7C00, filename = "bonus2",  name = "Skyfall" },
         new BlastCorpsLevelMeta { id =  8, levelStart = 0x4E7C00, dlStart = 0x4E8BF7, end = 0x4E8F70, filename = "bonus3",  name = "Twilight Foundry" },
         new BlastCorpsLevelMeta { id =  9, levelStart = 0x4E8F70, dlStart = 0x4F441E, end = 0x4F5C10, filename = "level9",  name = "Crystal Rift" },
         new BlastCorpsLevelMeta { id = 10, levelStart = 0x4F5C10, dlStart = 0x4FF4C1, end = 0x500520, filename = "level10", name = "Argent Towers" },
         new BlastCorpsLevelMeta { id = 11, levelStart = 0x500520, dlStart = 0x506D73, end = 0x507E80, filename = "level11", name = "Skerries" },
         new BlastCorpsLevelMeta { id = 12, levelStart = 0x507E80, dlStart = 0x51014A, end = 0x511340, filename = "level12", name = "Diamond Sands" },
         new BlastCorpsLevelMeta { id = 13, levelStart = 0x511340, dlStart = 0x5213B7, end = 0x523080, filename = "level13", name = "Ebony Coast" },
         new BlastCorpsLevelMeta { id = 14, levelStart = 0x523080, dlStart = 0x52B974, end = 0x52CD00, filename = "level14", name = "Oyster Harbor" },
         new BlastCorpsLevelMeta { id = 15, levelStart = 0x52CD00, dlStart = 0x531885, end = 0x532700, filename = "level15", name = "Carrick Point" },
         new BlastCorpsLevelMeta { id = 16, levelStart = 0x532700, dlStart = 0x53D32E, end = 0x53E9B0, filename = "level16", name = "Havoc District" },
         new BlastCorpsLevelMeta { id = 17, levelStart = 0x53E9B0, dlStart = 0x54909A, end = 0x54A820, filename = "level17", name = "Ironstone Mine" },
         new BlastCorpsLevelMeta { id = 18, levelStart = 0x54A820, dlStart = 0x551DAA, end = 0x552DE0, filename = "level18", name = "Beeton Tracks" },
         new BlastCorpsLevelMeta { id = 19, levelStart = 0x552DE0, dlStart = 0x554A33, end = 0x555000, filename = "level19", name = "J-Bomb" },
         new BlastCorpsLevelMeta { id = 20, levelStart = 0x555000, dlStart = 0x55F205, end = 0x560E90, filename = "level20", name = "Jade Plateau" },
         new BlastCorpsLevelMeta { id = 21, levelStart = 0x560E90, dlStart = 0x5646CB, end = 0x5652D0, filename = "level21", name = "Marine Quarter" },
         new BlastCorpsLevelMeta { id = 22, levelStart = 0x5652D0, dlStart = 0x56E0F7, end = 0x56F3F0, filename = "level22", name = "Cooter Creek" },
         new BlastCorpsLevelMeta { id = 23, levelStart = 0x56F3F0, dlStart = 0x571E88, end = 0x5721E0, filename = "level23", name = "Gibbon's Gate" },
         new BlastCorpsLevelMeta { id = 24, levelStart = 0x5721E0, dlStart = 0x573354, end = 0x5736E0, filename = "level24", name = "Baboon Catacomb" },
         new BlastCorpsLevelMeta { id = 25, levelStart = 0x5736E0, dlStart = 0x5795B8, end = 0x57A2C0, filename = "level25", name = "Sleek Streets" },
         new BlastCorpsLevelMeta { id = 26, levelStart = 0x57A2C0, dlStart = 0x58005F, end = 0x580B60, filename = "level26", name = "Obsidian Mile" },
         new BlastCorpsLevelMeta { id = 27, levelStart = 0x580B60, dlStart = 0x587CA0, end = 0x588CE0, filename = "level27", name = "Corvine Bluff" },
         new BlastCorpsLevelMeta { id = 28, levelStart = 0x588CE0, dlStart = 0x58B53D, end = 0x58BE80, filename = "level28", name = "Sideswipe" },
         new BlastCorpsLevelMeta { id = 29, levelStart = 0x58BE80, dlStart = 0x596201, end = 0x597B80, filename = "level29", name = "Echo Marches" },
         new BlastCorpsLevelMeta { id = 30, levelStart = 0x597B80, dlStart = 0x59AEC7, end = 0x59B7D0, filename = "level30", name = "Kipling Plant" },
         new BlastCorpsLevelMeta { id = 31, levelStart = 0x59B7D0, dlStart = 0x5A4451, end = 0x5A5840, filename = "level31", name = "Falchion Field" },
         new BlastCorpsLevelMeta { id = 32, levelStart = 0x5A5840, dlStart = 0x5AF6C1, end = 0x5B0B10, filename = "level32", name = "Morgan Hall" },
         new BlastCorpsLevelMeta { id = 33, levelStart = 0x5B0B10, dlStart = 0x5B4E60, end = 0x5B5A30, filename = "level33", name = "Tempest City" },
         new BlastCorpsLevelMeta { id = 34, levelStart = 0x5B5A30, dlStart = 0x5B850E, end = 0x5B8BB0, filename = "level34", name = "Orion Plaza" },
         new BlastCorpsLevelMeta { id = 35, levelStart = 0x5B8BB0, dlStart = 0x5C366C, end = 0x5C4C80, filename = "level35", name = "Glander's Ranch" },
         new BlastCorpsLevelMeta { id = 36, levelStart = 0x5C4C80, dlStart = 0x5C9D2D, end = 0x5CA9C0, filename = "level36", name = "Dagger Pass" },
         new BlastCorpsLevelMeta { id = 37, levelStart = 0x5CA9C0, dlStart = 0x5CC86E, end = 0x5CCF50, filename = "level37", name = "Geode Square" },
         new BlastCorpsLevelMeta { id = 38, levelStart = 0x5CCF50, dlStart = 0x5D07B8, end = 0x5D1060, filename = "level38", name = "Shuttle Island" },
         new BlastCorpsLevelMeta { id = 39, levelStart = 0x5D1060, dlStart = 0x5DB0FC, end = 0x5DC830, filename = "level39", name = "Mica Park" },
         new BlastCorpsLevelMeta { id = 40, levelStart = 0x5DC830, dlStart = 0x5E5B29, end = 0x5E6EE0, filename = "level40", name = "Moon" },
         new BlastCorpsLevelMeta { id = 41, levelStart = 0x5E6EE0, dlStart = 0x5EB4C8, end = 0x5EC800, filename = "level41", name = "Cobalt Quarry" },
         new BlastCorpsLevelMeta { id = 42, levelStart = 0x5EC800, dlStart = 0x5F2C67, end = 0x5F3A80, filename = "level42", name = "Moraine Chase" },
         new BlastCorpsLevelMeta { id = 43, levelStart = 0x5F3A80, dlStart = 0x5FFF79, end = 0x6014B0, filename = "level43", name = "Mercury" },
         new BlastCorpsLevelMeta { id = 44, levelStart = 0x6014B0, dlStart = 0x6095C1, end = 0x60A710, filename = "level44", name = "Venus" },
         new BlastCorpsLevelMeta { id = 45, levelStart = 0x60A710, dlStart = 0x6126C5, end = 0x613AA0, filename = "level45", name = "Mars" },
         new BlastCorpsLevelMeta { id = 46, levelStart = 0x613AA0, dlStart = 0x61C88C, end = 0x61DD70, filename = "level46", name = "Neptune" },
         new BlastCorpsLevelMeta { id = 47, levelStart = 0x61DD70, dlStart = 0x6211ED, end = 0x621AF0, filename = "level47", name = "CMO Intro" },
         new BlastCorpsLevelMeta { id = 48, levelStart = 0x621AF0, dlStart = 0x625CAC, end = 0x6269E0, filename = "level48", name = "Silver Junction" },
         new BlastCorpsLevelMeta { id = 49, levelStart = 0x6269E0, dlStart = 0x62F871, end = 0x630C30, filename = "level49", name = "End Sequence" },
         new BlastCorpsLevelMeta { id = 50, levelStart = 0x630C30, dlStart = 0x634D35, end = 0x635700, filename = "level50", name = "Shuttle Clear" },
         new BlastCorpsLevelMeta { id = 51, levelStart = 0x635700, dlStart = 0x63BD00, end = 0x63CA10, filename = "level51", name = "Dark Heartland" },
         new BlastCorpsLevelMeta { id = 52, levelStart = 0x63CA10, dlStart = 0x6416D6, end = 0x641F30, filename = "level52", name = "Magma Peak" },
         new BlastCorpsLevelMeta { id = 53, levelStart = 0x641F30, dlStart = 0x6440CC, end = 0x644810, filename = "level53", name = "Thunderfist" },
         new BlastCorpsLevelMeta { id = 54, levelStart = 0x644810, dlStart = 0x645D55, end = 0x646080, filename = "level54", name = "Saline Watch" },
         new BlastCorpsLevelMeta { id = 55, levelStart = 0x646080, dlStart = 0x6470B7, end = 0x647550, filename = "level55", name = "Backlash" },
         new BlastCorpsLevelMeta { id = 56, levelStart = 0x647550, dlStart = 0x65362E, end = 0x654FC0, filename = "level56", name = "Bison Ridge" },
         new BlastCorpsLevelMeta { id = 57, levelStart = 0x654FC0, dlStart = 0x65F329, end = 0x660950, filename = "level57", name = "Ember Hamlet" },
         new BlastCorpsLevelMeta { id = 58, levelStart = 0x660950, dlStart = 0x66550A, end = 0x665F80, filename = "level58", name = "Cromlech Court" },
         new BlastCorpsLevelMeta { id = 59, levelStart = 0x665F80, dlStart = 0x66BB49, end = 0x66C900, filename = "level59", name = "Lizard Island" },
      };

      const int LEVEL_TABLE_START = 0x7FC000;
      const int LEVEL_START = 8 * 1024 * 1024;
      const int LEVEL_ALIGNMENT = 0x10;

      public RomType type { get; set; }
      public Version version { get; set; }
      public Region region { get; set; }
      public string savePath { get; set; }

      // raw bytes of ROM to be written out
      private byte[] romData;

      private LevelData[] levels = new LevelData[levelMeta.Count];

      // swap every other byte: ABCD -> BADC
      private void swapBytes()
      {
         for (int i = 0; i < romData.Length; i += 2)
         {
            byte tmp = romData[i];
            romData[i] = romData[i + 1];
            romData[i + 1] = tmp;
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
         if (romData.Length == 8 * 1024 * 1024)
         {
            foreach (RomMeta rom in romMeta)
            {
               if (cksum1 == rom.cksum1 && cksum2 == rom.cksum2)
               {
                  version = rom.version;
                  region = rom.region;
                  type = RomType.Vanilla;
                  break;
               }
            }
         }
         else if (romData.Length > 8 * 1024 * 1024)
         {
            // assume extended unless table at end contains invalid data
            type = RomType.Extended;
            // verify table references valid range of data
            for (uint offset = LEVEL_TABLE_START; offset < LEVEL_TABLE_START + 0x10 * levelMeta.Count; offset += 4)
            {
               UInt32 levelOffset = BE.U32(romData, offset);
               if (levelOffset < 4 * 1024 * 1024 || levelOffset > romData.Length)
               {
                  type = RomType.Invalid;
                  break;
               }
            }
            if (type == RomType.Extended)
            {
               switch (romData[0x3E])
               {
                  case 0x45: region = Region.US; break;
                  case 0x4A: region = Region.Japan; break;
                  case 0x50: region = Region.Europe; break;
               }
               switch (romData[0x3F])
               {
                  case 0x00: version = Version.Ver1p0; break;
                  case 0x01: version = Version.Ver1p1; break;
               }
            }
         }
      }

      // copy level data from extended ROM to level structure
      private void copyLevels()
      {
         int start, end, length;
         for (uint i = 0; i < levels.Length; i++)
         {
            // copy level
            start = BE.I32(romData, LEVEL_TABLE_START + 0x10 * i);
            end = BE.I32(romData, LEVEL_TABLE_START + 0x10 * i + 4);
            length = end - start;
            levels[i].levelDataGzip = new byte[length];
            Array.Copy(romData, start, levels[i].levelDataGzip, 0, length);
            // copy display list
            start = BE.I32(romData, LEVEL_TABLE_START + 0x10 * i + 8);
            end = BE.I32(romData, LEVEL_TABLE_START + 0x10 * i + 0xC);
            levels[i].dlDataGzip = new byte[length];
            Array.Copy(romData, start, levels[i].dlDataGzip, 0, length);
         }
      }

      private static bool SaveBinFile(string filePath, byte[] data, int start, int end)
      {
         try
         {
            FileStream outStream = File.OpenWrite(filePath);
            outStream.Write(data, start, end - start);
            outStream.Close();
            return true;
         }
         catch
         {
            return false;
         }
      }

      public static int RunProcess(string exePath, string arguments)
      {
         ProcessStartInfo startInfo = new ProcessStartInfo();
         startInfo.CreateNoWindow = true;
         startInfo.UseShellExecute = false;
         startInfo.FileName = exePath;
         startInfo.WindowStyle = ProcessWindowStyle.Hidden;
         startInfo.Arguments = arguments;

         using (Process exeProcess = Process.Start(startInfo))
         {
            exeProcess.WaitForExit();
            return exeProcess.ExitCode;
         }
      }

      public static byte[] GzipDeflate(byte[] data, string embedFilename)
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

      private static byte[] GzipInflate(byte[] gzip)
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

      private int Align(int num, int alignment)
      {
         return ((num + (alignment - 1)) / alignment) * alignment;
      }

      public void ExtendRom()
      {
         byte[] codeTextGzip;
         byte[] codeDataGzip;

         // TODO: support more ROM types
         if (type == RomType.Vanilla && region == Region.US && version == Version.Ver1p1)
         {
            // 1. extract hd_code_text and save copy of hd_code_data
            const int textStart = 0x787FD0;
            const int textEnd = 0x7D73B4;
            const int dataStart = 0x7D73B4;
            const int dataEnd = 0x7E3AD0;
            codeTextGzip = new byte[textEnd - textStart];
            Array.Copy(romData, textStart, codeTextGzip, 0, codeTextGzip.Length);
            codeDataGzip = new byte[dataEnd - dataStart];
            Array.Copy(romData, dataStart, codeDataGzip, 0, codeDataGzip.Length);

            // 2. inflate hd_code_text
            byte[] codeText = GzipInflate(codeTextGzip);

            // 3. apply patch to hd_code_text
            // start 0x119BC
            // fill 0x12240-pc()
            UInt32[] patch = new UInt32[] {
               0x000E7100,
               0x3C01B080,
               0x002E0821,
               0x8C2EC000,
               0xAFAE0024,
               0x8C2FC00C,
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
            codeTextGzip = GzipDeflate(codeText, "hd_code_text.raw");

            // 5. extend ROM to 12MB
            Array.Resize<byte>(ref romData, 12 * 1024 * 1024);

            // 6. copy hd_code_text and hd_code_data back into ROM
            Array.Copy(codeTextGzip, 0, romData, textStart, codeTextGzip.Length);
            Array.Copy(codeDataGzip, 0, romData, textStart + codeTextGzip.Length, codeDataGzip.Length);

            // 7. copy all gziped levels into memory, to later be written to end of ROM or inflated and edited
            foreach (BlastCorpsLevelMeta level in levelMeta)
            {
               int length;
               // copy level
               length = level.dlStart - level.levelStart;
               levels[level.id].levelDataGzip = new byte[length];
               Array.Copy(romData, level.levelStart, levels[level.id].levelDataGzip, 0, length);
               // copy display list
               length = level.end - level.dlStart;
               levels[level.id].dlDataGzip = new byte[length];
               Array.Copy(romData, level.dlStart, levels[level.id].dlDataGzip, 0, length);
            }

            type = RomType.Extended;
         }
      }

      public byte[] GetLevelData(int levelId)
      {
         if (levelId < levels.Length)
         {
            return GzipInflate(levels[levelId].levelDataGzip);
         }
         return null;
      }

      public byte[] GetDisplayList(int levelId)
      {
         if (levelId < levels.Length)
         {
            return GzipInflate(levels[levelId].dlDataGzip);
         }
         return null;
      }

      public void UpdateLevel(int levelId, byte[] levelData, byte[] dlData)
      {
         if (levelId < levels.Length)
         {
            string basename = levelMeta[levelId].filename;
            levels[levelId].levelDataGzip = GzipDeflate(levelData, basename + ".raw");
            levels[levelId].dlDataGzip = GzipDeflate(dlData, basename + "_dl.raw");
         }
      }

      public bool SaveRom(string romPath)
      {
         int offset = LEVEL_START;
         int tableOffset = LEVEL_TABLE_START;
         foreach (LevelData level in levels)
         {
            // copy level
            int length = level.levelDataGzip.Length;
            Array.Copy(level.levelDataGzip, 0, romData, offset, length);
            BE.ToBytes(offset, romData, tableOffset);
            offset += length;
            BE.ToBytes(offset, romData, tableOffset + 4);
            tableOffset += 8;
            // copy display list
            length = level.dlDataGzip.Length;
            Array.Copy(level.dlDataGzip, 0, romData, offset, length);
            BE.ToBytes(offset, romData, tableOffset);
            offset += length;
            BE.ToBytes(offset, romData, tableOffset + 4);
            tableOffset += 8;
            offset = Align(offset, LEVEL_ALIGNMENT);
         }

         bool success = SaveBinFile(romPath, romData, 0, romData.Length);

         // TODO: checksum update? don't actually modify anything in the checksum area
         return success;
      }

      public bool LoadRom(string romPath)
      {
         FileInfo info = new FileInfo(romPath);
         if (info.Exists && info.Length >= 8 * 1024 * 1024)
         {
            romData = System.IO.File.ReadAllBytes(romPath);
            forceByteOrdering();
            collectRomData();
            if (version != Version.Invalid && type == RomType.Extended)
            {
               copyLevels();
               savePath = romPath;
            }
            return true;
         }
         else
         {
            return false;
         }
      }
   }
}
