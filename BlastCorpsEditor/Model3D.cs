using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlastCorpsEditor
{
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

   class Model3D
   {
      const byte F3D_MOVEMEM = 0x03;
      const byte F3D_VTX = 0x04;
      const byte F3D_DL = 0x06;
      const byte F3D_QUAD = 0xB5;
      const byte F3D_CLRGEOMODE = 0xB6;
      const byte F3D_SETGEOMODE = 0xB7;
      const byte F3D_ENDDL = 0xB8;
      const byte F3D_TEXTURE = 0xBB;
      const byte F3D_TRI1 = 0xBF;
      const byte G_SETTILESIZE = 0xF2;
      const byte G_LOADBLOCK = 0xF3;
      const byte G_SETTILE = 0xF5;
      const byte G_SETFOGCOLOR = 0xF8;
      const byte G_SETENVCOLOR = 0xFB;
      const byte G_SETCOMBINE = 0xFC;
      const byte G_SETTIMG = 0xFD;

      public List<Texture> textures = new List<Texture>();
      public List<Triangle> triangles = new List<Triangle>();

      public Model3D(BlastCorpsLevel level)
      {
         parseModel(level);
      }

      public void parseModel(BlastCorpsLevel level)
      {
         byte[] displayList = level.displayList;
         Vertex[] vertexBuffer = new Vertex[16];
         Stack<uint> dlStack = new Stack<uint>();
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
                     if (nextTexture.width < 0)
                     {
                        nextTexture.width = (((displayList[offset + 5] << 8) | (displayList[offset + 6] & 0xF0)) >> 6) + 1;
                        nextTexture.height = (((displayList[offset + 6] & 0x0F) << 8 | displayList[offset + 7]) >> 2) + 1;
                     }
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
   }
}
