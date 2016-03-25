using System;
using System.IO;
using BlastCorpsEditor;

namespace BlastCorpsConsole
{
   class Program
   {
      static void StatusPrint(ConsoleColor color, string colorMessage, string plainMessage)
      {
         ConsoleColor orig = System.Console.ForegroundColor;
         System.Console.ForegroundColor = color;
         System.Console.Write(colorMessage);
         System.Console.ForegroundColor = orig;
         System.Console.WriteLine(": " + plainMessage);
      }

      static void OutputLevelTxt(string filename, BlastCorpsLevelMeta levelMeta, BlastCorpsLevel level)
      {
         int offset;
         using (StreamWriter file = new StreamWriter(filename))
         {
            file.WriteLine("Level {0}: {1} [{2}]", levelMeta.id, levelMeta.name, levelMeta.filename);
            file.WriteLine();
            file.WriteLine("Header:");
            offset = 0;
            foreach (UInt16 val in level.header.u16s)
            {
               file.WriteLine("{0:X2}: {1:X}", offset, val);
               offset += 2;
            }
            file.WriteLine("{0:X2}: {1}", offset, level.header.gravity);
            offset += 4;
            file.WriteLine("{0:X2}: {1}", offset, level.header.u1C);
            offset += 4;
            foreach (UInt32 val in level.header.offsets)
            {
               file.WriteLine("{0:X2}: {1:X}", offset, val);
               offset += 4;
            }
            file.WriteLine();


            file.WriteLine("Carrier: {0}", level.carrier);
            file.WriteLine();

            file.WriteLine("Ammo [{0}]:", level.ammoBoxes.Count);
            foreach (AmmoBox ammo in level.ammoBoxes)
            {
               file.WriteLine(ammo);
            }
            file.WriteLine();

            file.WriteLine("Communication Points [{0}]:", level.commPoints.Count);
            foreach (CommPoint comm in level.commPoints)
            {
               file.WriteLine(comm);
            }
            file.WriteLine();

            file.WriteLine("RDUs [{0}]:", level.rdus.Count);
            foreach (RDU rdu in level.rdus)
            {
               file.WriteLine(rdu);
            }
            file.WriteLine();

            file.WriteLine("TNT Crates [{0}]:", level.tntCrates.Count);
            foreach (TNTCrate tnt in level.tntCrates)
            {
               file.WriteLine(tnt);
            }
            file.WriteLine();

            file.WriteLine("Square Blocks [{0}]:", level.squareBlocks.Count);
            foreach (SquareBlock block in level.squareBlocks)
            {
               file.WriteLine(block.ToStringFull());
            }
            file.WriteLine();

            file.WriteLine("Vehicles [{0}]:", level.vehicles.Count);
            foreach (Vehicle vehicle in level.vehicles)
            {
               file.WriteLine(vehicle);
            }
            file.WriteLine();

            file.WriteLine("Buildings [{0}]:", level.buildings.Count);
            foreach (Building building in level.buildings)
            {
               file.WriteLine(building);
            }
            file.WriteLine();
         }
      }

      static void Main(string[] args)
      {
         if (args.Length < 1)
         {
            System.Console.WriteLine("Usage: BlastCorpsConsole <DIR with .raw levels> [DIR to output <level>.txt]");
            return;
         }
         string tmpDir = Path.GetTempPath();
         string inputDir = args[0];
         string outputDir = null;

         if (args.Length > 1)
         {
            outputDir = args[1];
            System.Console.WriteLine("Outputing data to {0}", outputDir);
         }

         tmpDir = Path.Combine(tmpDir, "BlastCorps");
         System.Console.WriteLine("Saving generated levels to: " + tmpDir);

         Directory.CreateDirectory(tmpDir);

         foreach (BlastCorpsLevelMeta levelMeta in BlastCorpsRom.levelMeta)
         {
            string inputFile = Path.Combine(inputDir, levelMeta.filename + ".raw");
            string outputFile = Path.Combine(tmpDir, levelMeta.filename + ".raw");
            byte[] inData = File.ReadAllBytes(inputFile);
            BlastCorpsLevel level = BlastCorpsLevel.decodeLevel(inData, inData);

            if (outputDir != null)
            {
               string txtFile = Path.Combine(outputDir, levelMeta.filename + ".txt");
               OutputLevelTxt(txtFile, levelMeta, level);
            }

            byte[] outData = level.ToBytes();
            FileStream outStream = File.OpenWrite(outputFile);
            outStream.Write(outData, 0, outData.Length);
            outStream.Close();
            bool passed = true;
            if (levelMeta.id == 14)
            {
               StatusPrint(ConsoleColor.Yellow, "level14 fails square hole off", "0175A5");
            }
            if (inData.Length != outData.Length)
            {
               StatusPrint(ConsoleColor.Red, "Fail", levelMeta.filename + ".raw, lengths differ: " + inData.Length + " -> " + outData.Length);
               passed = false;
            }
            for (int i = 0; i < Math.Min(inData.Length, outData.Length); i++)
            {
               if (inData[i] != outData[i])
               {
                  StatusPrint(ConsoleColor.Red, "Fail", levelMeta.filename + ".raw, mismatch at " + i.ToString("X6"));
                  passed = false;
                  break;
               }
            }
            if (passed)
            {
               StatusPrint(ConsoleColor.Green, "Pass", levelMeta.filename + ".raw");
            }
         }
      }
   }
}
