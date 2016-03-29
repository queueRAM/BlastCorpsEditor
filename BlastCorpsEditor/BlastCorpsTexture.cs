using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlastCorpsEditor
{
   class BlastCorpsTexture
   {
      public const UInt32 ROM_OFFSET = 0x4CE0;
      public UInt32 offset;
      public UInt16 length;
      public UInt16 type;
      private byte[] raw;
      private byte[] inflated;
      private byte[] lut; // lookup table used for types 4 and 5

      public BlastCorpsTexture(UInt32 offset, UInt16 length, UInt16 type)
      {
         this.offset = offset;
         this.length = length;
         this.type = type;
      }

      public BlastCorpsTexture(byte[] data, uint index, uint lutOffset)
      {
         offset = BE.U32(data, ROM_OFFSET + 8 * index);
         length = BE.U16(data, ROM_OFFSET + 8 * index + 4);
         type = BE.U16(data, ROM_OFFSET + 8 * index + 6);
         raw = new byte[length];
         Array.Copy(data, ROM_OFFSET + offset, raw, 0, length);
         if (type == 4 || type == 5)
         {
            lut = new byte[4096];
            Array.Copy(data, lutOffset, lut, 0, lut.Length);
         }
      }

      public byte[] GetInflated()
      {
         return inflated;
      }

      public void decode()
      {
         switch (type)
         {
            case 0: inflated = decode0(raw); break;
            case 1: inflated = decode1(raw); break;
            case 2: inflated = decode2(raw); break;
            // TODO: need to figure out where last param is set for decoders 4 and 5
            case 4: inflated = decode4(raw, lut); break;
            case 5: inflated = decode5(raw, lut); break;
            case 3: inflated = decode3(raw); break;
            case 6: inflated = decode6(raw); break;
         }
      }

      // just a memcpy from source to destination
      public static byte[] decode0(byte[] source)
      {
         byte[] retBuffer = new byte[source.Length];
         Array.Copy(source, retBuffer, retBuffer.Length);
         return retBuffer;
      }

      public static byte[] decode1(byte[] source)
      {
         byte[] retBuffer = new byte[256 * 256];
         int remaining = source.Length;
         uint inIdx = 0;
         int outIdx = 0;
         UInt16 t0, t1, t3;
         int t2;
         while (remaining != 0)
         {
            t0 = BE.U16(source, inIdx);
            inIdx += 2; // a0
            if ((t0 & 0x8000) == 0) {
               t1 = (UInt16)((t0 & 0xFFC0) << 1);
               t0 &= 0x3F;
               t0 = (UInt16)(t0 | t1);
               BE.ToBytes(t0, retBuffer, outIdx);
               outIdx += 2;
               remaining -= 2;
            } else {
               t1 = (UInt16)(t0 & 0x1F); // lookback length
               t0 = (UInt16)((t0 & 0x7FFF) >> 5); // lookback offset
               remaining -= 2; // a1
               t2 = outIdx - t0; // t2 - lookback pointer from current out
               while (t1 != 0) {
                  t3 = BE.U16(retBuffer, (uint)t2);
                  t2 += 2;
                  outIdx += 2;
                  t1 -= 1;
                  BE.ToBytes(t3, retBuffer, outIdx - 2);
               }
            }
         }
         Array.Resize<byte>(ref retBuffer, outIdx);
         return retBuffer;
      }

      public static byte[] decode2(byte[] source)
      {
         byte[] retBuffer = new byte[256 * 256];
         int remaining = source.Length;
         uint inIdx = 0;
         uint outIdx = 0;
         uint lookIdx;
         UInt32 t0, t1, t2, t3;
         while (remaining != 0) {
            t0 = BE.U16(source, inIdx);
            inIdx += 2;
            if ((t0 & 0x8000) == 0) { // t0 >= 0
               t1 = t0 & 0x7800;
               t2 = t0 & 0x0780;
               t1 <<= 17; // 0x11
               t2 <<= 13; // 0xD;
               t1 |= t2;
               t2 = t0 & 0x78;
               t2 <<= 9;
               t1 |= t2;
               t2 = t0 & 7;
               t2 <<= 5;
               t1 |= t2;
               BE.ToBytes(t1, retBuffer, (int)outIdx);
               outIdx += 4;
               remaining -= 2;
            } else {
               t1 = t0 & 0x1f;
               t0 &= 0x7FE0;
               t0 >>= 4;
               remaining -= 2;
               lookIdx = outIdx - t0; // t2
               while (t1 != 0) {
                  t3 = BE.U32(retBuffer, lookIdx);
                  BE.ToBytes(t3, retBuffer, (int)outIdx);
                  lookIdx += 4; // t2
                  t1 -= 1;
                  outIdx += 4;
               }
            }
         }
         Array.Resize<byte>(ref retBuffer, (int)outIdx);
         return retBuffer;
      }

      public static byte[] decode3(byte[] source)
      {
         byte[] retBuffer = new byte[256 * 256];
         int remaining = source.Length;
         uint inIdx = 0;
         uint outIdx = 0;
         UInt16 t0, t1, t3;
         uint t2Idx;
         while (remaining != 0) {
            t0 = BE.U16(source, inIdx);
            inIdx += 2;
            if ((0x8000 & t0) == 0) {
               t1 = (UInt16)(t0 >> 8);
               t1 <<= 1;
               retBuffer[outIdx] = (byte)t1; // sb
               t1 = (UInt16)(t0 & 0xFF);
               t1 <<= 1;
               retBuffer[outIdx + 1] = (byte)t1; // sb
               outIdx += 2;
               remaining -= 2;
            } else {
               t1 = (UInt16)(t0 & 0x1F);
               t0 &= 0x7FFF;
               t0 >>= 5;
               remaining -= 2;
               t2Idx = outIdx - t0;
               while (t1 != 0) {
                  t3 = BE.U16(retBuffer, t2Idx);
                  t2Idx += 2;
                  t1 -= 1;
                  BE.ToBytes(t3, retBuffer, (int)outIdx);
                  outIdx += 2;
               }
            }
         }
         Array.Resize<byte>(ref retBuffer, (int)outIdx);
         return retBuffer;
      }

      public static byte[] decode4(byte[] source, byte[] lut)
      {
         byte[] retBuffer = new byte[256 * 256];
         int remaining = source.Length;
         uint inIdx = 0;
         int outIdx = 0;
         uint lutIdx;
         UInt32 t3;
         UInt16 t0, t1, t2;
         while (remaining != 0) {
            t0 = BE.U16(source, inIdx);
            inIdx += 2;
            if ((t0 & 0x8000) == 0) {
               t1 = (UInt16)(t0 >> 8);
               t2 = (UInt16)(t1 & 0xFE);
               t2 = BE.U16(lut, t2); // t2 += t4; // t4 set in proc_802A57DC: lw    $t4, 0xc($a0)
               t1 &= 1;
               t2 <<= 1;
               t1 |= t2;
               BE.ToBytes(t1, retBuffer, outIdx);
               outIdx += 2;
               t1 = (UInt16)(t0 & 0xFE);
               t1 = BE.U16(lut, t1);
               t0 &= 1;
               remaining -= 2;
               t1 <<= 1;
               t1 |= t0;
               BE.ToBytes(t1, retBuffer, outIdx);
               outIdx += 2;
            } else {
               t1 = (UInt16)(t0 & 0x1F);
               t0 &= 0x7FE0;
               t0 >>= 4;
               remaining -= 2;
               lutIdx = (uint)(outIdx - t0);
               while (t1 != 0) {
                  t3 = BE.U32(lut, lutIdx);
                  lutIdx += 4;
                  t1 -= 1;
                  BE.ToBytes(t3, retBuffer, outIdx);
                  outIdx += 4;
               }
            }
         }
         Array.Resize<byte>(ref retBuffer, outIdx);
         return retBuffer;
      }

      public static byte[] decode5(byte[] source, byte[] lut)
      {
         byte[] retBuffer = new byte[256 * 256];
         int remaining = source.Length;
         uint inIdx = 0;
         int outIdx = 0;
         int lutIdx;
         int t0, t1, t2, t3;
         while (remaining != 0) {
            t0 = BE.U16(source, inIdx);
            inIdx += 2;
            if ((t0 & 0x8000) == 0) { // bltz
               t1 = t0 >> 4;
               t1 = t1 << 1;
               lutIdx = t1; // t1 += t4
               t1 = BE.U16(lut, (uint)lutIdx);
               t0 &= 0xF;
               t0 <<= 4;
               t2 = t1 & 0x7C00;
               t3 = t1 & 0x03E0;
               t2 <<= 17; // 0x11
               t3 <<= 14; // 0xe
               t2 |= t3;
               t3 = t1 & 0x1F;
               t3 <<= 11; // 0xb
               t2 |= t3;
               t2 |= t0;
               BE.ToBytes((UInt32)t2, retBuffer, outIdx);
               outIdx += 4;
               remaining -= 2;
            } else {
               t1 = t0 & 0x1F;
               t0 &= 0x7FE0;
               t0 >>= 4;
               remaining -= 2;
               int tmpIdx = outIdx - t0; // t2
               while (t1 != 0) {
                  UInt32 t3u32 = BE.U32(retBuffer, (uint)tmpIdx); //t2
                  tmpIdx += 4; // t2
                  t1 -= 1;
                  BE.ToBytes(t3u32, retBuffer, outIdx);
                  outIdx += 4;
               }
            }
         }
         Array.Resize<byte>(ref retBuffer, outIdx);
         return retBuffer;
      }

      public static byte[] decode6(byte[] source)
      {
         byte[] retBuffer = new byte[256 * 256];
         int remaining = source.Length;
         uint inIdx = 0;
         int outIdx = 0;
         UInt16 t0, t1;
         while (remaining != 0) {
            t0 = BE.U16(source, inIdx);
            inIdx += 2;
            if ((0x8000 & t0) == 0) {
               UInt16 t2;
               t1 = (UInt16)(t0 >> 8);
               t2 = (UInt16)(t1 & 0x38);
               t1 = (UInt16)(t1 & 0x07);
               t2 <<= 2;
               t1 <<= 1;
               t1 |= t2;
               retBuffer[outIdx] = (byte)t1; // sb
               t1 = (UInt16)(t0 & 0xFF);
               t2 = (UInt16)(t1 & 0x38);
               t1 = (UInt16)(t1 & 0x07);
               t2 <<= 2;
               t1 <<= 1;
               t1 |= t2;
               retBuffer[outIdx + 1] = (byte)t1;
               outIdx += 2;
               remaining -= 2;
            } else {
               uint t2;
               t1 = (UInt16)(t0 & 0x1F);
               t0 = (UInt16)(t0 & 0x7FFF);
               t0 >>= 5;
               remaining -= 2;
               t2 = (uint)(outIdx - t0);
               while (t1 != 0) {
                  UInt16 t3u16 = BE.U16(retBuffer, t2);
                  t2 += 2;
                  t1 -= 1;
                  BE.ToBytes(t3u16, retBuffer, outIdx);
                  outIdx += 2;
               }
            }
         }
         Array.Resize<byte>(ref retBuffer, outIdx);
         return retBuffer;
      }
   }
}
