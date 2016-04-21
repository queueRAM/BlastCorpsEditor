using System;
using System.Collections.Generic;
using System.IO;

namespace BlastCorpsEditor
{
   public class LevelHeader
   {
      public UInt16[] u16s = new UInt16[12];
      public Int32 gravity;
      public Int32 u1C; // TODO: identify
      public UInt32[] offsets = new UInt32[42];
      public UInt32 dlOffset;
   }

   public class BlastCorpsItem
   {
      public Int16 x { get; set; }
      public Int16 y { get; set; }
      public Int16 z { get; set; }

      public override string ToString()
      {
         return "(" + x + ", " + y + ", " + z + ")";
      }
   }

   public class AmmoBox : BlastCorpsItem
   {
      public UInt16 type;

      public AmmoBox(Int16 x, Int16 y, Int16 z, UInt16 type)
      {
         this.x = x;
         this.y = y;
         this.z = z;
         this.type = type;
      }

      public override string ToString()
      {
         return base.ToString() + ", " + type;
      }
   }

   public class Collision24
   {
      public Int16 x1, y1, z1;
      public Int16 x2, y2, z2;
      public Int16 x3, y3, z3;
      public UInt16 type;

      public Collision24(Int16 x1, Int16 y1, Int16 z1, Int16 x2, Int16 y2, Int16 z2, Int16 x3, Int16 y3, Int16 z3, UInt16 t)
      {
         this.x1 = x1;
         this.y1 = y1;
         this.z1 = z1;
         this.x2 = x2;
         this.y2 = y2;
         this.z2 = z2;
         this.x3 = x3;
         this.y3 = y3;
         this.z3 = z3;
         this.type = t;
      }

      public override string ToString()
      {
         return "(" + x1 + "," + y1 + "," + z1 + ") (" + x2 + "," + y2 + "," + z2 + ") (" + x3 + "," + y3 + "," + z3 + ")";
      }
   }

   public class CommPoint : BlastCorpsItem
   {
      public UInt16 h6;

      public CommPoint(Int16 x, Int16 y, Int16 z, UInt16 h6)
      {
         this.x = x;
         this.y = y;
         this.z = z;
         this.h6 = h6;
      }

      public override string ToString()
      {
         return base.ToString() + ", " + h6;
      }
   }

   public class AnimatedTexture
   {
      // TODO: details on data members
      public UInt32 w0;
      public byte b5, b6, b7, b8, b9, bA, bB;
      public UInt32[] textureIndexes;

      public AnimatedTexture(UInt32 w0, byte b5, byte b6, byte b7, byte b8, byte b9, byte bA, byte bB, UInt32[] textureIndexes)
      {
         this.w0 = w0;
         this.b5 = b5;
         this.b6 = b6;
         this.b7 = b7;
         this.b8 = b8;
         this.b9 = b9;
         this.bA = bA;
         this.bB = bB;
         this.textureIndexes = textureIndexes;
      }

      public override string ToString()
      {
         string indexStr = "";
         if (textureIndexes.Length > 0)
         {
            indexStr = String.Format("{0:X8}", textureIndexes[0]);
            for (int i = 1; i < textureIndexes.Length; i++)
            {
               indexStr += String.Format(", {0:X8}", textureIndexes[i]);
            }
         }
         return String.Format("{0:X8} {1:X2} {2:X2} {3:X2} {4:X2} {5:X2} {6:X2} {7:X2} {8:X2} [{9}]",
            w0, (textureIndexes.Length + 1), b5, b6, b7, b8, b9, bA, bB, indexStr);
      }
   }

   public class TerrainTri
   {
      public Int16 x1, y1, z1;
      public Int16 x2, y2, z2;
      public Int16 x3, y3, z3;
      // TODO: what are these?
      public byte b12, b13;

      public TerrainTri(Int16 x1, Int16 y1, Int16 z1, Int16 x2, Int16 y2, Int16 z2, Int16 x3, Int16 y3, Int16 z3, byte b12, byte b13)
      {
         this.x1 = x1;
         this.y1 = y1;
         this.z1 = z1;
         this.x2 = x2;
         this.y2 = y2;
         this.z2 = z2;
         this.x3 = x3;
         this.y3 = y3;
         this.z3 = z3;
         this.b12 = b12;
         this.b13 = b13;
      }

      public override string ToString()
      {
         return "(" + x1 + "," + y1 + "," + z1 + ") (" + x2 + "," + y2 + "," + z2 + ") (" + x3 + "," + y3 + "," + z3 + ") " + b12 + ":" + b13;
      }
   }

   public class TerrainGroup
   {
      public List<TerrainTri> triangles = new List<TerrainTri>();
   }

   public class LevelBounds
   {
      public Int16 x1, z1, x2, z2;

      public override string ToString()
      {
         return "(" + x1 + ", " + z1 + "), (" + x2 + ", " + z2 + ")";
      }
   }

   public class RDU : BlastCorpsItem
   {
      public RDU(Int16 x, Int16 y, Int16 z)
      {
         this.x = x;
         this.y = y;
         this.z = z;
      }
   }

   public class TNTCrate : BlastCorpsItem
   {
      public byte texture;
      public byte timer;
      public Int16 h8; // TODO
      public UInt16 power;

      public TNTCrate(Int16 x, Int16 y, Int16 z, byte texture, byte timer, Int16 h8, UInt16 power)
      {
         this.x = x;
         this.y = y;
         this.z = z;
         this.texture = texture;
         this.timer = timer;
         this.h8 = h8;
         this.power = power;
      }

      public override string ToString()
      {
         return base.ToString() + ", " + texture.ToString("X2") + ", " + timer.ToString("X2") + ", " + h8.ToString("X4") + ", " + power.ToString("X4");
      }
   }

   public class SquareBlock : BlastCorpsItem
   {
      public enum Type { Block, Hole }
      public enum Shape { Square, Diamond1, Diamond2 }
      public Type type;
      public Shape shape;
      public bool isCounted;
      public class Node
      {
         public Int16[] x;
         public Int16[] y;
         public Int16[] z;
         public byte[] other;

         public Node(Int16 x1, Int16 y1, Int16 z1, Int16 x2, Int16 y2, Int16 z2, Int16 x3, Int16 y3, Int16 z3, byte[] other)
         {
            x = new Int16[] { x1, x2, x3 };
            y = new Int16[] { y1, y2, y3 };
            z = new Int16[] { z1, z2, z3 };
            this.other = other;
         }
         public Node(Int16 x1, Int16 y1, Int16 z1, Int16 x2, Int16 y2, Int16 z2, Int16 x3, Int16 y3, Int16 z3) : this(x1, y1, z1, x2, y2, z2, x3, y3, z3, new byte[4])
         {
         }

         public override string ToString()
         {
            string byteStr = string.Format("{0:X02} {1:X02} {2:X02} {3:X02}", other[0], other[1], other[2], other[3]);
            return string.Format("({0}, {1}, {2}), ({3}, {4}, {5}), ({6}, {7}, {8}), {9}", x[0], y[0], z[0], x[1], y[1], z[1], x[2], y[2], z[2], byteStr);
         }
      }
      public List<Node> nodes = new List<Node>();

      public SquareBlock(Int16 x, Int16 y, Int16 z, byte type1, byte type2)
      {
         this.x = x;
         this.y = y;
         this.z = z;
         if (type2 == 8)
         {
            this.type = Type.Hole;
            switch (type1)
            {
               case 0: this.shape = Shape.Square; break;
               case 1: this.shape = Shape.Diamond1; break;
               case 2: this.shape = Shape.Diamond2; break;
            }
         }
         else
         {
            this.type = Type.Block;
            switch (type2)
            {
               case 0: this.shape = Shape.Square; break;
               case 1: this.shape = Shape.Diamond1; break;
               case 2: this.shape = Shape.Diamond2; break;
            }
         }
      }

      public SquareBlock(Int16 x, Int16 y, Int16 z, Type type, Shape shape)
      {
         this.x = x;
         this.y = y;
         this.z = z;
         this.type = type;
         this.shape = shape;
      }

      public byte Type1()
      {
         if (type == Type.Block)
         {
            return 0;
         }
         else
         {
            switch (shape)
            {
               case Shape.Square: return 0;
               case Shape.Diamond1: return 1;
               case Shape.Diamond2: return 2;
            }
         }
         return 0;
      }

