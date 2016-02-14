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

      static void Main(string[] args)
      {
         if (args.Length < 1)
         {
            System.Console.WriteLine("Usage: BlastCorpsConsole <DIR with .raw levels>");
            return;
         }
         string tmpDir = Path.GetTempPath();
         string inputDir = args[0];

         tmpDir = Path.Combine(tmpDir, "BlastCorps");
         System.Console.WriteLine("Saving generated levels to: " + tmpDir);

         Directory.CreateDirectory(tmpDir);

         foreach (BlastCorpsLevelMeta levelMeta in BlastCorpsRom.levelMeta)
         {
            string inputFile = Path.Combine(inputDir, levelMeta.filename + ".raw");
            string outputFile = Path.Combine(tmpDir, levelMeta.filename + ".raw");
            byte[] inData = File.ReadAllBytes(inputFile);
            BlastCorpsLevel level = BlastCorpsLevel.decodeLevel(inData, inData);
            byte[] outData = level.ToBytes();
            FileStream outStream = File.OpenWrite(outputFile);
            outStream.Write(outData, 0, outData.Length);
            outStream.Close();
            bool passed = true;
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
