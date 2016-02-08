using System;
using System.Collections.Generic;

namespace BlastCorpsEditor
{
   // helper class for converting arrays of bytes in big endian to native integer types
   class BE
   {
      public static UInt32 U32(byte[] buf, uint i)
      {
         byte[] u32bytes = new byte[4];
         Array.Copy(buf, i, u32bytes, 0, u32bytes.Length);
         if (BitConverter.IsLittleEndian)
         {
            Array.Reverse(u32bytes);
         }
         return BitConverter.ToUInt32(u32bytes, 0);
      }

      public static Int32 I32(byte[] buf, uint i)
      {
         byte[] i32bytes = new byte[4];
         Array.Copy(buf, i, i32bytes, 0, i32bytes.Length);
         if (BitConverter.IsLittleEndian)
         {
            Array.Reverse(i32bytes);
         }
         return BitConverter.ToInt32(i32bytes, 0);
      }

      public static UInt16 U16(byte[] buf, uint i)
      {
         byte[] u16bytes = new byte[2];
         Array.Copy(buf, i, u16bytes, 0, u16bytes.Length);
         if (BitConverter.IsLittleEndian)
         {
            Array.Reverse(u16bytes);
         }
         return BitConverter.ToUInt16(u16bytes, 0);
      }

      public static Int16 I16(byte[] buf, uint i)
      {
         byte[] i16bytes = new byte[2];
         Array.Copy(buf, i, i16bytes, 0, i16bytes.Length);
         if (BitConverter.IsLittleEndian)
         {
            Array.Reverse(i16bytes);
         }
         return BitConverter.ToInt16(i16bytes, 0);
      }

      public static int ToBytes(Int16 val, byte[] buf, int i)
      {
         byte[] data = BitConverter.GetBytes(val);
         if (BitConverter.IsLittleEndian)
         {
            Array.Reverse(data);
         }
         Array.Copy(data, 0, buf, i, data.Length);
         return data.Length;
      }

      public static int ToBytes(UInt16 val, byte[] buf, int i)
      {
         byte[] data = BitConverter.GetBytes(val);
         if (BitConverter.IsLittleEndian)
         {
            Array.Reverse(data);
         }
         Array.Copy(data, 0, buf, i, data.Length);
         return data.Length;
      }

      public static int ToBytes(Int32 val, byte[] buf, int i)
      {
         byte[] data = BitConverter.GetBytes(val);
         if (BitConverter.IsLittleEndian)
         {
            Array.Reverse(data);
         }
         Array.Copy(data, 0, buf, i, data.Length);
         return data.Length;
      }

      public static int ToBytes(UInt32 val, byte[] buf, int i)
      {
         byte[] data = BitConverter.GetBytes(val);
         if (BitConverter.IsLittleEndian)
         {
            Array.Reverse(data);
         }
         Array.Copy(data, 0, buf, i, data.Length);
         return data.Length;
      }
   }

   public class BlastCorpsLevelMeta
   {
      public uint id { get; set; }
      public string name { get; set; }
      public string filename { get; set; }
      public uint start { get; set; }
      public uint end { get; set; }

      public override string ToString()
      {
         return id + ": " + name + " [" + filename + "]";
      }
   }

   public class LevelHeader
   {
      public UInt16[] u16s = new UInt16[12];
      public Int32[] twoVals = new Int32[2];
      public UInt32[] offsets = new UInt32[42];
   }

   public class AmmoBox
   {
      public Int16 x, y, z;
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
         return x + ", " + y + ", " + z + ", " + type;
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

   public class CommPoint
   {
      public Int16 x, y, z;
      public UInt16 todo;

      public CommPoint(Int16 x, Int16 y, Int16 z, UInt16 todo)
      {
         this.x = x;
         this.y = y;
         this.z = z;
         this.todo = todo;
      }

      public override string ToString()
      {
         return x + ", " + y + ", " + z + ", " + todo;
      }
   }

   public class LevelBounds
   {
      public Int16 x1, z1, x2, z2;

      public override string ToString()
      {
         return x1 + ", " + z1 + " " + x2 + ", " + z2;
      }
   }

   public class RDU
   {
      public Int16 x, y, z;
      public bool selected;

      public RDU(Int16 x, Int16 y, Int16 z)
      {
         this.x = x;
         this.y = y;
         this.z = z;
         this.selected = false;
      }