      public byte Type2()
      {
         if (type == Type.Hole)
         {
            return 8;
         }
         else
         {
            switch (shape)
            {
               case Shape.Square: return 0;
               case Shape.Diamond1: return 1;
               case Shape.Diamond2: return 2;
            }
         }
         return 0;
      }

      public UInt16 GetCount()
      {
         return (UInt16)(isCounted ? 1 : 0);
      }

      public void addNode(Int16 x1, Int16 y1, Int16 z1, Int16 x2, Int16 y2, Int16 z2, Int16 x3, Int16 y3, Int16 z3, byte[] data, int index)
      {
         Node n = new Node(x1, y1, z1, x2, y2, z2, x3, y3, z3);
         Array.Copy(data, index, n.other, 0, 4);
         nodes.Add(n);
      }

      static readonly Node[][] nodeOffsets = new Node[][] {
         new Node[] {
            new Node( 38, -15, -36, -40, -15, -36, -40, 15, -36, new byte[] { 0x07, 0xFD, 0x01, 0x00 }),
            new Node( 38,  15, -36,  38, -15, -36, -40, 15, -36, new byte[] { 0x07, 0xFD, 0x01, 0x00 }),
            new Node( 38, -15,  42,  38, -15, -36,  38, 15, -36, new byte[] { 0x0B, 0xFF, 0x00, 0x00 }),
            new Node( 38,  15,  42,  38, -15,  42,  38, 15, -36, new byte[] { 0x0B, 0xFF, 0x00, 0x00 }),
            new Node(-40, -15, -36, -40, -15,  42, -40, 15,  42, new byte[] { 0x0B, 0xFE, 0x01, 0x00 }),
            new Node(-40,  15, -36, -40, -15, -36, -40, 15,  42, new byte[] { 0x0B, 0xFE, 0x01, 0x00 }),
            new Node(-40, -15,  42,  38, -15,  42,  38, 15,  42, new byte[] { 0x07, 0xFD, 0x00, 0x00 }),
            new Node(-40,  15,  42, -40, -15,  42,  38, 15,  42, new byte[] { 0x07, 0xFD, 0x00, 0x00 })
         },
         new Node[] {
            new Node(  1, -15, -45, -44, -15,   0, -44, 15,   0, new byte[] { 0x09, 0xFC, 0x01, 0x00 }),
            new Node(  1,  15, -45,   1, -15, -45, -44, 15,   0, new byte[] { 0x09, 0xFC, 0x01, 0x00 }),
            new Node( 45, -15,   0,   1, -15, -45,   1, 15, -45, new byte[] { 0x0D, 0xFF, 0x00, 0x00 }),
            new Node( 45,  15,   0,  45, -15,   0,   1, 15, -45, new byte[] { 0x0D, 0xFF, 0x00, 0x00 }),
            new Node(-44, -15,   0,   0, -15,  44,   0, 15,  44, new byte[] { 0x0D, 0xFE, 0x01, 0x00 }),
            new Node(-44,  15,   0, -44, -15,   0,   0, 15,  44, new byte[] { 0x0D, 0xFE, 0x01, 0x00 }),
            new Node(  0, -15,  44,  45, -15,   0,  45, 15,   0, new byte[] { 0x09, 0xFD, 0x00, 0x00 }),
            new Node(  0,  15,  44,   0, -15,  44,  45, 15,   0, new byte[] { 0x09, 0xFD, 0x00, 0x00 })
         },
         new Node[] {
            new Node( -9, -30, -55, -64, -30,   0, -64,  0,   0, new byte[] { 0x09, 0xFD, 0x01, 0x00 }),
            new Node( -9,   0, -55,  -9, -30, -55, -64,  0,   0, new byte[] { 0x09, 0xFD, 0x01, 0x00 }),
            new Node( 46,   0,   0,  48,   0, -51,  48, 30, -51, new byte[] { 0x0B, 0xE6, 0x00, 0x00 }),
            new Node( 46,  30,   0,  46,   0,   0,  48, 30, -51, new byte[] { 0x0B, 0xE6, 0x00, 0x00 }),
            new Node(-64, -30,   0,  -9, -30,  55,  -9,  0,  55, new byte[] { 0x0D, 0xFE, 0x01, 0x00 }),
            new Node(-64,   0,   0, -64, -30,   0,  -9,  0,  55, new byte[] { 0x0D, 0xFE, 0x01, 0x00 }),
            new Node( 48,   0,  49,  46,   0,   0,  46, 30,   0, new byte[] { 0x0C, 0x19, 0x00, 0x00 }),
            new Node( 48,  30,  49,  48,   0,  49,  46, 30,   0, new byte[] { 0x0C, 0x19, 0x00, 0x00 })
         }
      };

      // update nodes' positions depending on hole type and position
      public void computeNodes()
      {
         if (type == Type.Hole)
         {
            nodes.Clear();
            byte holeType = Type1();
            if (Type1() < nodeOffsets.Length)
            {
               foreach (var n in nodeOffsets[holeType])
               {
                  nodes.Add(new Node((Int16)(x + n.x[0]), (Int16)(y + n.y[0]), (Int16)(z + n.z[0]), (Int16)(x + n.x[1]), (Int16)(y + n.y[1]), (Int16)(z + n.z[1]), (Int16)(x + n.x[2]), (Int16)(y + n.y[2]), (Int16)(z + n.z[2]), n.other));
               }
            }
         }
      }

      public override string ToString()
      {
         return base.ToString() + ", " + Type1() + ", " + Type2() + ((type == Type.Hole) ? (", " + isCounted) : "");
      }

      public string ToStringFull()
      {
         string retString = base.ToString() + ", " + type + ", " + Type2() + ((type == Type.Hole) ? (", " + isCounted) : "");
         foreach (Node node in nodes)
         {
            retString += string.Format("\n  {0}", node) + string.Format("  ({0}, {1}, {2}), ({3}, {4}, {5}), ({6}, {7}, {8})", node.x[0] - x, node.y[0] - y, node.z[0] - z, node.x[1] - x, node.y[1] - y, node.z[1] - z, node.x[2] - x, node.y[2] - y, node.z[2] - z);
         }
         return retString;
      }
   }

   public class Bounds
   {
      public Int16 x1, z1, x2, z2;
      public UInt16 todo;

      public Bounds(Int16 x1, Int16 z1, Int16 x2, Int16 z2, UInt16 todo)
      {
         this.x1 = x1;
         this.z1 = z1;
         this.x2 = x2;
         this.z2 = z2;
         this.todo = todo;
      }

      public override string ToString()
      {
         return "(" + x1 + ", " + z1 + "), (" + x2 + ", " + z2 + "), " + todo;
      }
   }

   public class Object58 : BlastCorpsItem
   {
      public UInt16 h6;

      public Object58(Int16 x, Int16 y, Int16 z, UInt16 h6)
      {
         this.x = x;
         this.y = y;
         this.z = z;
         this.h6 = h6;
      }

      public override string ToString()
      {
         return base.ToString() + ", " + h6;
      }
   }

   public class Vehicle : BlastCorpsItem
   {
      public byte type;
      public Int16 heading;

      public Vehicle(byte type, Int16 x, Int16 y, Int16 z, Int16 heading)
      {
         this.type = type;
         this.x = x;
         this.y = y;
         this.z = z;
         this.heading = heading;
      }

      public override string ToString()
      {
         return type.ToString("X2") + ", " + base.ToString() + ", " + heading;
      }
   }

   public class Carrier : BlastCorpsItem
   {
      public byte speed {get; set;}
      public UInt16 heading {get; set;}
      public UInt16 distance { get; set; }

      public override string ToString()
      {
         return speed + ", " + base.ToString() + ", " + heading;
      }
   }

   public class Building : BlastCorpsItem
   {
      public UInt16 type;
      public bool isCounted;
      public byte b9; // TODO
      public UInt16 movement;
      public UInt16 speed;

      public Building(Int16 x, Int16 y, Int16 z, UInt16 type, byte counter, byte b9, UInt16 movement, UInt16 speed)
      {
         this.x = x;
         this.y = y;
         this.z = z;
         this.type = type;
         this.isCounted = counter > 0;
         this.b9 = b9;
         this.movement = movement;
         this.speed = speed;
      }

