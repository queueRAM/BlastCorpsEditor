using BlastCorpsEditor;
using System;
using System.IO;

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
               using (StreamWriter writer = new StreamWriter(txtFile))
               {
                  level.Write(writer, levelMeta);
               }
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
