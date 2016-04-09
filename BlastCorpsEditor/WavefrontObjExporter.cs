using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace BlastCorpsEditor
{
   class WavefrontObjExporter
   {
      public static void ExportTerrain(List<TerrainGroup> terrain, string filename, float scale)
      {
         using (System.IO.StreamWriter file = new System.IO.StreamWriter(filename))
         {
            int count = 0;
            int vertCount = 1;
            file.WriteLine(fileHeader());
            file.WriteLine("mtllib blast_corps.mtl");
            foreach (TerrainGroup tg in terrain)
            {
               file.WriteLine(String.Format("g Terrain{0:X2}", count));
               foreach (TerrainTri tri in tg.triangles)
               {
                  file.WriteLine(String.Format("usemtl Terrain{0:X02}{1:X02}", tri.b12, tri.b13));
                  file.WriteLine(toObjVert(tri.x1, tri.y1, tri.z1, scale));
                  file.WriteLine(toObjVert(tri.x2, tri.y2, tri.z2, scale));
                  file.WriteLine(toObjVert(tri.x3, tri.y3, tri.z3, scale));
                  file.WriteLine("f " + vertCount + " " + (vertCount + 1) + " " + (vertCount + 2));
                  vertCount += 3;
               }
               count++;
            }
         }
      }

      public static void ExportCollision(List<CollisionGroup> collision, string filename, float scale)
      {
         using (System.IO.StreamWriter file = new System.IO.StreamWriter(filename))
         {
            int count = 0;
            int vertCount = 1;
            file.WriteLine(fileHeader());
            file.WriteLine("mtllib blast_corps.mtl");
            foreach (CollisionGroup cg in collision)
            {
               file.WriteLine(String.Format("g Collision{0:X2}", count));
               foreach (CollisionTri tri in cg.triangles)
               {
                  file.WriteLine(String.Format("usemtl Collision{0:X02}", tri.b14));
                  file.WriteLine(toObjVert(tri.x1, tri.y1, tri.z1, scale));
                  file.WriteLine(toObjVert(tri.x2, tri.y2, tri.z2, scale));
                  file.WriteLine(toObjVert(tri.x3, tri.y3, tri.z3, scale));
                  file.WriteLine("f " + vertCount + " " + (vertCount + 1) + " " + (vertCount + 2));
                  vertCount += 3;
               }
               count++;
            }
         }
      }

      public static void ExportCollision(List<Collision24> collision, string filename, float scale)
      {
         using (System.IO.StreamWriter file = new System.IO.StreamWriter(filename))
         {
            int vertCount = 1;
            file.WriteLine(fileHeader());
            file.WriteLine("mtllib blast_corps.mtl");
            foreach (Collision24 tri in collision)
            {
               file.WriteLine(String.Format("usemtl Collision24_{0:X4}", tri.type));
               file.WriteLine(toObjVert(tri.x1, tri.y1, tri.z1, scale));
               file.WriteLine(toObjVert(tri.x2, tri.y2, tri.z2, scale));
               file.WriteLine(toObjVert(tri.x3, tri.y3, tri.z3, scale));
               file.WriteLine("f " + vertCount + " " + (vertCount + 1) + " " + (vertCount + 2));
               vertCount += 3;
            }
         }
      }

      public static void ExportObject60(List<Object60> object60s, string filename, float scale)
      {
         using (System.IO.StreamWriter file = new System.IO.StreamWriter(filename))
         {
            int vertCount = 1;
            file.WriteLine(fileHeader());
            foreach (Object60 obj in object60s)
            {
               file.WriteLine(toObjVert(obj.x - 2, obj.y, obj.z - 2, scale));
               file.WriteLine(toObjVert(obj.x + 2, obj.y, obj.z - 2, scale));
               file.WriteLine(toObjVert(obj.x + 2, obj.y + obj.h6, obj.z - 2, scale));
               file.WriteLine("f " + vertCount + " " + (vertCount + 1) + " " + (vertCount + 2));
               vertCount += 3;

               file.WriteLine(toObjVert(obj.x - 2, obj.y, obj.z - 2, scale));
               file.WriteLine(toObjVert(obj.x + 2, obj.y + obj.h6, obj.z - 2, scale));
               file.WriteLine(toObjVert(obj.x - 2, obj.y + obj.h6, obj.z - 2, scale));
               file.WriteLine("f " + vertCount + " " + (vertCount + 1) + " " + (vertCount + 2));
               vertCount += 3;
            }
         }
      }

      public static void ExportWalls(List<WallGroup> wallGroups, string filename, float scale)
      {
         using (System.IO.StreamWriter file = new System.IO.StreamWriter(filename))
         {
            int vertCount = 1;
            file.WriteLine(fileHeader());
            file.WriteLine("mtllib blast_corps.mtl");
            foreach (WallGroup group in wallGroups)
            {
               foreach (Wall wall in group.walls)
               {
                  file.WriteLine(String.Format("usemtl Wall{0:X4}", wall.type));
                  file.WriteLine(toObjVert(wall.x1, wall.y1, wall.z1, scale));
                  file.WriteLine(toObjVert(wall.x2, wall.y2, wall.z2, scale));
                  file.WriteLine(toObjVert(wall.x3, wall.y3, wall.z3, scale));
                  file.WriteLine("f " + vertCount + " " + (vertCount + 1) + " " + (vertCount + 2));
                  vertCount += 3;
               }
            }
         }
      }

      public static void ExportDisplayList(BlastCorpsRom rom, BlastCorpsLevel level, string filename, float scale)
      {
         Model3D model = new Model3D(level);

         string mtlFileName = filename + ".mtl";

         using (System.IO.StreamWriter file = new System.IO.StreamWriter(filename))
         {
            int vertCount = 1;
            file.WriteLine(fileHeader());
            file.WriteLine("mtllib {0}", Path.GetFileName(mtlFileName));
            foreach (Triangle tri in model.triangles)
            {
               float uScale = 32.0f * tri.texture.width;
               float vScale = 32.0f * tri.texture.height;
               file.WriteLine();
               file.WriteLine("usemtl Texture{0:X4}", tri.texture.address);
               file.WriteLine(toObjVert(tri.vertices[0], scale, uScale, vScale));
               file.WriteLine(toObjVert(tri.vertices[1], scale, uScale, vScale));
               file.WriteLine(toObjVert(tri.vertices[2], scale, uScale, vScale));
               file.WriteLine("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}", vertCount, (vertCount + 1), (vertCount + 2));
               vertCount += 3;
            }
         }

         // create textures directory
         string textureDirName = "textures";
         string textureDir = Path.Combine(Path.GetDirectoryName(filename), textureDirName);
         Directory.CreateDirectory(textureDir);
         using (System.IO.StreamWriter file = new System.IO.StreamWriter(mtlFileName))
         {
            List<uint> processedTextures = new List<uint>();
            file.WriteLine(fileHeader());
            foreach (Texture t in model.textures)
            {
               if (!processedTextures.Contains(t.address))
               {
                  string textureFilename = String.Format("{0:X4}.png", t.address);
                  string textureFile = String.Format(textureDirName + "/" + textureFilename);
                  file.WriteLine("newmtl Texture{0:X4}", t.address);
                  file.WriteLine("Ka 0.0 0.0 0.0"); // ambiant color
                  file.WriteLine("Kd 1.0 1.0 1.0"); // diffuse color
                  file.WriteLine("Ks 0.3 0.3 0.3"); // specular color
                  file.WriteLine("d 1");            // dissolved
                  file.WriteLine("map_Kd {0}", textureFile);
                  file.WriteLine();

                  BlastCorpsTexture bct = new BlastCorpsTexture(rom.GetRawData(), t.address, 0);
                  bct.decode();
                  byte[] n64Texture = bct.GetInflated();
                  N64Format format = N64Format.RGBA;
                  int depth = 16;

                  switch (bct.type)
                  {
                     case 0: // IA8?
                        // TODO: memcpy, no info
                        format = N64Format.IA;
                        depth = 8;
                        break;
                     case 1: // RBGA16?
                        format = N64Format.RGBA;
                        depth = 16;
                        break;
                     case 2: // RGBA32?
                        format = N64Format.RGBA;
                        depth = 32;
                        break;
                     case 3: // IA8?
                        format = N64Format.IA;
                        depth = 8;
                        break;
                     case 4: // IA16?
                        format = N64Format.IA;
                        depth = 16;
                        break;
                     case 5: // RGBA32?
                        format = N64Format.RGBA;
                        depth = 32;
                        break;
                     case 6: // IA8?
                        format = N64Format.IA;
                        depth = 8;
                        break;
                  }

                  Bitmap loadedBitmap;
                  switch (format)
                  {
                     case N64Format.RGBA:
                        loadedBitmap = N64Graphics.RGBA(n64Texture, t.width, t.height, depth);
                        break;
                     case N64Format.IA:
                     default:
                        loadedBitmap = N64Graphics.IA(n64Texture, t.width, t.height, depth);
                        break;
                  }
                  loadedBitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
                  loadedBitmap.Save(Path.Combine(textureDir, textureFilename));

                  processedTextures.Add(t.address);
               }
            }
         }
      }

      private static string toObjVert(int x, int y, int z, float scale)
      {
         float fx, fy, fz;
         fx = (float)x / scale;
         fy = (float)y / scale;
         fz = (float)z / scale;
         return "v " + fx + " " + fy + " " + fz;
      }

      private static string toObjVert(Vertex vert, float scale, float uScale, float vScale)
      {
         const float normalScale = 127.0f;
         float fx, fy, fz;
         fx = (float)vert.x / scale;
         fy = (float)vert.y / scale;
         fz = (float)vert.z / scale;
         string vertData = "v " + fx + " " + fy + " " + fz;
         fx = (float)vert.u / uScale;
         fy = (float)vert.v / vScale;
         string textureData = "vt " + fx + " " + fy;
         fx = (float)vert.normals[0] / normalScale;
         fy = (float)vert.normals[1] / normalScale;
         fz = (float)vert.normals[2] / normalScale;
         string normalData = "vn " + fx + " " + fy + " " + fz;
         return vertData + Environment.NewLine + textureData + Environment.NewLine + normalData;
      }

      private static string fileHeader()
      {
         var appName = "Blast Corps Editor";
         var version = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).ProductVersion;
         return "# Generated by " + appName + " v" + version;
      }
   }
}