      public override string ToString()
      {
         return base.ToString() + ", " + type + ", " + isCounted + ", " + b9 + ", " + movement + ", " + speed;
      }
   }

   public class Object60 : BlastCorpsItem
   {
      public Int16 h6;
      public byte b8;
      public byte[] values;
      public byte[] lastTwo = new byte[2];

      public Object60(Int16 x, Int16 y, Int16 z, Int16 h6, byte b8, byte count, byte[] data, uint remainingIdx)
      {
         this.x = x;
         this.y = y;
         this.z = z;
         this.h6 = h6;
         this.b8 = b8;
         values = new byte[count];
         for (int i = 0; i < values.Length; i++)
         {
            values[i] = data[i + remainingIdx];
         }
         lastTwo[0] = data[count + remainingIdx];
         lastTwo[1] = data[count + remainingIdx + 1];
      }

      public override string ToString()
      {
         return base.ToString() + ", " + h6 + ", " + b8 + ", " + values.Length + ", " + values + ", " + lastTwo;
      }
   }

   public class Wall
   {
      public Int16 x1, y1, z1;
      public Int16 x2, y2, z2;
      public Int16 x3, y3, z3;
      public UInt16 type;

      public Wall(Int16 x1, Int16 y1, Int16 z1, Int16 x2, Int16 y2, Int16 z2, Int16 x3, Int16 y3, Int16 z3, UInt16 t)
      {
         this.x1 = x1;
         this.y1 = y1;
         this.z1 = z1;
         this.x2 = x2;
         this.y2 = y2;
         this.z2 = z2;
         this.x3 = x3;
         this.y3 = y3;
         this.z3 = z3;
         this.type = t;
      }

      public override string ToString()
      {
         return "(" + x1 + "," + y1 + "," + z1 + ") (" + x2 + "," + y2 + "," + z2 + ") (" + x3 + "," + y3 + "," + z3 + ") " + type;
      }
   }

   public class WallGroup
   {
      public byte b0;
      public byte[] header;
      public List<Wall> walls = new List<Wall>();

      public WallGroup(byte b0, byte[] header)
      {
         this.b0 = b0;
         this.header = header;
      }
   }

   public class TrainPlatform
   {
      public struct StoppingTriangle
      {
         public Int16 x1, x2, x3, z1, z2, z3;
      }
      public struct PlatformCollision
      {
         public Int16 x1, x2, x3, y1, y2, y3, z1, z2, z3;
         // TODO: what are these?
         public UInt16 h12;
         public byte b14;
      }
      public byte b0;
      public byte b1;
      public StoppingTriangle[] stoppingZone;
      public UInt32 word;
      public PlatformCollision[] collision;
      public byte[] someList;
   }

   public class CollisionTri
   {
      public Int16 x1, y1, z1;
      public Int16 x2, y2, z2;
      public Int16 x3, y3, z3;
      // TODO: what are these?
      public UInt16 h12;
      public byte b14, b15;

      public CollisionTri(Int16 x1, Int16 y1, Int16 z1, Int16 x2, Int16 y2, Int16 z2, Int16 x3, Int16 y3, Int16 z3, UInt16 h12, byte b14, byte b15)
      {
         this.x1 = x1;
         this.y1 = y1;
         this.z1 = z1;
         this.x2 = x2;
         this.y2 = y2;
         this.z2 = z2;
         this.x3 = x3;
         this.y3 = y3;
         this.z3 = z3;
         this.h12 = h12;
         this.b14 = b14;
         this.b15 = b15;
      }

      public override string ToString()
      {
         return "(" + x1 + "," + y1 + "," + z1 + "), (" + x2 + "," + y2 + "," + z2 + "), (" + x3 + "," + y3 + "," + z3 + "), " + h12 + ", " + b14 + ", " + b15;
      }
   }

   public class CollisionGroup
   {
      public List<CollisionTri> triangles = new List<CollisionTri>();
   }

   public class BlastCorpsLevel
   {
      public LevelHeader header = new LevelHeader();
      public List<AmmoBox> ammoBoxes = new List<AmmoBox>();
      public List<Collision24> collision24 = new List<Collision24>();
      public List<CommPoint> commPoints = new List<CommPoint>();
      public List<AnimatedTexture> animatedTextures = new List<AnimatedTexture>();
      public List<TerrainGroup> terrainGroups = new List<TerrainGroup>();
      public List<RDU> rdus = new List<RDU>();
      public List<TNTCrate> tntCrates = new List<TNTCrate>();
      public List<SquareBlock> squareBlocks = new List<SquareBlock>();
      public List<Bounds> bounds40 = new List<Bounds>();
      public List<Bounds> bounds44 = new List<Bounds>();
      public LevelBounds bounds = new LevelBounds();
      public List<Vehicle> vehicles = new List<Vehicle>();
      public Carrier carrier = new Carrier();
      public List<Object58> object58s = new List<Object58>();
      public List<Building> buildings = new List<Building>();
      public List<Object60> object60s = new List<Object60>();
      public byte object60b0;
      public List<WallGroup> wallGroups = new List<WallGroup>();
      private byte[] copy48;
      public List<TrainPlatform> trainPlatforms = new List<TrainPlatform>();
      public List<CollisionGroup> collision6C = new List<CollisionGroup>();
      public List<CollisionGroup> collision70 = new List<CollisionGroup>();
      private byte[] copy74;
      public byte[] vertData;
      public byte[] copyLevelData;
      public byte[] displayList;

      // 0x00-0x20: Header
      private void decodeHeader(byte[] data)
      {
         // save offset of display list which will be used for header offset 0x78 and up
         header.dlOffset = (UInt32)data.Length;

         // read in 12 u16s
         for (uint i = 0; i < header.u16s.Length; i++)
         {
            header.u16s[i] = BE.U16(data, i*2);
         }
         // read in two signed 32 values
         header.gravity = BE.I32(data, 0x18);
         header.u1C = BE.I32(data, 0x1C);
         // read in section offsets
         for (uint i = 0; i < header.offsets.Length; i++)
         {
            header.offsets[i] = BE.U32(data, 0x20 + i * 4);
         }
      }

      // 0x20: Ammo
      // [XX XX] [YY YY] [ZZ ZZ] [TT TT]
      private void decodeAmmoBoxes(byte[] data)
      {
         uint start = BE.U32(data, 0x20);
         uint end = BE.U32(data, 0x24);
         for (uint idx = start; idx < end; idx += 8)
         {
            Int16 x, y, z;
            UInt16 t;
            x = BE.I16(data, idx);
            y = BE.I16(data, idx + 2);
            z = BE.I16(data, idx + 4);
            t = BE.U16(data, idx + 6);
            AmmoBox ammo = new AmmoBox(x, y, z, t);
            ammoBoxes.Add(ammo);
         }
      }

      // 0x24: Collision triangles
      // [X1 X1] [Y1 Y1] [Z1 Z1] [X2 X2] [Y2 Y2] [Z2 Z2] [X3 X3] [Y3 Y3] [Z3 Z3] [?? ??]
      private void decodeCollision24(byte[] data)
      {
         uint start = BE.U32(data, 0x24);
         uint end = BE.U32(data, 0x28);
         for (uint idx = start; idx < end; idx += 20)
         {
            Int16 x1, y1, z1;
            Int16 x2, y2, z2;
            Int16 x3, y3, z3;
            UInt16 t;
            x1 = BE.I16(data, idx);
            y1 = BE.I16(data, idx + 2);
            z1 = BE.I16(data, idx + 4);
            x2 = BE.I16(data, idx + 6);
            y2 = BE.I16(data, idx + 8);
            z2 = BE.I16(data, idx + 10);
            x3 = BE.I16(data, idx + 12);
            y3 = BE.I16(data, idx + 14);
            z3 = BE.I16(data, idx + 16);
            t = BE.U16(data, idx + 18);
            Collision24 zone = new Collision24(x1, y1, z1, x2, y2, z2, x3, y3, z3, t);
            collision24.Add(zone);
         }
      }