      public override string ToString()
      {
         return x + ", " + y + ", " + z;
      }
   }

   public class TNTCrate
   {
      public Int16 x, y, z;
      public byte b6; // TODO
      public byte timer;
      public Int16 h8, hA; // TODO

      public TNTCrate(Int16 x, Int16 y, Int16 z, byte b6, byte timer, Int16 h8, Int16 hA)
      {
         this.x = x;
         this.y = y;
         this.z = z;
         this.b6 = b6;
         this.timer = timer;
         this.h8 = h8;
         this.hA = hA;
      }

      public override string ToString()
      {
         return x + ", " + y + ", " + z + ", " + b6.ToString("X2") + ", " + timer.ToString("X2") + ", " + h8.ToString("X4") + ", " + hA.ToString("X4");
      }
   }

   public class SquareBlock
   {
      public Int16 x, y, z;
      public byte type;
      public byte hole;
      public UInt16 extra;
      public class Node
      {
         public Int16[] x;
         public Int16[] y;
         public Int16[] z;
         public byte[] other = new byte[4];
      }
      public List<Node> nodes = new List<Node>();

      public SquareBlock(Int16 x, Int16 y, Int16 z, byte type, byte hole)
      {
         this.x = x;
         this.y = y;
         this.z = z;
         this.type = type;
         this.hole = hole;
      }

      public void addNode(Int16 x1, Int16 y1, Int16 z1, Int16 x2, Int16 y2, Int16 z2, Int16 x3, Int16 y3, Int16 z3, byte[] data, int index)
      {
         Node n = new Node();
         n.x = new Int16[] { x1, x2, x3 };
         n.y = new Int16[] { y1, y2, y3 };
         n.z = new Int16[] { z1, z2, z3 };
         Array.Copy(data, index, n.other, 0, 4);
         nodes.Add(n);
      }

      public override string ToString()
      {
         return x + ", " + y + ", " + z + ", " + type + ", " + hole + ((hole == 8) ? (", " + extra) : "");
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
         return x1 + ", " + z1 + ", " + x2 + ", " + z2 + ", " + todo;
      }
   }

   public class Vehicle
   {
      public byte type;
      public Int16 x, y, z, heading;

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
         return type.ToString("X2") + ": " + x + ", " + y + ", " + z + ", " + heading;
      }
   }

   public class Carrier
   {
      public byte speed;
      public Int16 x, y, z;
      public UInt16 heading, distance;

      public override string ToString()
      {
         return speed + ", " + x + ", " + y + ", " + z + ", " + heading;
      }
   }

   public class Building
   {
      public Int16 x, y, z;
      public UInt16 type;
      // TODO: what are these?
      public byte b8, b9;
      public UInt16 hA, hC;

      public Building(Int16 x, Int16 y, Int16 z, UInt16 type, byte b8, byte b9, UInt16 hA, UInt16 hC)
      {
         this.x = x;
         this.y = y;
         this.z = z;
         this.type = type;
         this.b8 = b8;
         this.b9 = b9;
         this.hA = hA;
         this.hC = hC;
      }

      public override string ToString()
      {
         return x + ", " + y + ", " + z + ", " + type + ", " + b8 + ", " + b9 + ", " + hA + ", " + hC;
      }
   }

   public class BlastCorpsLevel
   {
      public LevelHeader header = new LevelHeader();
      public List<AmmoBox> ammoBoxes = new List<AmmoBox>();
      public List<Collision24> collisions = new List<Collision24>();
      public List<CommPoint> commPoints = new List<CommPoint>();
      public List<RDU> rdus = new List<RDU>();
      public List<TNTCrate> tntCrates = new List<TNTCrate>();
      public List<SquareBlock> squareBlocks = new List<SquareBlock>();
      public List<Bounds> bounds40 = new List<Bounds>();
      public List<Bounds> bounds44 = new List<Bounds>();
      public LevelBounds bounds = new LevelBounds();
      public List<Vehicle> vehicles = new List<Vehicle>();
      public Carrier carrier = new Carrier();
      public List<Building> buildings = new List<Building>();
      private byte[] copy2C;
      private byte[] copy30;
      private byte[] copy48;
      private byte[] copy58;
      private byte[] copy60;
      private byte[] copy64;
      private byte[] copy68;
      private byte[] copy6C;
      private byte[] copy70;
      private byte[] copy74;
      private byte[] vertData;

