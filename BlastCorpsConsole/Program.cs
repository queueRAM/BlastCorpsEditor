using System;
using System.IO;
using BlastCorpsEditor;

namespace BlastCorpsConsole
{
   class Program
   {
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
         System.Console.WriteLine("tmpDir: " + tmpDir);

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
            if (inData.Length != outData.Length)
            {
               System.Console.WriteLine("Error \"" + levelMeta.filename + "\" lengths differ: " + inData.Length + " -> " + outData.Length);
            }
            for (int i = 0; i < Math.Min(inData.Length, outData.Length); i++)
            {
               if (inData[i] != outData[i])
               {
                  System.Console.WriteLine("Error \"" + levelMeta.filename + "\" mismatch at " + i.ToString("X6"));
                  break;
               }
            }
         }
      }
   }
}