      // 0x28: Communication Point
      // [XX XX] [YY YY] [ZZ ZZ] [?? ??]
      private void decodeCommPoints(byte[] data)
      {
         uint start = BE.U32(data, 0x28);
         uint end = BE.U32(data, 0x2C);
         for (uint idx = start; idx < end; idx += 8)
         {
            Int16 x, y, z;
            UInt16 todo;
            x = BE.I16(data, idx);
            y = BE.I16(data, idx + 2);
            z = BE.I16(data, idx + 4);
            todo = BE.U16(data, idx + 6);
            CommPoint comm = new CommPoint(x, y, z, todo);
            commPoints.Add(comm);
         }
      }

      // 0x2C: Animated textures
      // {[W0 W0 W0 W0] [CC] [B5] [B6] [??] [??] [??] [??] [??] {[WC WC WC WC]}}
      private void decodeAnimatedTextures(byte[] data)
      {
         uint start = BE.U32(data, 0x2C);
         uint end = BE.U32(data, 0x30);
         uint idx = start;
         while (idx < end)
         {
            UInt32 w0;
            byte count, b5, b6, b7, b8, b9, bA, bB;
            w0 = BE.U32(data, idx);
            count = data[idx+4];
            b5 = data[idx+5];
            b6 = data[idx+6];
            b7 = data[idx+7];
            b8 = data[idx+8];
            b9 = data[idx+9];
            bA = data[idx+0xA];
            bB = data[idx+0xB];
            UInt32[] textureIdx = new UInt32[count-1];
            idx += 0xC;
            for (int i = 0; i < count - 1; i++) {
               textureIdx[i] = BE.U32(data, idx);
               idx += 4;
            }
            AnimatedTexture animated = new AnimatedTexture(w0, b5, b6, b7, b8, b9, bA, bB, textureIdx);
            animatedTextures.Add(animated);
         }
      }

      // 0x30: Terrain data
      // [EE EE EE EE] {[X1 X1] [Y1 Y1] [Z1 Z1] [X2 X2] [Y2 Y2] [Z2 Z2] [X3 X3] [Y3 Y3] [Z3 Z3] [AA] [BB]}
      private void decodeTerrain(byte[] data)
      {
         uint start = BE.U32(data, 0x30);
         uint end = BE.U32(data, 0x34);
         uint idx = start;
         uint next;
         while (idx < end)
         {
            next = start + BE.U32(data, idx);
            idx += 4;
            TerrainGroup tg = new TerrainGroup();
            while (idx < next)
            {
               Int16 x1, y1, z1, x2, y2, z2, x3, y3, z3;
               x1 = BE.I16(data, idx);
               y1 = BE.I16(data, idx + 2);
               z1 = BE.I16(data, idx + 4);
               x2 = BE.I16(data, idx + 6);
               y2 = BE.I16(data, idx + 8);
               z2 = BE.I16(data, idx + 0xA);
               x3 = BE.I16(data, idx + 0xC);
               y3 = BE.I16(data, idx + 0xE);
               z3 = BE.I16(data, idx + 0x10);
               tg.triangles.Add(new TerrainTri(x1, y1, z1, x2, y2, z2, x3, y3, z3, data[idx + 0x12], data[idx + 0x13]));
               idx += 0x14;
            }
            terrainGroups.Add(tg);
         }
      }

      // 0x34: RDUs
      // [XX XX] [YY YY] [ZZ ZZ]
      private void decodeRDUs(byte[] data)
      {
         uint start = BE.U32(data, 0x34);
         uint end = BE.U32(data, 0x38);
         for (uint idx = start; idx < end; idx += 6)
         {
            Int16 x, y, z;
            x = BE.I16(data, idx);
            y = BE.I16(data, idx + 2);
            z = BE.I16(data, idx + 4);
            RDU rdu = new RDU(x, y, z);
            rdus.Add(rdu);
         }
      }

      // 0x38: TNT crates
      // [XX XX] [YY YY] [ZZ ZZ] [TE] [TY] [H8 H8] [HA HA]
      private void decodeTNTCrates(byte[] data)
      {
         uint start = BE.U32(data, 0x38);
         uint end = BE.U32(data, 0x3C);
         for (uint idx = start; idx < end; idx += 12)
         {
            Int16 x, y, z, h8;
            UInt16 power;
            x = BE.I16(data, idx);
            y = BE.I16(data, idx + 2);
            z = BE.I16(data, idx + 4);
            h8 = BE.I16(data, idx + 8);
            power = BE.U16(data, idx + 0xA);
            TNTCrate tnt = new TNTCrate(x, y, z, data[idx + 6], data[idx + 7], h8, power);
            tntCrates.Add(tnt);
         }
      }

      // 0x3C: Square blocks and holes
      // [NN NN] {[XX XX] [YY YY] [ZZ ZZ] [T1] [T2] (CC CC) {[X1 X1] [Y1 Y1] [Z1 Z1]... [AA] [BB] [CC] [DD]}}
      private void decodeSquareBlocks(byte[] data)
      {
         uint start = BE.U32(data, 0x3C);
         uint end = BE.U32(data, 0x40);
         uint idx = start;
         while (end > idx)
         {
            UInt16 num = BE.U16(data, idx);
            // TODO: is this proper termination condition?
            if (num == 0)
            {
               break;
            }
            idx += 2;
            for (int g = 0; g < num; g++)
            {
               Int16 x, y, z;
               byte type1, type2;
               x = BE.I16(data, idx);
               y = BE.I16(data, idx + 2);
               z = BE.I16(data, idx + 4);
               type1 = data[idx + 6];
               type2 = data[idx + 7];
               SquareBlock block = new SquareBlock(x, y, z, type1, type2);
               squareBlocks.Add(block);
               idx += 8;
               if (block.type == SquareBlock.Type.Hole)
               {
                  block.isCounted = BE.U16(data, idx) > 0;
                  idx += 2;
                  for (int i = 0; i < 8; i++)
                  {
                     Int16 x2, y2, z2, x3, y3, z3;
                     x = BE.I16(data, idx);
                     y = BE.I16(data, idx + 2);
                     z = BE.I16(data, idx + 4);
                     x2 = BE.I16(data, idx + 6);
                     y2 = BE.I16(data, idx + 8);
                     z2 = BE.I16(data, idx + 0xA);
                     x3 = BE.I16(data, idx + 0xC);
                     y3 = BE.I16(data, idx + 0xE);
                     z3 = BE.I16(data, idx + 0x10);
                     block.addNode(x, y, z, x2, y2, z2, x3, y3, z3, data, (int)(idx + 0x12));
                     idx += 22;
                  }
               }
            }
         }
      }

      // 0x40: TODO some bounding boxes
      // [X1 X1] [Z1 Z1] [X2 X2] [Z2 Z2] [?? ??]
      private void decodeBounds40(byte[] data)
      {
         uint start = BE.U32(data, 0x40);
         uint end = BE.U32(data, 0x44);
         for (uint idx = start; idx < end; idx += 10)
         {
            Int16 x1, z1, x2, z2;
            UInt16 todo;
            x1 = BE.I16(data, idx + 0);
            z1 = BE.I16(data, idx + 2);
            x2 = BE.I16(data, idx + 4);
            z2 = BE.I16(data, idx + 6);
            todo = BE.U16(data, idx + 8);
            Bounds bound = new Bounds(x1, z1, x2, z2, todo);
            bounds40.Add(bound);
         }
      }

      // 0x44: TODO some bounding boxes?
      // [X1 X1] [Z1 Z1] [X2 X2] [Z2 Z2] [?? ??]
      private void decodeBounds44(byte[] data)
      {
         uint start = BE.U32(data, 0x44);
         uint end = BE.U32(data, 0x48);
         for (uint idx = start; idx < end; idx += 10)
         {
            Int16 x1, z1, x2, z2;
            UInt16 todo;
            x1 = BE.I16(data, idx + 0);
            z1 = BE.I16(data, idx + 2);
            x2 = BE.I16(data, idx + 4);
            z2 = BE.I16(data, idx + 6);
            todo = BE.U16(data, idx + 8);
            Bounds bound = new Bounds(x1, z1, x2, z2, todo);
            bounds44.Add(bound);
         }
      }

