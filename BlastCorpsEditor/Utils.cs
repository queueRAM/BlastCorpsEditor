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
}
