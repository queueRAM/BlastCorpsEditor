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
            file.WriteLine("mtllib blast_corps_terrain.mtl");
            foreach (TerrainGroup tg in terrain)
            {
               file.WriteLine("g Terrain" + count);
               foreach (TerrainTri tri in tg.triangles)
               {
                  file.WriteLine("usemtl Terrain" + tri.b12);
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
            file.WriteLine("mtllib blast_corps_collision.mtl");
            foreach (CollisionGroup cg in collision)
            {
               file.WriteLine("g Collision" + count);
               foreach (CollisionTri tri in cg.triangles)
               {
                  file.WriteLine("usemtl Collision" + tri.b14);
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
            file.WriteLine("mtllib blast_corps_collision.mtl");
            foreach (Collision24 tri in collision)
            {
               file.WriteLine("usemtl Collision" + tri.type);
               file.WriteLine(toObjVert(tri.x1, tri.y1, tri.z1, scale));
               file.WriteLine(toObjVert(tri.x2, tri.y2, tri.z2, scale));
               file.WriteLine(toObjVert(tri.x3, tri.y3, tri.z3, scale));
               file.WriteLine("f " + vertCount + " " + (vertCount + 1) + " " + (vertCount + 2));
               vertCount += 3;
            }
         }
      }

      public class Vertex
      {
         public enum Type { RGB, XYZ }
         public int x, y, z;
         public int u, v;
         public byte[] normals = new byte[3];
         public Type type;
         byte a;
         public Vertex(byte[] data, uint offset)
         {
            x = BE.I16(data, offset);
            y = BE.I16(data, offset + 0x2);
            z = BE.I16(data, offset + 0x4);
            // skip 6-7
            u = BE.I16(data, offset + 0x8);
            v = BE.I16(data, offset + 0xA);
            normals[0] = data[offset + 0xC];
            normals[1] = data[offset + 0xD];
            normals[2] = data[offset + 0xE];
            a = data[offset + 0xF];
         }
      }

      public class Triangle
      {
         public Vertex[] vertices = new Vertex[3];
         public Texture texture;
         public Triangle()
         {

         }
         public Triangle(Vertex v0, Vertex v1, Vertex v2)
         {
            vertices[0] = v0;
            vertices[1] = v1;
            vertices[2] = v2;
         }
      }

      public class Texture
      {
         public UInt32 address;
         public int width;
         public int height;
         public Texture()
         {
            address = 0xFFFFFFFF;
            width = -1;
            height = -1;
         }
         public bool IsComplete()
         {
            return (width != -1 && height != -1 && address != 0xFFFFFFFF);
         }
      }

      private static void LoadVertices(byte[] segmentData, uint segOffset, int index, int count, Vertex[] vertexBuffer)
      {
         for (uint i = 0; i < count; i++)
         {
            if (i + index < vertexBuffer.Length)
            {
               vertexBuffer[i + index] = new Vertex(segmentData, segOffset + i * 0x10);
            }
         }
      }

      public static void ExportDisplayList(BlastCorpsRom rom, BlastCorpsLevel level, string filename, float scale)
      {
         const byte F3D_MOVEMEM    = 0x03;
         const byte F3D_VTX        = 0x04;
         const byte F3D_DL         = 0x06;   
         const byte F3D_QUAD       = 0xB5;
         const byte F3D_CLRGEOMODE = 0xB6;
         const byte F3D_SETGEOMODE = 0xB7;
         const byte F3D_ENDDL      = 0xB8;
         const byte F3D_TEXTURE    = 0xBB;
         const byte F3D_TRI1       = 0xBF;
         const byte G_SETTILESIZE  = 0xF2;
         const byte G_LOADBLOCK    = 0xF3;
         const byte G_SETTILE      = 0xF5;
         const byte G_SETFOGCOLOR  = 0xF8;
         const byte G_SETENVCOLOR  = 0xFB;
         const byte G_SETCOMBINE   = 0xFC;
         const byte G_SETTIMG      = 0xFD;

         byte[] displayList = level.displayList;
         Vertex[] vertexBuffer = new Vertex[16];
         List<Triangle> triangles = new List<Triangle>();
         Stack<uint> dlStack = new Stack<uint>();
         List<Texture> textures = new List<Texture>();
         uint segAddress;
         uint segOffset;
         byte bank;
         Texture nextTexture = new Texture();
         Triangle nextTri = new Triangle();
         List<UInt32> displayLists = new List<UInt32>();
         displayLists.Add(level.header.offsets[(0x88 - 0x20) / 4]);
         displayLists.Add(level.header.offsets[(0x90 - 0x20) / 4]);
         displayLists.Add(level.header.offsets[(0x94 - 0x20) / 4]);
         displayLists.Add(level.header.offsets[(0x98 - 0x20) / 4]);
         displayLists.Add(level.header.offsets[(0x9C - 0x20) / 4]);

         using (System.IO.StreamWriter file = new System.IO.StreamWriter(filename))
         {
            foreach (uint startOffset in displayLists)
            {
               bool done = false;
               for (uint offset = startOffset - level.header.dlOffset; offset < displayList.Length && !done; offset += 8)
               {
                  UInt32 w0 = BE.U32(displayList, offset);
                  UInt32 w1 = BE.U32(displayList, offset + 4);
                  byte command = displayList[offset];
                  switch (command)
                  {
                     case F3D_MOVEMEM:
                        break;
                     case F3D_VTX:
                        int count = ((displayList[offset + 1] >> 4) & 0xF) + 1;
                        int index = (displayList[offset + 1]) & 0xF;
                        segAddress = w1;
                        bank = displayList[offset + 4];
                        segOffset = segAddress & 0x00FFFFFF;
                        LoadVertices(level.vertData, segOffset, index, count, vertexBuffer);
                        break;
                     case F3D_DL:
                        segAddress = w1;
                        bank = displayList[offset + 4];
                        segOffset = segAddress & 0x00FFFFFF;
                        dlStack.Push(offset);
                        offset = segOffset - 8; // subtract 8 since for loop will increment by 8
                        break;
                     case F3D_QUAD:
                        break;
                     case F3D_CLRGEOMODE:
                        break;
                     case F3D_SETGEOMODE:
                        break;
                     case F3D_ENDDL:
                        if (dlStack.Count == 0)
                        {
                           done = true;
                        }
                        else
                        {
                           offset = dlStack.Pop();
                        }
                        break;
                     case F3D_TEXTURE:
                        // reset tile sizes
                        nextTexture = new Texture();
                        break;
                     case F3D_TRI1:
                        int vertex0 = displayList[offset + 5] / 0x0A;
                        int vertex1 = displayList[offset + 6] / 0x0A;
                        int vertex2 = displayList[offset + 7] / 0x0A;
                        Triangle tri = new Triangle(vertexBuffer[vertex0], vertexBuffer[vertex1], vertexBuffer[vertex2]);
                        tri.texture = nextTexture;
                        triangles.Add(tri);
                        break;
                     case G_SETTILESIZE:
                        nextTexture.width = (((displayList[offset + 5] << 8) | (displayList[offset + 6] & 0xF0)) >> 6) + 1;
                        nextTexture.height = (((displayList[offset + 6] & 0x0F) << 8 | displayList[offset + 7]) >> 2) + 1;
                        if (nextTexture.IsComplete())
                        {
                           textures.Add(nextTexture);
                        }
                        break;
                     case G_LOADBLOCK:
                        break;
                     case G_SETTILE:
                        break;
                     case G_SETFOGCOLOR:
                        break;
                     case G_SETENVCOLOR:
                        break;
                     case G_SETCOMBINE:
                        break;
                     case G_SETTIMG:
                        segAddress = w1;
                        bank = displayList[offset + 4];
                        segOffset = segAddress & 0x00FFFFFF;
                        nextTexture.address = segAddress;
                        if (nextTexture.IsComplete())
                        {
                           textures.Add(nextTexture);
                        }
                        break;
                  }
               }
            }
         }

         string mtlFileName = filename + ".mtl";

         using (System.IO.StreamWriter file = new System.IO.StreamWriter(filename))
         {
            int vertCount = 1;
            file.WriteLine(fileHeader());
            file.WriteLine("mtllib {0}", Path.GetFileName(mtlFileName));
            foreach (Triangle tri in triangles)
            {
               file.WriteLine();
               file.WriteLine("usemtl Texture{0:X4}", tri.texture.address);
               file.WriteLine(toObjVert(tri.vertices[0], scale));
               file.WriteLine(toObjVert(tri.vertices[1], scale));
               file.WriteLine(toObjVert(tri.vertices[2], scale));
               file.WriteLine("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}", vertCount, (vertCount + 1), (vertCount + 2));
               vertCount += 3;
            }
         }

         using (System.IO.StreamWriter file = new System.IO.StreamWriter(mtlFileName))
         {
            List<uint> processedTextures = new List<uint>();
            file.WriteLine(fileHeader());
            foreach (Texture t in textures)
            {
               if (!processedTextures.Contains(t.address))
               {
                  string textureFilename = String.Format("{0:X4}.png", t.address);
                  string textureFile = String.Format("textures/" + textureFilename);
                  file.WriteLine("newmtl Texture{0:X4}", t.address);
                  file.WriteLine("Ka 1.0 1.0 1.0"); // ambiant color
                  file.WriteLine("Kd 1.0 1.0 1.0"); // diffuse color
                  file.WriteLine("Ks 0.4 0.4 0.4"); // specular color
                  file.WriteLine("Ns 0");           // specular exponent
                  file.WriteLine("d 1");            // dissolved
                  file.WriteLine("Tr 1");           // inverted
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
                     case 3: // IA16?
                        format = N64Format.IA;
                        depth = 16;
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
                  loadedBitmap.Save(Path.Combine(Path.GetDirectoryName(filename), "textures", textureFilename));

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

      private static string toObjVert(Vertex vert, float scale)
      {
         const float textureScale = 1024.0f;
         const float normalScale = 127.0f;
         float fx, fy, fz;
         fx = (float)vert.x / scale;
         fy = (float)vert.y / scale;
         fz = (float)vert.z / scale;
         string vertData = "v " + fx + " " + fy + " " + fz;
         fx = (float)vert.u / textureScale;
         fy = (float)vert.v / textureScale;
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