      // 0x48: TODO
      private void decode48(byte[] data)
      {
         copy48 = ArraySlice(data, BE.U32(data, 0x48), BE.U32(data, 0x4C));
      }

      // 0x4C: Level bounds
      // [X1 X1] [Z1 Z1] [X2 X2] [Z2 Z2]
      private void decodeLevelBounds(byte[] data)
      {
         uint start = BE.U32(data, 0x4C);
         bounds.x1 = BE.I16(data, start);
         bounds.z1 = BE.I16(data, start + 2);
         bounds.x2 = BE.I16(data, start + 4);
         bounds.z2 = BE.I16(data, start + 6);
      }

      // 0x50: Vehicles
      // [TT] [XX XX] [YY YY] [ZZ ZZ] [HH HH]
      private void decodeVehicles(byte[] data)
      {
         uint start = BE.U32(data, 0x50);
         uint end = BE.U32(data, 0x54);
         for (uint idx = start; idx < end; idx += 9)
         {
            Int16 x, y, z, h;
            x = BE.I16(data, idx + 1);
            y = BE.I16(data, idx + 3);
            z = BE.I16(data, idx + 5);
            h = BE.I16(data, idx + 7);
            Vehicle vehicle = new Vehicle(data[idx], x, y, z, h);
            vehicles.Add(vehicle);
         }
      }

      // 0x54: Missile carrier settings
      // [SS] [XX XX] [ZZ ZZ] [HH HH] [DD DD] [??]
      private void decodeMissileCarrier(byte[] data)
      {
         uint start = BE.U32(data, 0x54);
         carrier.speed = data[start];
         carrier.x = BE.I16(data, start + 1);
         carrier.z = BE.I16(data, start + 3);
         carrier.heading = BE.U16(data, start + 5);
         carrier.distance = BE.U16(data, start + 7);
      }

      // 0x58 TODO
      private void decode58(byte[] data)
      {
         uint start = BE.U32(data, 0x58);
         uint end = BE.U32(data, 0x5C);
         for (uint idx = start; idx < end; idx += 8)
         {
            Int16 x, y, z;
            UInt16 h6;
            x = BE.I16(data, idx);
            y = BE.I16(data, idx + 2);
            z = BE.I16(data, idx + 4);
            h6 = BE.U16(data, idx + 6);
            Object58 obj = new Object58(x, y, z, h6);
            object58s.Add(obj);
         }
      }

      // 0x5C: Buildings
      // [XX XX] [YY YY] [ZZ ZZ] [TT TT] [B8] [B9] [HA HA] [HC HC]
      private void decodeBuildings(byte[] data)
      {
         uint start = BE.U32(data, 0x5C);
         uint end = BE.U32(data, 0x60);
         for (uint idx = start; idx < end; idx += 14)
         {
            Int16 x, y, z;
            UInt16 type, hA, hC;
            x = BE.I16(data, idx);
            y = BE.I16(data, idx + 2);
            z = BE.I16(data, idx + 4);
            type = BE.U16(data, idx + 6);
            hA = BE.U16(data, idx + 0xA);
            hC = BE.U16(data, idx + 0xC);
            Building b = new Building(x, y, z, type, data[idx+8], data[idx+9], hA, hC);
            buildings.Add(b);
         }
      }

      // 0x60 TODO
      private void decode60(byte[] data)
      {
         uint start = BE.U32(data, 0x60);
         uint end = BE.U32(data, 0x64);
         uint idx = start;
         object60b0 = data[idx++];
         while (idx < end)
         {
            Int16 x, y, z, h6;
            byte b8, count;
            x = BE.I16(data, idx);
            y = BE.I16(data, idx + 2);
            z = BE.I16(data, idx + 4);
            h6 = BE.I16(data, idx + 6);
            b8 = data[idx + 8];
            count = data[idx + 9];
            Object60 obj = new Object60(x, y, z, h6, b8, count, data, idx + 0xA);
            object60s.Add(obj);
            idx += 0xCu + count;
         }
      }

      // 0x64 TODO
      private void decode64(byte[] data)
      {
         uint start = BE.U32(data, 0x64);
         uint end = BE.U32(data, 0x68);
         uint idx = start;
         while (idx < end)
         {
            byte b0 = data[idx++];
            byte headerLength = data[idx++];
            byte[] header = new byte[headerLength];
            Array.Copy(data, idx, header, 0, headerLength);
            idx += headerLength;
            byte count = data[idx++];
            WallGroup group = new WallGroup(b0, header);
            for (uint i = 0; i < count; i++)
            {
               Int16 x, y, z, x2, y2, z2, x3, y3, z3;
               UInt16 type;
               x = BE.I16(data, idx);
               y = BE.I16(data, idx + 2);
               z = BE.I16(data, idx + 4);
               x2 = BE.I16(data, idx + 6);
               y2 = BE.I16(data, idx + 8);
               z2 = BE.I16(data, idx + 0xA);
               x3 = BE.I16(data, idx + 0xC);
               y3 = BE.I16(data, idx + 0xE);
               z3 = BE.I16(data, idx + 0x10);
               type = BE.U16(data, idx + 0x12);
               Wall wall = new Wall(x, y, z, x2, y2, z2, x3, y3, z3, type);
               group.walls.Add(wall);
               idx += 20;
            }
            wallGroups.Add(group);
         }
      }

      // 0x68: Train platform and stopping zone
      // [B0] [B1]
      // [B2] {[X1 X1] [Z1 Z1] [X2 X2] [Z2 Z2] [X3 X3] [Z3 Z3]}
      // [WW WW WW WW]
      // [AA] {[H0 H0] [H2 H2] [H4 H4] [H6 H6] [H8 H8] [HA HA] [HC HC] [HE HE] [HG HG] [HI HI] [BK]}
      // [CC] {[BZ]}
      private void decodeTrainPlatform(byte[] data)
      {
         uint start = BE.U32(data, 0x68);
         uint end = BE.U32(data, 0x6C);
         uint idx = start;
         int count;
         while (idx < end)
         {
            TrainPlatform platform = new TrainPlatform();
            platform.b0 = data[idx++];
            platform.b1 = data[idx++];
            count = data[idx++];
            platform.stoppingZone = new TrainPlatform.StoppingTriangle[count];
            for (uint i = 0; i < count; i++)
            {
               TrainPlatform.StoppingTriangle triangle = new TrainPlatform.StoppingTriangle();
               triangle.x1 = BE.I16(data, idx);
               triangle.z1 = BE.I16(data, idx + 2);
               triangle.x2 = BE.I16(data, idx + 4);
               triangle.z2 = BE.I16(data, idx + 6);
               triangle.x3 = BE.I16(data, idx + 8);
               triangle.z3 = BE.I16(data, idx + 0xA);
               idx += 0xC;
               platform.stoppingZone[i] = triangle;
            }
            platform.word = BE.U32(data, idx);
            idx += 4;
            count = data[idx++];
            platform.collision = new TrainPlatform.PlatformCollision[count];
            for (uint i = 0; i < count; i++)
            {
               TrainPlatform.PlatformCollision collision = new TrainPlatform.PlatformCollision();
               collision.x1 = BE.I16(data, idx);
               collision.y1 = BE.I16(data, idx + 2);
               collision.z1 = BE.I16(data, idx + 4);
               collision.x2 = BE.I16(data, idx + 6);
               collision.y2 = BE.I16(data, idx + 8);
               collision.z2 = BE.I16(data, idx + 0xA);
               collision.x3 = BE.I16(data, idx + 0xC);
               collision.y3 = BE.I16(data, idx + 0xE);
               collision.z3 = BE.I16(data, idx + 0x10);
               collision.h12 = BE.U16(data, idx + 0x12);
               collision.b14 = data[idx + 0x14];
               idx += 0x15;
               platform.collision[i] = collision;
            }
            count = data[idx++];
            platform.someList = new byte[count];
            for (uint i = 0; i < count; i++)
            {
               platform.someList[i] = data[idx];
               idx++;
            }
            trainPlatforms.Add(platform);
         }
      }

