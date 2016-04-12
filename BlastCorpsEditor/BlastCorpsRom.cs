using System;
using System.Collections.Generic;
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
      public int dlLength { get; set; }
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

      // level data for US V1.0 and V1.1 ROMs
      public static List<BlastCorpsLevelMeta> levelMeta = new List<BlastCorpsLevelMeta>()
      {
         new BlastCorpsLevelMeta { id =  0, dlLength = 0x0017B5, filename = "chimp",   name = "Simian Acres" },
         new BlastCorpsLevelMeta { id =  1, dlLength = 0x000C7F, filename = "lagp",    name = "Angel City" },
         new BlastCorpsLevelMeta { id =  2, dlLength = 0x000F7B, filename = "valley",  name = "Outland Farm" },
         new BlastCorpsLevelMeta { id =  3, dlLength = 0x000879, filename = "fact",    name = "Blackridge Works" },
         new BlastCorpsLevelMeta { id =  4, dlLength = 0x00241A, filename = "dip",     name = "Glory Crossing" },
         new BlastCorpsLevelMeta { id =  5, dlLength = 0x001738, filename = "beetle",  name = "Shuttle Gully" },
         new BlastCorpsLevelMeta { id =  6, dlLength = 0x00059C, filename = "bonus1",  name = "Salvage Wharf" },
         new BlastCorpsLevelMeta { id =  7, dlLength = 0x0007A8, filename = "bonus2",  name = "Skyfall" },
         new BlastCorpsLevelMeta { id =  8, dlLength = 0x000379, filename = "bonus3",  name = "Twilight Foundry" },
         new BlastCorpsLevelMeta { id =  9, dlLength = 0x0017F2, filename = "level9",  name = "Crystal Rift" },
         new BlastCorpsLevelMeta { id = 10, dlLength = 0x00105F, filename = "level10", name = "Argent Towers" },
         new BlastCorpsLevelMeta { id = 11, dlLength = 0x00110D, filename = "level11", name = "Skerries" },
         new BlastCorpsLevelMeta { id = 12, dlLength = 0x0011F6, filename = "level12", name = "Diamond Sands" },
         new BlastCorpsLevelMeta { id = 13, dlLength = 0x001CC9, filename = "level13", name = "Ebony Coast" },
         new BlastCorpsLevelMeta { id = 14, dlLength = 0x00138C, filename = "level14", name = "Oyster Harbor" },
         new BlastCorpsLevelMeta { id = 15, dlLength = 0x000E7B, filename = "level15", name = "Carrick Point" },
         new BlastCorpsLevelMeta { id = 16, dlLength = 0x001682, filename = "level16", name = "Havoc District" },
         new BlastCorpsLevelMeta { id = 17, dlLength = 0x001786, filename = "level17", name = "Ironstone Mine" },
         new BlastCorpsLevelMeta { id = 18, dlLength = 0x001036, filename = "level18", name = "Beeton Tracks" },
         new BlastCorpsLevelMeta { id = 19, dlLength = 0x0005CD, filename = "level19", name = "J-Bomb" },
         new BlastCorpsLevelMeta { id = 20, dlLength = 0x001C8B, filename = "level20", name = "Jade Plateau" },
         new BlastCorpsLevelMeta { id = 21, dlLength = 0x000C05, filename = "level21", name = "Marine Quarter" },
         new BlastCorpsLevelMeta { id = 22, dlLength = 0x0012F9, filename = "level22", name = "Cooter Creek" },
         new BlastCorpsLevelMeta { id = 23, dlLength = 0x000358, filename = "level23", name = "Gibbon's Gate" },
         new BlastCorpsLevelMeta { id = 24, dlLength = 0x00038C, filename = "level24", name = "Baboon Catacomb" },
         new BlastCorpsLevelMeta { id = 25, dlLength = 0x000D08, filename = "level25", name = "Sleek Streets" },
         new BlastCorpsLevelMeta { id = 26, dlLength = 0x000B01, filename = "level26", name = "Obsidian Mile" },
         new BlastCorpsLevelMeta { id = 27, dlLength = 0x001040, filename = "level27", name = "Corvine Bluff" },
         new BlastCorpsLevelMeta { id = 28, dlLength = 0x000943, filename = "level28", name = "Sideswipe" },
         new BlastCorpsLevelMeta { id = 29, dlLength = 0x00197F, filename = "level29", name = "Echo Marches" },
         new BlastCorpsLevelMeta { id = 30, dlLength = 0x000909, filename = "level30", name = "Kipling Plant" },
         new BlastCorpsLevelMeta { id = 31, dlLength = 0x0013EF, filename = "level31", name = "Falchion Field" },
         new BlastCorpsLevelMeta { id = 32, dlLength = 0x00144F, filename = "level32", name = "Morgan Hall" },
         new BlastCorpsLevelMeta { id = 33, dlLength = 0x000BD0, filename = "level33", name = "Tempest City" },
         new BlastCorpsLevelMeta { id = 34, dlLength = 0x0006A2, filename = "level34", name = "Orion Plaza" },
         new BlastCorpsLevelMeta { id = 35, dlLength = 0x001614, filename = "level35", name = "Glander's Ranch" },
         new BlastCorpsLevelMeta { id = 36, dlLength = 0x000C93, filename = "level36", name = "Dagger Pass" },
         new BlastCorpsLevelMeta { id = 37, dlLength = 0x0006E2, filename = "level37", name = "Geode Square" },
         new BlastCorpsLevelMeta { id = 38, dlLength = 0x0008A8, filename = "level38", name = "Shuttle Island" },
         new BlastCorpsLevelMeta { id = 39, dlLength = 0x001734, filename = "level39", name = "Mica Park" },
         new BlastCorpsLevelMeta { id = 40, dlLength = 0x0013B7, filename = "level40", name = "Moon" },
         new BlastCorpsLevelMeta { id = 41, dlLength = 0x001338, filename = "level41", name = "Cobalt Quarry" },
         new BlastCorpsLevelMeta { id = 42, dlLength = 0x000E19, filename = "level42", name = "Moraine Chase" },
         new BlastCorpsLevelMeta { id = 43, dlLength = 0x001537, filename = "level43", name = "Mercury" },
         new BlastCorpsLevelMeta { id = 44, dlLength = 0x00114F, filename = "level44", name = "Venus" },
         new BlastCorpsLevelMeta { id = 45, dlLength = 0x0013DB, filename = "level45", name = "Mars" },
         new BlastCorpsLevelMeta { id = 46, dlLength = 0x0014E4, filename = "level46", name = "Neptune" },
         new BlastCorpsLevelMeta { id = 47, dlLength = 0x000903, filename = "level47", name = "CMO Intro" },
         new BlastCorpsLevelMeta { id = 48, dlLength = 0x000D34, filename = "level48", name = "Silver Junction" },
         new BlastCorpsLevelMeta { id = 49, dlLength = 0x0013BF, filename = "level49", name = "End Sequence" },
         new BlastCorpsLevelMeta { id = 50, dlLength = 0x0009CB, filename = "level50", name = "Shuttle Clear" },
         new BlastCorpsLevelMeta { id = 51, dlLength = 0x000D10, filename = "level51", name = "Dark Heartland" },
         new BlastCorpsLevelMeta { id = 52, dlLength = 0x00085A, filename = "level52", name = "Magma Peak" },
         new BlastCorpsLevelMeta { id = 53, dlLength = 0x000744, filename = "level53", name = "Thunderfist" },
         new BlastCorpsLevelMeta { id = 54, dlLength = 0x00032B, filename = "level54", name = "Saline Watch" },
         new BlastCorpsLevelMeta { id = 55, dlLength = 0x000499, filename = "level55", name = "Backlash" },
         new BlastCorpsLevelMeta { id = 56, dlLength = 0x001992, filename = "level56", name = "Bison Ridge" },
         new BlastCorpsLevelMeta { id = 57, dlLength = 0x001627, filename = "level57", name = "Ember Hamlet" },
         new BlastCorpsLevelMeta { id = 58, dlLength = 0x000A76, filename = "level58", name = "Cromlech Court" },
         new BlastCorpsLevelMeta { id = 59, dlLength = 0x000DB7, filename = "level59", name = "Lizard Island" },
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

      Dictionary<string, int> gzipFiles = new Dictionary<string, int>();

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

      private void findGzipFiles()
      {
         const int START_SEARCH = 4 * 1024 * 1024;
         for (int i = START_SEARCH; i < romData.Length - 4; i++)
         {
            if (romData[i] == 0x1F && romData[i+1] == 0x8B && romData[i + 2] == 0x08 && romData[i+3] == 0x08)
            {
               int length = 0;
               while (romData[i + 10 + length] != 0x00)
               {
                  length++;
               }
               string name = System.Text.Encoding.ASCII.GetString(romData, i + 10, length);
               if (!gzipFiles.ContainsKey(name))
               {
                  gzipFiles.Add(name, i);
               }
               else
               {
                  // TODO: do something about the duplicate
                  // MessageBox.Show(String.Format("Duplicate: {0}: {1:X} {2:X}", name, gzipFiles[name], i), "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Hand);
               }
            }
         }

         // assign levels
         foreach (BlastCorpsLevelMeta level in levelMeta)
         {
            string basename = level.filename + ".raw";
            string dlname = level.filename + "_dl.raw";
            if (gzipFiles.ContainsKey(basename))
            {
               level.levelStart = gzipFiles[basename];
            }
            if (gzipFiles.ContainsKey(dlname))
            {
               level.dlStart = gzipFiles[dlname];
               level.end = level.dlStart + level.dlLength;
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

         // Only (U) 1.0, 1.1 and (J) ROMs supported
         // TODO: support (E) ROM
         if (type == RomType.Vanilla && (region == Region.US || region == Region.Japan))
         {
            // 1. extract hd_code_text and save copy of hd_code_data
            // TODO: move this to a table or combine with romMeta or use gzipFiles lookup table
            int textStart = 0x787FD0;
            int textEnd = 0x7D73B4;
            int dataStart = 0x7D73B4;
            int dataEnd = 0x7E3AD0;
            if (region == Region.US && version == Version.Ver1p0)
            {
               textStart = 0x787FD0; textEnd = 0x7D74D7; dataStart = 0x7D74D7; dataEnd = 0x7E3BF0;
            }
            else if (region == Region.US && version == Version.Ver1p1)
            {
               textStart = 0x787FD0; textEnd = 0x7D73B4; dataStart = 0x7D73B4; dataEnd = 0x7E3AD0;
            }
            else if (region == Region.Japan)
            {
               textStart = 0x786490; textEnd = 0x7D5A54; dataStart = 0x7D5A54; dataEnd = 0x7E2050;
            }
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
               0x000E7100, // sll   t6, t6, 0x4    // each entry is four words
               0x3C01B080, // lui   at, 0xB080     // %hi(0xB07FC000), 0xB0000000 = ROM
               0x002E0821, // addu  at, at, t6
               0x8C2EC000, // lw    t6, 0xC000(at) // %lo(0xB07FC000)
               0xAFAE0024, // sw    t6, 0x24(sp)
               0x8C2FC00C, // lw    t7, 0xC00C(at) // %lo(0xB07FC00C)
               0x01EE7822, // sub   t7, t7, t6     // compute length
               0x8FAC0030, // lw    t4, 0x30(sp)
               0xAD8F0000, // sw    t7, 0x0(t4)
               0x10000217  // b     0x12240        // skip over old code
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

      public byte[] GetRawData()
      {
         return romData;
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
            offset = Align(offset, LEVEL_ALIGNMENT);
            BE.ToBytes(offset, romData, tableOffset + 4);
            tableOffset += 8;
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
            if (version != Version.Invalid)
            {
               if (type == RomType.Extended)
               {
                  copyLevels();
                  savePath = romPath;
               }
               else
               {
                  findGzipFiles();
               }
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
