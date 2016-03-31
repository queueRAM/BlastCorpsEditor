using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlastCorpsEditor
{
   public partial class TextureForm : Form
   {
      private BlastCorpsRom rom;
      private Bitmap loadedBitmap;

      public TextureForm(BlastCorpsRom rom)
      {
         InitializeComponent();
         this.rom = rom;
      }

      private void GuessDims(int type, int length, ref int width, ref int height, ref N64Format format, ref int depth)
      {
         const int KB = 1024;
         switch (type)
         {
            case 0: // IA8?
               // TODO: memcpy, no info
               format = N64Format.IA;
               depth = 8;
               switch (length)
               {
                  case 256: width = 16; height = 16; break;
                  case 512: width = 32; height = 16; break;
                  case 1 * KB: width = 32; height = 32; break;
                  case 2 * KB: width = 64; height = 32; break;
                  case 4 * KB: width = 64; height = 64; break;
                  default: width = 16; height = (length * 8 / depth) / width; break;
               }
               break;
            case 1: // RBGA16?
               format = N64Format.RGBA;
               depth = 16;
               switch (length)
               {
                  case 16: width = 4; height = 2; break;
                  case 512: width = 16; height = 16; break;
                  case 1 * KB: width = 16; height = 32; break;
                  case 2 * KB: width = 32; height = 32; break;
                  case 4 * KB: width = 64; height = 32; break;
                  case 8 * KB: width = 64; height = 64; break;
                  case 0xC80: width = 40; height = 40; break;
                  default: width = 32; height = length / width / 2; break;
               }
               break;
            case 2: // RGBA32?
               format = N64Format.RGBA;
               depth = 32;
               switch (length)
               {
                  case 1 * KB: width = 16; height = 16; break;
                  case 2 * KB: width = 16; height = 32; break;
                  case 4 * KB: width = 32; height = 32; break;
                  case 8 * KB: width = 64; height = 32; break;
                  default: width = 32; height = length / width / 4; break;
               }
               break;
            case 3: // IA8?
               format = N64Format.IA;
               depth = 8;
               switch (length)
               {
                  case 1 * KB: width = 32; height = 32; break;
                  case 2 * KB: width = 32; height = 64; break;
                  case 4 * KB: width = 64; height = 64; break;
                  case 8 * KB: width = 64; height = 128; break;
                  default: width = 32; height = length / width; break;
               }
               break;
            case 4: // IA16?
               format = N64Format.IA;
               depth = 16;
               switch (length)
               {
                  case 1 * KB: width = 32; height = 16; break;
                  case 2 * KB: width = 32; height = 32; break;
                  case 4 * KB: width = 32; height = 64; break;
                  case 8 * KB: width = 64; height = 64; break;
                  default: width = 32; height = length / width / 2; break;
               }
               break;
            case 5: // RGBA32?
               format = N64Format.RGBA;
               depth = 32;
               switch (length)
               {
                  case 1 * KB: width = 16; height = 16; break;
                  case 2 * KB: width = 32; height = 16; break;
                  case 4 * KB: width = 32; height = 32; break;
                  case 8 * KB: width = 64; height = 32; break;
                  default: width = 32; height = length / width / 2; break;
               }
               break;
            case 6: // IA8? IA4 always has alpha (lsb) clear
               format = N64Format.IA;
               depth = 8;
               width = 16;
               height = (length * 8 / depth) / width;
               break;
         }
      }

      private void LoadImage(uint index, int width, int height, N64Format format, int depth)
      {
         BlastCorpsTexture texture = new BlastCorpsTexture(rom.GetRawData(), index, (uint)numericLut.Value);
         texture.decode();
         byte[] n64Texture = texture.GetInflated();
         textBoxOffset.Text = texture.offset.ToString("X");
         textBoxLength.Text = texture.length.ToString("X");
         textBoxType.Text = texture.type.ToString();
         loadedBitmap = null;
         if (n64Texture == null || n64Texture.Length == 0)
         {
            textBoxUncompressed.Text = "null";
         }
         else
         {
            textBoxUncompressed.Text = n64Texture.Length.ToString("X");
            if (width == 0)
            {
               GuessDims(texture.type, n64Texture.Length, ref width, ref height, ref format, ref depth);
               numericHeight.Value = height;
               numericWidth.Value = width;
               switch (format)
               {
                  case N64Format.RGBA:
                     comboBoxFormat.SelectedIndex = (depth == 32) ? 1 : 0;
                     break;
                  case N64Format.IA:
                     switch (depth)
                     {
                        case 16: comboBoxFormat.SelectedIndex = 2; break;
                        case 8: comboBoxFormat.SelectedIndex = 3; break;
                        case 4: comboBoxFormat.SelectedIndex = 4; break;
                        case 1: comboBoxFormat.SelectedIndex = 5; break;
                     }
                     break;
               }
            }
            switch (format)
            {
               case N64Format.RGBA:
                  loadedBitmap = N64Graphics.RGBA(n64Texture, width, height, depth);
                  break;
               case N64Format.IA:
                  loadedBitmap = N64Graphics.IA(n64Texture, width, height, depth);
                  break;
            }

            if (loadedBitmap != null)
            {
               loadedBitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
            }
         }
         pictureBoxTexture.Image = loadedBitmap;
         pictureBoxActual.Image = loadedBitmap;
         buttonExport.Enabled = (loadedBitmap != null);
         numericLut.Enabled = (texture.type == 4 || texture.type == 5);
      }

      private void LoadImage()
      {
         LoadImage((uint)numericImage.Value, 0, 0, N64Format.RGBA, 0);
      }

      private void TextureForm_Load(object sender, EventArgs e)
      {
         LoadImage();
      }

      private void numericImage_ValueChanged(object sender, EventArgs e)
      {
         LoadImage();
      }

      private void buttonReload_Click(object sender, EventArgs e)
      {
         N64Format format = N64Format.RGBA;
         int depth = 16;
         switch (comboBoxFormat.SelectedIndex)
         {
            case 0: format = N64Format.RGBA; depth = 16; break;
            case 1: format = N64Format.RGBA; depth = 32; break;
            case 2: format = N64Format.IA; depth = 16; break;
            case 3: format = N64Format.IA; depth = 8; break;
            case 4: format = N64Format.IA; depth = 4; break;
            case 5: format = N64Format.IA; depth = 1; break;
         }
         LoadImage((uint)numericImage.Value, (int)numericWidth.Value, (int)numericHeight.Value, format, depth);
      }

      private void buttonExport_Click(object sender, EventArgs e)
      {
         SaveFileDialog saveFileDialog = new SaveFileDialog();
         saveFileDialog.Filter = "PNG Image(*.png)|*.png";
         saveFileDialog.Title = "Export Texture to PNG";
         saveFileDialog.ShowDialog();

         if (saveFileDialog.FileName != "")
         {
            loadedBitmap.Save(saveFileDialog.FileName);
         }
      }
   }
}