      // 0x6C: X/Z Collision
      // {[EE EE EE EE] {[X1 X1] [Y1 Y1] [Z1 Z1] [X2 X2] [Y2 Y2] [Z2 Z2] [X3 X3] [Y3 Y3] [Z3 Z3] [AA AA] [BB] [CC]}}
      private void decodeCollision6C(byte[] data)
      {
         uint start = BE.U32(data, 0x6C);
         uint w = BE.U16(data, 0x10);
         uint h = BE.U16(data, 0x12);
         uint count = w * h;
         uint idx = start;
         uint end;
         while (count > 0)
         {
            end = start + BE.U32(data, idx);
            idx += 4;
            CollisionGroup cg = new CollisionGroup();
            while (idx < end)
            {
               Int16 x1, y1, z1, x2, y2, z2, x3, y3, z3;
               UInt16 h12;
               x1 = BE.I16(data, idx);
               y1 = BE.I16(data, idx + 2);
               z1 = BE.I16(data, idx + 4);
               x2 = BE.I16(data, idx + 6);
               y2 = BE.I16(data, idx + 8);
               z2 = BE.I16(data, idx + 0xA);
               x3 = BE.I16(data, idx + 0xC);
               y3 = BE.I16(data, idx + 0xE);
               z3 = BE.I16(data, idx + 0x10);
               h12 = BE.U16(data, idx + 0x12);
               cg.triangles.Add(new CollisionTri(x1, y1, z1, x2, y2, z2, x3, y3, z3, h12, data[idx + 0x14], data[idx + 0x15]));
               idx += 0x16;
            }
            collision6C.Add(cg);
            count--;
         }
      }

      // 0x70: Player Collision
      // {[EE EE EE EE] {[X1 X1] [Y1 Y1] [Z1 Z1] [X2 X2] [Y2 Y2] [Z2 Z2] [X3 X3] [Y3 Y3] [Z3 Z3] [AA AA] [BB] [CC]}}
      // TODO: this is a copy of 0x6C above, could be merged into common routine
      private void decodeCollision70(byte[] data)
      {
         uint start = BE.U32(data, 0x70);
         uint w = BE.U16(data, 0x10);
         uint h = BE.U16(data, 0x12);
         uint count = w * h;
         uint idx = start;
         uint end;
         while (count > 0)
         {
            end = start + BE.U32(data, idx);
            idx += 4;
            CollisionGroup cg = new CollisionGroup();
            while (idx < end)
            {
               Int16 x1, y1, z1, x2, y2, z2, x3, y3, z3;
               UInt16 h12;
               x1 = BE.I16(data, idx);
               y1 = BE.I16(data, idx + 2);
               z1 = BE.I16(data, idx + 4);
               x2 = BE.I16(data, idx + 6);
               y2 = BE.I16(data, idx + 8);
               z2 = BE.I16(data, idx + 0xA);
               x3 = BE.I16(data, idx + 0xC);
               y3 = BE.I16(data, idx + 0xE);
               z3 = BE.I16(data, idx + 0x10);
               h12 = BE.U16(data, idx + 0x12);
               cg.triangles.Add(new CollisionTri(x1, y1, z1, x2, y2, z2, x3, y3, z3, h12, data[idx + 0x14], data[idx + 0x15]));
               idx += 0x16;
            }
            collision70.Add(cg);
            count--;
         }
      }

      // 0x74 TODO
      private void decode74(byte[] data)
      {
         copy74 = ArraySlice(data, BE.U32(data, 0x74), BE.U32(data, 0x78));
      }

      private void saveVertices(byte[] data)
      {
         vertData = ArraySlice(data, 0xC8, BE.U32(data, 0x20));
      }

      private byte[] ArraySlice(byte[] data, uint start, uint end)
      {
         byte[] retArr = null;
         uint length = end - start;
         if (length > 0)
         {
            retArr = new byte[length];
            Array.Copy(data, start, retArr, 0, length);
         }
         return retArr;
      }

      private static int AppendArray(byte[] dest, int offset, byte[] src)
      {
         if (src != null)
         {
            Array.Copy(src, 0, dest, offset, src.Length);
            offset += src.Length;
            return src.Length;
         }
         return 0;
      }

