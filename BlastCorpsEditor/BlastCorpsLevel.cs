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
         return id + ": " + name + " / " + filename;
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

   public class Zone
   {
      public Int16 x1, y1, z1;
      public Int16 x2, y2, z2;
      public Int16 x3, y3, z3;
      public UInt16 type;

      public Zone(Int16 x1, Int16 y1, Int16 z1, Int16 x2, Int16 y2, Int16 z2, Int16 x3, Int16 y3, Int16 z3, UInt16 t)
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
      public UInt16 type;

      public CommPoint(Int16 x, Int16 y, Int16 z)
      {
         this.x = x;
         this.y = y;
         this.z = z;
      }

      public override string ToString()
      {
         return x + ", " + y + ", " + z;
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
         public byte[] other = new byte[4]; // TODO
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

      public void addNode(Int16 x1, Int16 y1, Int16 z1, Int16 x2, Int16 y2, Int16 z2, Int16 x3, Int16 y3, Int16 z3)
      {
         Node n = new Node();
         n.x = new Int16[] { x1, x2, x3 };
         n.y = new Int16[] { y1, y2, y3 };
         n.z = new Int16[] { z1, z2, z3 };
         nodes.Add(n);
      }

      public override string ToString()
      {
         return x + ", " + y + ", " + z + ", " + type + ", " + hole + ((hole == 8) ? (", " + extra) : "");
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

   public class BlastCorpsLevel
   {
      public LevelHeader header = new LevelHeader();
      public LevelBounds bounds = new LevelBounds();
      public List<AmmoBox> ammoBoxes = new List<AmmoBox>();
      public List<Zone> zones = new List<Zone>();
      public List<CommPoint> commPoints = new List<CommPoint>();
      public List<RDU> rdus = new List<RDU>();
      public List<TNTCrate> tntCrates = new List<TNTCrate>();
      public List<SquareBlock> squareBlocks = new List<SquareBlock>();
      public List<Vehicle> vehicles = new List<Vehicle>();

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
            Zone zone = new Zone(x1, y1, z1, x2, y2, z2, x3, y3, z3, t);
            zones.Add(zone);
         }
      }

      private void decodeCommPoints(byte[] data)
      {
         uint start = BE.U32(data, 0x28);
         uint end = BE.U32(data, 0x2C);
         for (uint idx = start; idx < end; idx += 8)
         {
            Int16 x, y, z;
            x = BE.I16(data, idx);
            y = BE.I16(data, idx + 2);
            z = BE.I16(data, idx + 4);
            CommPoint comm = new CommPoint(x, y, z);
            commPoints.Add(comm);
         }
      }

      private void decodeLevelBounds(byte[] data)
      {
         uint start = BE.U32(data, 0x4C);
         bounds.x1 = BE.I16(data, start);
         bounds.z1 = BE.I16(data, start+2);
         bounds.x2 = BE.I16(data, start+4);
         bounds.z2 = BE.I16(data, start+6);
      }

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
                     // TODO skipping 4 bytes
                     block.addNode(x, y, z, x2, y2, z2, x3, y3, z3);
                     idx += 22;
                  }
               }
            }
         }
      }

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

      static public BlastCorpsLevel decodeLevel(byte[] levelData)
      {
         BlastCorpsLevel level = new BlastCorpsLevel();
         level.decodeHeader(levelData);
         level.decodeAmmoBoxes(levelData);
         level.decodeCollision24(levelData);
         level.decodeCommPoints(levelData);
         level.decodeLevelBounds(levelData);
         level.decodeRDUs(levelData);
         level.decodeTNTCrates(levelData);
         level.decodeSquareBlocks(levelData);
         level.decodeVehicles(levelData);
         return level;
      }
   }
}
