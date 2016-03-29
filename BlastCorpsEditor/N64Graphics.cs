using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlastCorpsEditor
{
   enum N64Format { RGBA, IA }

   class N64Graphics
   {
      public static int SCALE_5_8(int val)
      {
         return (((val) * 0xFF) / 0x1F);
      }

      public static int SCALE_4_8(int val)
      {
         return ((val) * 0x11);
      }

      public static int SCALE_3_8(int val)
      {
         return ((val) * 0x24);
      }

      private static void SetPixel(Graphics g, int x, int y, Brush b)
      {
         g.FillRectangle(b, x, y, 1, 1);
      }

      public static Bitmap RGBA(byte[] raw, int width, int height, int depth)
      {
         Bitmap bitmap = new Bitmap(width, height);

         Graphics graphics = Graphics.FromImage(bitmap);
         SolidBrush brush = new SolidBrush(Color.Magenta);
         if (depth == 16)
         {
            for (int i = 0; i < width * height; i++)
            {
               int idx = i * 2;
               if (idx + 2 > raw.Length)
               {
                  break;
               }
               int red = SCALE_5_8((raw[idx] & 0xF8) >> 3);
               int green = SCALE_5_8(((raw[idx] & 0x07) << 2) | ((raw[idx + 1] & 0xC0) >> 6));
               int blue = SCALE_5_8((raw[idx + 1] & 0x3E) >> 1);
               int alpha = ((raw[idx + 1] & 0x01) > 0) ? 0xFF : 0x00;
               brush.Color = Color.FromArgb(alpha, red, green, blue);
               int y = i / width;
               int x = i % width;
               SetPixel(graphics, x, y, brush);
            }
         }
         else if (depth == 32)
         {
            for (int i = 0; i < width * height; i++)
            {
               int idx = i * 4;
               if (idx + 4 > raw.Length)
               {
                  break;
               }
               int red = raw[idx];
               int green = raw[idx + 1];
               int blue = raw[idx + 2];
               int alpha = raw[idx + 3];
               brush.Color = Color.FromArgb(alpha, red, green, blue);
               int y = i / width;
               int x = i % width;
               SetPixel(graphics, x, y, brush);
            }
         }

         return bitmap;
      }

      public static Bitmap IA(byte[] raw, int width, int height, int depth)
      {
         Bitmap bitmap = new Bitmap(width, height);

         Graphics graphics = Graphics.FromImage(bitmap);
         SolidBrush brush = new SolidBrush(Color.Magenta);
         switch (depth) {
            case 16:
               for (int i = 0; i < width * height; i++) {
                  int idx = i * 2;
                  if (idx + 2 > raw.Length)
                  {
                     break;
                  }
                  int intensity = raw[idx];
                  int alpha = raw[idx + 1];
                  brush.Color = Color.FromArgb(alpha, intensity, intensity, intensity);
                  int y = i / width;
                  int x = i % width;
                  SetPixel(graphics, x, y, brush);
               }
               break;
            case 8:
               for (int i = 0; i < width * height; i++) {
                  if (i + 1 > raw.Length)
                  {
                     break;
                  }
                  int intensity = SCALE_4_8((raw[i] & 0xF0) >> 4);
                  int alpha = SCALE_4_8(raw[i] & 0x0F);
                  brush.Color = Color.FromArgb(alpha, intensity, intensity, intensity);
                  int y = i / width;
                  int x = i % width;
                  SetPixel(graphics, x, y, brush);
               }
               break;
            case 4:
               for (int i = 0; i < width * height; i++) {
                  int idx = i / 2;
                  if (idx >= raw.Length)
                  {
                     break;
                  }
                  byte bits;
                  bits = raw[idx];
                  if (i % 2 > 0) {
                     bits &= 0xF;
                  } else {
                     bits >>= 4;
                  }
                  int intensity = SCALE_3_8((bits >> 1) & 0x07);
                  int alpha = (bits & 0x01) > 0 ? 0xFF : 0x00;
                  brush.Color = Color.FromArgb(alpha, intensity, intensity, intensity);
                  int y = i / width;
                  int x = i % width;
                  SetPixel(graphics, x, y, brush);
               }
               break;
            case 1:
               for (int i = 0; i < width * height; i++) {
                  int idx = i / 8;
                  if (idx >= raw.Length)
                  {
                     break;
                  }
                  int bits;
                  int mask;
                  bits = raw[i/8];
                  mask = 1 << (7 - (i % 8)); // MSb->LSb
                  bits = ((bits & mask) > 0) ? 0xFF : 0x00;
                  int intensity = bits;
                  int alpha = bits;
                  brush.Color = Color.FromArgb(alpha, intensity, intensity, intensity);
                  int y = i / width;
                  int x = i % width;
                  SetPixel(graphics, x, y, brush);
               }
               break;
            default:
               break;
         }

         return bitmap;
      }
   }
}