      public byte[] ToBytes()
      {
         // TODO: convert to MemoryStream 
         byte[] data = new byte[400 * 1024];
         int offset = 0x0;
         int first;

         foreach (UInt16 val in header.u16s)
         {
            offset += BE.ToBytes(val, data, offset);
         }
         offset += BE.ToBytes(header.gravity, data, offset);
         offset += BE.ToBytes(header.u1C, data, offset);

         // data starts at 0xC8, fill in header offsets as we go
         offset = 0xC8;
         Array.Copy(vertData, 0, data, offset, vertData.Length);
         offset += vertData.Length;

         BE.ToBytes(offset, data, 0x20);
         foreach (AmmoBox ammo in ammoBoxes)
         {
            offset += BE.ToBytes(ammo.x, data, offset);
            offset += BE.ToBytes(ammo.y, data, offset);
            offset += BE.ToBytes(ammo.z, data, offset);
            offset += BE.ToBytes(ammo.type, data, offset);
         }

         BE.ToBytes(offset, data, 0x24);
         foreach (Collision24 collision in collision24)
         {
            offset += BE.ToBytes(collision.x1, data, offset);
            offset += BE.ToBytes(collision.y1, data, offset);
            offset += BE.ToBytes(collision.z1, data, offset);
            offset += BE.ToBytes(collision.x2, data, offset);
            offset += BE.ToBytes(collision.y2, data, offset);
            offset += BE.ToBytes(collision.z2, data, offset);
            offset += BE.ToBytes(collision.x3, data, offset);
            offset += BE.ToBytes(collision.y3, data, offset);
            offset += BE.ToBytes(collision.z3, data, offset);
            offset += BE.ToBytes(collision.type, data, offset);
         }

         BE.ToBytes(offset, data, 0x28);
         foreach (CommPoint comm in commPoints)
         {
            offset += BE.ToBytes(comm.x, data, offset);
            offset += BE.ToBytes(comm.y, data, offset);
            offset += BE.ToBytes(comm.z, data, offset);
            offset += BE.ToBytes(comm.h6, data, offset);
         }

         BE.ToBytes(offset, data, 0x2C);
         foreach (AnimatedTexture animated in animatedTextures)
         {
            offset += BE.ToBytes(animated.w0, data, offset);
            data[offset++] = (byte)(animated.textureIndexes.Length+1);
            data[offset++] = animated.b5;
            data[offset++] = animated.b6;
            data[offset++] = animated.b7;
            data[offset++] = animated.b8;
            data[offset++] = animated.b9;
            data[offset++] = animated.bA;
            data[offset++] = animated.bB;
            foreach (UInt32 word in animated.textureIndexes)
            {
               offset += BE.ToBytes(word, data, offset);
            }
         }

         BE.ToBytes(offset, data, 0x30);
         first = offset;
         foreach (TerrainGroup tg in terrainGroups)
         {
            offset += BE.ToBytes(offset - first + tg.triangles.Count * 0x14 + 4, data, offset);
            foreach (TerrainTri tri in tg.triangles)
            {
               offset += BE.ToBytes(tri.x1, data, offset);
               offset += BE.ToBytes(tri.y1, data, offset);
               offset += BE.ToBytes(tri.z1, data, offset);
               offset += BE.ToBytes(tri.x2, data, offset);
               offset += BE.ToBytes(tri.y2, data, offset);
               offset += BE.ToBytes(tri.z2, data, offset);
               offset += BE.ToBytes(tri.x3, data, offset);
               offset += BE.ToBytes(tri.y3, data, offset);
               offset += BE.ToBytes(tri.z3, data, offset);
               data[offset++] = tri.b12;
               data[offset++] = tri.b13;
            }
         }

         BE.ToBytes(offset, data, 0x34);
         foreach (RDU rdu in rdus)
         {
            offset += BE.ToBytes(rdu.x, data, offset);
            offset += BE.ToBytes(rdu.y, data, offset);
            offset += BE.ToBytes(rdu.z, data, offset);
         }

         BE.ToBytes(offset, data, 0x38);
         foreach (TNTCrate tnt in tntCrates)
         {
            offset += BE.ToBytes(tnt.x, data, offset);
            offset += BE.ToBytes(tnt.y, data, offset);
            offset += BE.ToBytes(tnt.z, data, offset);
            data[offset++] = tnt.texture;
            data[offset++] = tnt.timer;
            offset += BE.ToBytes(tnt.h8, data, offset);
            offset += BE.ToBytes(tnt.power, data, offset);
         }

         BE.ToBytes(offset, data, 0x3C);
         if (squareBlocks.Count > 1)
         {
            // separate holes from blocks
            List<SquareBlock> blocks = new List<SquareBlock>();
            List<SquareBlock> holes = new List<SquareBlock>();
            foreach (SquareBlock block in squareBlocks)
            {
               if (block.type == SquareBlock.Type.Hole)
               {
                  holes.Add(block);
               }
               else
               {
                  blocks.Add(block);
               }
            }

            offset += BE.ToBytes((UInt16)blocks.Count, data, offset);
            foreach (SquareBlock block in blocks)
            {
               offset += BE.ToBytes(block.x, data, offset);
               offset += BE.ToBytes(block.y, data, offset);
               offset += BE.ToBytes(block.z, data, offset);
               data[offset++] = block.Type1();
               data[offset++] = block.Type2();
            }

            offset += BE.ToBytes((UInt16)holes.Count, data, offset);
            foreach (SquareBlock block in holes)
            {
               offset += BE.ToBytes(block.x, data, offset);
               offset += BE.ToBytes(block.y, data, offset);
               offset += BE.ToBytes(block.z, data, offset);
               data[offset++] = block.Type1();
               data[offset++] = block.Type2();
               block.computeNodes();
               offset += BE.ToBytes((UInt16)(block.isCounted ? 1 : 0), data, offset);
               foreach (SquareBlock.Node node in block.nodes)
               {
                  for (int i = 0; i < 3; i++)
                  {
                     offset += BE.ToBytes(node.x[i], data, offset);
                     offset += BE.ToBytes(node.y[i], data, offset);
                     offset += BE.ToBytes(node.z[i], data, offset);
                  }
                  Array.Copy(node.other, 0, data, offset, node.other.Length);
                  offset += node.other.Length;
               }
            }
         }

         BE.ToBytes(offset, data, 0x40);
         foreach (Bounds bound in bounds40)
         {
            offset += BE.ToBytes(bound.x1, data, offset);
            offset += BE.ToBytes(bound.z1, data, offset);
            offset += BE.ToBytes(bound.x2, data, offset);
            offset += BE.ToBytes(bound.z2, data, offset);
            offset += BE.ToBytes(bound.todo, data, offset);
         }

         BE.ToBytes(offset, data, 0x44);
         foreach (Bounds bound in bounds44)
         {
            offset += BE.ToBytes(bound.x1, data, offset);
            offset += BE.ToBytes(bound.z1, data, offset);
            offset += BE.ToBytes(bound.x2, data, offset);
            offset += BE.ToBytes(bound.z2, data, offset);
            offset += BE.ToBytes(bound.todo, data, offset);
         }

         // TODO: 0x48 real data
         BE.ToBytes(offset, data, 0x48);
         offset += AppendArray(data, offset, copy48);

         BE.ToBytes(offset, data, 0x4C);
         offset += BE.ToBytes(bounds.x1, data, offset);
         offset += BE.ToBytes(bounds.z1, data, offset);
         offset += BE.ToBytes(bounds.x2, data, offset);
         offset += BE.ToBytes(bounds.z2, data, offset);

         BE.ToBytes(offset, data, 0x50);
         foreach (Vehicle veh in vehicles)
         {
            data[offset++] = veh.type;
            offset += BE.ToBytes(veh.x, data, offset);
            offset += BE.ToBytes(veh.y, data, offset);
            offset += BE.ToBytes(veh.z, data, offset);
            offset += BE.ToBytes(veh.heading, data, offset);
         }

         BE.ToBytes(offset, data, 0x54);
         data[offset++] = carrier.speed;
         offset += BE.ToBytes(carrier.x, data, offset);
         offset += BE.ToBytes(carrier.z, data, offset);
         offset += BE.ToBytes(carrier.heading, data, offset);
         offset += BE.ToBytes(carrier.distance, data, offset);

         // 0x58 starts at half-word boundary, so pad as necessary
         if ((offset & 0x1) > 0)
         {
            data[offset++] = 0x0;
         }

         BE.ToBytes(offset, data, 0x58);
         foreach (Object58 obj in object58s)
         {
            offset += BE.ToBytes(obj.x, data, offset);
            offset += BE.ToBytes(obj.y, data, offset);
            offset += BE.ToBytes(obj.z, data, offset);
            offset += BE.ToBytes(obj.h6, data, offset);
         }

         BE.ToBytes(offset, data, 0x5C);
         foreach (Building b in buildings)
         {
            offset += BE.ToBytes(b.x, data, offset);
            offset += BE.ToBytes(b.y, data, offset);
            offset += BE.ToBytes(b.z, data, offset);
            offset += BE.ToBytes(b.type, data, offset);
            data[offset++] = (byte)(b.isCounted ? 1 : 0);
            data[offset++] = b.b9;
            offset += BE.ToBytes(b.movement, data, offset);
            offset += BE.ToBytes(b.speed, data, offset);
         }

         BE.ToBytes(offset, data, 0x60);
         data[offset++] = object60b0;
         foreach (Object60 obj in object60s)
         {
            offset += BE.ToBytes(obj.x, data, offset);
            offset += BE.ToBytes(obj.y, data, offset);
            offset += BE.ToBytes(obj.z, data, offset);
            offset += BE.ToBytes(obj.h6, data, offset);
            data[offset++] = obj.b8;
            data[offset++] = (byte)obj.values.Length;
            foreach (byte val in obj.values)
            {
               data[offset++] = val;
            }
            foreach (byte val in obj.lastTwo)
            {
               data[offset++] = val;
            }
         }

         BE.ToBytes(offset, data, 0x64);
         foreach (WallGroup group in wallGroups)
         {
            data[offset++] = group.b0;
            data[offset++] = (byte)group.header.Length;
            Array.Copy(group.header, 0, data, offset, group.header.Length);
            offset += group.header.Length;
            data[offset++] = (byte)group.walls.Count;
            foreach (Wall wall in group.walls)
            {
               offset += BE.ToBytes(wall.x1, data, offset);
               offset += BE.ToBytes(wall.y1, data, offset);
               offset += BE.ToBytes(wall.z1, data, offset);
               offset += BE.ToBytes(wall.x2, data, offset);
               offset += BE.ToBytes(wall.y2, data, offset);
               offset += BE.ToBytes(wall.z2, data, offset);
               offset += BE.ToBytes(wall.x3, data, offset);
               offset += BE.ToBytes(wall.y3, data, offset);
               offset += BE.ToBytes(wall.z3, data, offset);
               offset += BE.ToBytes(wall.type, data, offset);
            }
         }

         BE.ToBytes(offset, data, 0x68);
         foreach (TrainPlatform platform in trainPlatforms)
         {
            data[offset++] = platform.b0;
            data[offset++] = platform.b1;
            data[offset++] = (byte)platform.stoppingZone.Length;
            foreach (TrainPlatform.StoppingTriangle triangle in platform.stoppingZone)
            {
               offset += BE.ToBytes(triangle.x1, data, offset);
               offset += BE.ToBytes(triangle.z1, data, offset);
               offset += BE.ToBytes(triangle.x2, data, offset);
               offset += BE.ToBytes(triangle.z2, data, offset);
               offset += BE.ToBytes(triangle.x3, data, offset);
               offset += BE.ToBytes(triangle.z3, data, offset);
            }
            offset += BE.ToBytes(platform.word, data, offset);
            data[offset++] = (byte)platform.collision.Length;
            foreach (TrainPlatform.PlatformCollision collision in platform.collision)
            {
               offset += BE.ToBytes(collision.x1, data, offset);
               offset += BE.ToBytes(collision.y1, data, offset);
               offset += BE.ToBytes(collision.z1, data, offset);
               offset += BE.ToBytes(collision.x2, data, offset);
               offset += BE.ToBytes(collision.y2, data, offset);
               offset += BE.ToBytes(collision.z2, data, offset);
               offset += BE.ToBytes(collision.x3, data, offset);
               offset += BE.ToBytes(collision.y3, data, offset);
               offset += BE.ToBytes(collision.z3, data, offset);
               offset += BE.ToBytes(collision.h12, data, offset);
               data[offset++] = collision.b14;
            }
            data[offset++] = (byte)platform.someList.Length;
            foreach (byte b in platform.someList)
            {
               data[offset++] = b;
            }
         }

         BE.ToBytes(offset, data, 0x6C);
         first = offset;
         foreach (CollisionGroup cg in collision6C)
         {
            offset += BE.ToBytes(offset - first + cg.triangles.Count * 0x16 + 4, data, offset);
            foreach (CollisionTri tri in cg.triangles)
            {
               offset += BE.ToBytes(tri.x1, data, offset);
               offset += BE.ToBytes(tri.y1, data, offset);
               offset += BE.ToBytes(tri.z1, data, offset);
               offset += BE.ToBytes(tri.x2, data, offset);
               offset += BE.ToBytes(tri.y2, data, offset);
               offset += BE.ToBytes(tri.z2, data, offset);
               offset += BE.ToBytes(tri.x3, data, offset);
               offset += BE.ToBytes(tri.y3, data, offset);
               offset += BE.ToBytes(tri.z3, data, offset);
               offset += BE.ToBytes(tri.h12, data, offset);
               data[offset++] = tri.b14;
               data[offset++] = tri.b15;
            }
         }

         // TODO: this is a copy of 0x6C above, could be merged into common routine
         BE.ToBytes(offset, data, 0x70);
         first = offset;
         foreach (CollisionGroup cg in collision70)
         {
            offset += BE.ToBytes(offset - first + cg.triangles.Count * 0x16 + 4, data, offset);
            foreach (CollisionTri tri in cg.triangles)
            {
               offset += BE.ToBytes(tri.x1, data, offset);
               offset += BE.ToBytes(tri.y1, data, offset);
               offset += BE.ToBytes(tri.z1, data, offset);
               offset += BE.ToBytes(tri.x2, data, offset);
               offset += BE.ToBytes(tri.y2, data, offset);
               offset += BE.ToBytes(tri.z2, data, offset);
               offset += BE.ToBytes(tri.x3, data, offset);
               offset += BE.ToBytes(tri.y3, data, offset);
               offset += BE.ToBytes(tri.z3, data, offset);
               offset += BE.ToBytes(tri.h12, data, offset);
               data[offset++] = tri.b14;
               data[offset++] = tri.b15;
            }
         }

         // 0x74 starts at word boundary, so pad as necessary
         while ((offset & 0x3) > 0)
         {
            data[offset++] = 0x0;
         }

         // TODO: 0x74 real data
         BE.ToBytes(offset, data, 0x74);
         offset += AppendArray(data, offset, copy74);

         int startDL = offset;

         // update display list header offsets
         offset = 0x78;
         UInt32 minCheckOffset = (UInt32)header.dlOffset;
         for (int i = 0; i < header.offsets.Length; i++)
         {
            if (i * 4 + 0x20 >= 0x78)
            {
               // keep track of the min offset that is not zero
               if (header.offsets[i] != 0 && header.offsets[i] < minCheckOffset)
               {
                  minCheckOffset = header.offsets[i];
               }
               UInt32 dlOffset = (UInt32)(header.offsets[i] + startDL - header.dlOffset);
               offset += BE.ToBytes(dlOffset, data, offset);
            }
         }

         // update lists of pointers into the display list data at the end of the level data
         // TODO: figure out the structure of this data
         for (UInt32 off = (UInt32)(minCheckOffset - header.dlOffset + startDL); off < startDL; off += 4)
         {
            UInt32 val = BE.U32(data, off);
            if (val >= header.dlOffset)
            {
               UInt32 newVal = (UInt32)(val + startDL - header.dlOffset);
               BE.ToBytes(newVal, data, (int)off);
            }
         }

         byte[] copy = new byte[startDL];
         Array.Copy(data, copy, startDL);

         return copy;
      }