      // 0x00-0x20: Header
      private void decodeHeader(byte[] data)
      {
         // read in 12 u16s
         for (uint i = 0; i < header.u16s.Length; i++)
         {
            header.u16s[i] = BE.U16(data, i*2);
         }
         // read in two signed 32 values
         for (uint i = 0; i < header.twoVals.Length; i++)
         {
            header.twoVals[i] = BE.I32(data, 0x18 + i*4);
         }
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
            collisions.Add(zone);
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

      // 0x2C: TODO
      // {[W0 W0 W0 W0] [CC] [B5] [B6] [??] [??] [??] [??] [??] {[WC WC WC WC]}}
      private void decode2C(byte[] data)
      {
         copy2C = ArraySlice(data, BE.U32(data, 0x2C), BE.U32(data, 0x30));
      }

      // 0x30: TODO
      private void decode30(byte[] data)
      {
         copy30 = ArraySlice(data, BE.U32(data, 0x30), BE.U32(data, 0x34));
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
      // [XX XX] [YY YY] [ZZ ZZ] [B6] [TT] [H8 H8] [HA HA]
      private void decodeTNTCrates(byte[] data)
      {
         uint start = BE.U32(data, 0x38);
         uint end = BE.U32(data, 0x3C);
         for (uint idx = start; idx < end; idx += 12)
         {
            Int16 x, y, z, h8, hA;
            x = BE.I16(data, idx);
            y = BE.I16(data, idx + 2);
            z = BE.I16(data, idx + 4);
            h8 = BE.I16(data, idx + 8);
            hA = BE.I16(data, idx + 0xA);
            TNTCrate tnt = new TNTCrate(x, y, z, data[idx + 6], data[idx + 7], h8, hA);
            tntCrates.Add(tnt);
         }
      }

      // 0x3C: Square blocks and holes
      // [NN NN] {[XX XX] [YY YY] [ZZ ZZ] [T1] [T2] (HH HH) {[X1 X1] [Y1 Y1] [Z1 Z1]... [AA] [BB] [CC] [DD]}}
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
               byte type, hole;
               x = BE.I16(data, idx);
               y = BE.I16(data, idx + 2);
               z = BE.I16(data, idx + 4);
               type = data[idx + 6];
               hole = data[idx + 7];
               SquareBlock block = new SquareBlock(x, y, z, type, hole);
               squareBlocks.Add(block);
               idx += 8;
               if (hole == 8)
               {
                  block.extra = BE.U16(data, idx);
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
         copy58 = ArraySlice(data, BE.U32(data, 0x58), BE.U32(data, 0x5C));
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
         copy60 = ArraySlice(data, BE.U32(data, 0x60), BE.U32(data, 0x64));
      }

      // 0x64 TODO
      private void decode64(byte[] data)
      {
         copy64 = ArraySlice(data, BE.U32(data, 0x64), BE.U32(data, 0x68));
      }

      // 0x68: Train platform and stopping zone
      // [B0] [B1]
      // [B2] {[X1 X1] [Z1 Z1] [X2 X2] [Z2 Z2] [X3 X3] [Z3 Z3]}
      // [WW WW WW WW]
      // [AA] {[H0 H0] [H2 H2] [H4 H4] [H6 H6] [H8 H8] [HA HA] [HC HC] [HE HE] [HG HG] [HI HI] [BK]}
      // [CC] {[BZ]}
      private void decode68(byte[] data)
      {
         copy68 = ArraySlice(data, BE.U32(data, 0x68), BE.U32(data, 0x6C));
      }

      // 0x6C TODO
      private void decode6C(byte[] data)
      {
         copy6C = ArraySlice(data, BE.U32(data, 0x6C), BE.U32(data, 0x70));
      }

      // 0x70 TODO
      private void decode70(byte[] data)
      {
         copy70 = ArraySlice(data, BE.U32(data, 0x70), BE.U32(data, 0x74));
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
         // TODO: better estimate of size
         byte[] data = new byte[400 * 1024];
         int offset = 0x0;

         foreach (UInt16 val in header.u16s)
         {
            offset += BE.ToBytes(val, data, offset);
         }
         foreach (Int32 val in header.twoVals)
         {
            offset += BE.ToBytes(val, data, offset);
         }

         // TODO: real offsets
         offset = 0x78;
         for (int i = 0; i < header.offsets.Length; i++)
         {
            if (i * 4 + 0x20 >= 0x78)
            {
               offset += BE.ToBytes(header.offsets[i], data, offset);
            }
         }

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
         foreach (Collision24 collision in collisions)
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
            offset += BE.ToBytes(comm.todo, data, offset);
         }

         // TODO: 0x2C real data
         BE.ToBytes(offset, data, 0x2C);
         offset += AppendArray(data, offset, copy2C);

         // TODO: 0x30 real data
         BE.ToBytes(offset, data, 0x30);
         offset += AppendArray(data, offset, copy30);

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
            data[offset++] = tnt.b6;
            data[offset++] = tnt.timer;
            offset += BE.ToBytes(tnt.h8, data, offset);
            offset += BE.ToBytes(tnt.hA, data, offset);
         }

         BE.ToBytes(offset, data, 0x3C);
         if (squareBlocks.Count > 1)
         {
            bool first8 = false;
            // TODO: assuming blocks match holes and are in order
            // it might be better to store them in two separate lists
            offset += BE.ToBytes((UInt16)(squareBlocks.Count / 2), data, offset);
            foreach (SquareBlock block in squareBlocks)
            {
               if (block.hole == 8  && !first8)
               {
                  offset += BE.ToBytes((UInt16)(squareBlocks.Count / 2), data, offset);
                  first8 = true;
               }
               offset += BE.ToBytes(block.x, data, offset);
               offset += BE.ToBytes(block.y, data, offset);
               offset += BE.ToBytes(block.z, data, offset);
               data[offset++] = block.type;
               data[offset++] = block.hole;
               if (block.hole == 8)
               {
                  offset += BE.ToBytes(block.extra, data, offset);
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

         // TODO: 0x58 real data
         BE.ToBytes(offset, data, 0x58);
         offset += AppendArray(data, offset, copy58);

         BE.ToBytes(offset, data, 0x5C);
         foreach (Building b in buildings)
         {
            offset += BE.ToBytes(b.x, data, offset);
            offset += BE.ToBytes(b.y, data, offset);
            offset += BE.ToBytes(b.z, data, offset);
            offset += BE.ToBytes(b.type, data, offset);
            data[offset++] = b.b8;
            data[offset++] = b.b9;
            offset += BE.ToBytes(b.hA, data, offset);
            offset += BE.ToBytes(b.hC, data, offset);
         }

         // TODO: 0x60 real data
         BE.ToBytes(offset, data, 0x60);
         offset += AppendArray(data, offset, copy60);

         // TODO: 0x64 real data
         BE.ToBytes(offset, data, 0x64);
         offset += AppendArray(data, offset, copy64);

         // TODO: 0x68 real data
         BE.ToBytes(offset, data, 0x68);
         offset += AppendArray(data, offset, copy68);

         // TODO: 0x6C real data
         BE.ToBytes(offset, data, 0x6C);
         offset += AppendArray(data, offset, copy6C);

         // TODO: 0x70 real data
         BE.ToBytes(offset, data, 0x70);
         offset += AppendArray(data, offset, copy70);

         // TODO: 0x74 real data
         BE.ToBytes(offset, data, 0x74);
         offset += AppendArray(data, offset, copy74);

         byte[] copy = new byte[offset];
         Array.Copy(data, copy, offset);

         return copy;
      }


      static public BlastCorpsLevel decodeLevel(byte[] levelData)
      {
         BlastCorpsLevel level = new BlastCorpsLevel();
         level.decodeHeader(levelData);          // 0x00-0x1F
         level.saveVertices(levelData);          // 0xC8-ammo
         level.decodeAmmoBoxes(levelData);       // 0x20
         level.decodeCollision24(levelData);     // 0x24
         level.decodeCommPoints(levelData);      // 0x28
         level.decode2C(levelData);              // 0x2C TODO
         level.decode30(levelData);              // 0x30 TODO
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
         level.decode68(levelData);              // 0x68 TODO
         level.decode6C(levelData);              // 0x6C TODO
         level.decode70(levelData);              // 0x70 TODO
         level.decode74(levelData);              // 0x74 TODO
         // TODO: 0x78-0x9C are beyond level length
         return level;
      }
   }
}