      public void Write(StreamWriter writer, BlastCorpsLevelMeta levelMeta)
      {
         int offset;
         writer.WriteLine("Level {0}: {1} [{2}]", levelMeta.id, levelMeta.name, levelMeta.filename);
         writer.WriteLine();
         writer.WriteLine("Header:");
         offset = 0;
         foreach (UInt16 val in header.u16s)
         {
            writer.WriteLine("{0:X2}: {1:X}", offset, val);
            offset += 2;
         }
         writer.WriteLine("{0:X2}: {1}", offset, header.gravity);
         offset += 4;
         writer.WriteLine("{0:X2}: {1}", offset, header.u1C);
         offset += 4;
         foreach (UInt32 val in header.offsets)
         {
            writer.WriteLine("{0:X2}: {1:X}", offset, val);
            offset += 4;
         }
         writer.WriteLine();


         writer.WriteLine("Carrier: {0}", carrier);
         writer.WriteLine();

         writer.WriteLine("Ammo [{0}]:", ammoBoxes.Count);
         foreach (AmmoBox ammo in ammoBoxes)
         {
            writer.WriteLine(ammo);
         }
         writer.WriteLine();

         writer.WriteLine("Communication Points [{0}]:", commPoints.Count);
         foreach (CommPoint comm in commPoints)
         {
            writer.WriteLine(comm);
         }
         writer.WriteLine();

         writer.WriteLine("Animated Textures [{0}]:", animatedTextures.Count);
         foreach (AnimatedTexture anim in animatedTextures)
         {
            writer.WriteLine(anim);
         }
         writer.WriteLine();

         writer.WriteLine("RDUs [{0}]:", rdus.Count);
         foreach (RDU rdu in rdus)
         {
            writer.WriteLine(rdu);
         }
         writer.WriteLine();

         writer.WriteLine("TNT Crates [{0}]:", tntCrates.Count);
         foreach (TNTCrate tnt in tntCrates)
         {
            writer.WriteLine(tnt);
         }
         writer.WriteLine();

         writer.WriteLine("Square Blocks [{0}]:", squareBlocks.Count);
         foreach (SquareBlock block in squareBlocks)
         {
            writer.WriteLine(block.ToStringFull());
         }
         writer.WriteLine();

         writer.WriteLine("Vehicles [{0}]:", vehicles.Count);
         foreach (Vehicle vehicle in vehicles)
         {
            writer.WriteLine(vehicle);
         }
         writer.WriteLine();

         writer.WriteLine("Buildings [{0}]:", buildings.Count);
         foreach (Building building in buildings)
         {
            writer.WriteLine(building);
         }
         writer.WriteLine();
      }

      static public BlastCorpsLevel decodeLevel(byte[] levelData, byte[] displayListData)
      {
         BlastCorpsLevel level = new BlastCorpsLevel();
         level.decodeHeader(levelData);          // 0x00-0x1F
         level.saveVertices(levelData);          // 0xC8-ammo
         level.decodeAmmoBoxes(levelData);       // 0x20
         level.decodeCollision24(levelData);     // 0x24
         level.decodeCommPoints(levelData);      // 0x28
         level.decodeAnimatedTextures(levelData);// 0x2C
         level.decodeTerrain(levelData);         // 0x30
         level.decodeRDUs(levelData);            // 0x34
         level.decodeTNTCrates(levelData);       // 0x38
         level.decodeSquareBlocks(levelData);    // 0x3C
         level.decodeBounds40(levelData);        // 0x40
         level.decodeBounds44(levelData);        // 0x44
         level.decode48(levelData);              // 0x48 TODO: decode these U32s
         level.decodeLevelBounds(levelData);     // 0x4C
         level.decodeVehicles(levelData);        // 0x50
         level.decodeMissileCarrier(levelData);  // 0x54
         level.decode58(levelData);              // 0x58 TODO
         level.decodeBuildings(levelData);       // 0x5C
         level.decode60(levelData);              // 0x60 TODO
         level.decode64(levelData);              // 0x64 TODO
         level.decodeTrainPlatform(levelData);   // 0x68
         level.decodeCollision6C(levelData);     // 0x6C
         level.decodeCollision70(levelData);     // 0x70
         level.decode74(levelData);              // 0x74 TODO
         // 0x78-0x9C are located in the display list data
         level.copyLevelData = levelData;
         level.displayList = displayListData;
         return level;
      }
   }
}
