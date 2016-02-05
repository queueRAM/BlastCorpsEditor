using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace BlastCorpsEditor
{
   public partial class BlastCorpsViewer : UserControl
   {
      private BlastCorpsLevel level;
      private bool useGridLines;
      private ToolStripStatusLabel mouseStatus;

      public BlastCorpsViewer()
      {
         InitializeComponent();
         DoubleBuffered = true;
         ResizeRedraw = true;
      }

      public void SetStatusLabel(ToolStripStatusLabel label)
      {
         mouseStatus = label;
      }

      public bool UseGridLines
      {
         get { return useGridLines; }
         set { useGridLines = value; Invalidate(); }
      }

      public void SetLevel(BlastCorpsLevel level)
      {
         this.level = level;
         Invalidate();
      }

      private int pixelX(int levelX)
      {
         return Width - ((levelX - level.bounds.x1) * Width / (level.bounds.x2 - level.bounds.x1));
      }

      private int pixelY(int levelZ)
      {
         return Height - ((levelZ - level.bounds.z1) * Height / (level.bounds.z2 - level.bounds.z1));
      }

      private int levelX(int pixelX)
      {
         // pixelX = Width - ((levelX - level.bounds.x1) * Width / (level.bounds.x2 - level.bounds.x1))
         // ((levelX - level.bounds.x1) * Width / (level.bounds.x2 - level.bounds.x1)) + pixelX = Width
         // (levelX - level.bounds.x1) * Width / (level.bounds.x2 - level.bounds.x1) = Width - pixelX
         // (levelX - level.bounds.x1) = (Width - pixelX) * (level.bounds.x2 - level.bounds.x1) / Width
         // levelX = (Width - pixelX) * (level.bounds.x2 - level.bounds.x1) / Width + level.bounds.x1
         return (Width - pixelX) * (level.bounds.x2 - level.bounds.x1) / Width + level.bounds.x1;
      }

      private int levelZ(int pixelY)
      {
         // pixelX = Width - ((levelX - level.bounds.x1) * Width / (level.bounds.x2 - level.bounds.x1))
         // ((levelX - level.bounds.x1) * Width / (level.bounds.x2 - level.bounds.x1)) + pixelX = Width
         // (levelX - level.bounds.x1) * Width / (level.bounds.x2 - level.bounds.x1) = Width - pixelX
         // (levelX - level.bounds.x1) = (Width - pixelX) * (level.bounds.x2 - level.bounds.x1) / Width
         // levelX = (Width - pixelX) * (level.bounds.x2 - level.bounds.x1) / Width + level.bounds.x1
         return (Height - pixelY) * (level.bounds.z2 - level.bounds.z1) / Height + level.bounds.z1;
      }

      private void BlastCorpsViewer_Paint(object sender, PaintEventArgs e)
      {
         Brush ammoBrush = new SolidBrush(Color.LightBlue);
         Brush commBrush = new SolidBrush(Color.Orange);
         Brush rduBrush = new SolidBrush(Color.Goldenrod);
         Brush tntBrush = new SolidBrush(Color.Black);
         Brush blockBrush = new SolidBrush(Color.DarkGray);
         Pen blockPen = new Pen(Color.DarkGray);
         Brush vehBrush = new SolidBrush(Color.Blue);
         Brush selectedBrush = new SolidBrush(Color.Red);
         Pen vehPen = new Pen(Color.Blue);
         Pen zonePen = new Pen(Color.Plum);
         vehPen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
         if (level != null)
         {
            if (useGridLines)
            {
               // something like every 500
               Pen linePen = new Pen(Color.Black);
               linePen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
               const int every = 500;
               int start = ((level.bounds.x1 + every - 1) / every) * every;
               int end = ((level.bounds.x2 + every - 1) / every) * every;
               for (int x = start; x < end; x += every)
               {
                  int px = pixelX(x);
                  e.Graphics.DrawLine(linePen, px, 0, px, Height);
               }
               start = ((level.bounds.z1 + every - 1) / every) * every;
               end = ((level.bounds.z2 + every - 1) / every) * every;
               for (int y = start; y < end; y += every)
               {
                  int py = pixelY(y);
                  e.Graphics.DrawLine(linePen, 0, py, Width, py);
               }
            }
            foreach (AmmoBox ammo in level.ammoBoxes)
            {
               e.Graphics.FillRectangle(ammoBrush, pixelX(ammo.x), pixelY(ammo.z), 7, 7);
            }
            Point[] points = new Point[4];
            foreach (Zone zone in level.zones)
            {
               points[0].X = pixelX(zone.x1);
               points[0].Y = pixelY(zone.z1);
               points[1].X = pixelX(zone.x2);
               points[1].Y = pixelY(zone.z2);
               points[2].X = pixelX(zone.x3);
               points[2].Y = pixelY(zone.z3);
               points[3].X = pixelX(zone.x1);
               points[3].Y = pixelY(zone.z1);
               e.Graphics.DrawLines(zonePen, points);
            }
            foreach (CommPoint comm in level.commPoints)
            {
               e.Graphics.FillRectangle(commBrush, pixelX(comm.x), pixelY(comm.z), 10, 10);
            }
            foreach (RDU rdu in level.rdus)
            {
               e.Graphics.FillEllipse(rdu.selected ? selectedBrush : rduBrush, pixelX(rdu.x), pixelY(rdu.z), 7, 7);
            }
            foreach (TNTCrate tnt in level.tntCrates)
            {
               e.Graphics.FillRectangle(tntBrush, pixelX(tnt.x), pixelY(tnt.z), 7, 7);
            }
            foreach (SquareBlock block in level.squareBlocks)
            {
               if ((block.hole == 8 && block.type > 0) || (block.hole != 8 && block.hole > 0))
               {
                  e.Graphics.RotateTransform(45);
                  e.Graphics.TranslateTransform(pixelX(block.x), pixelY(block.z)-4, MatrixOrder.Append);
               }
               else
               {
                  e.Graphics.TranslateTransform(pixelX(block.x)-4, pixelY(block.z), MatrixOrder.Append);
               }
               if (block.hole == 8)
               {
                  e.Graphics.DrawRectangle(blockPen, 0, 0, 9, 9);
               }
               else
               {
                  e.Graphics.FillRectangle(blockBrush, 0, 0, 7, 7);
               }
               e.Graphics.ResetTransform();
            }
            foreach (Vehicle veh in level.vehicles)
            {
               int x = pixelX(veh.x);
               int y = pixelY(veh.z);
               e.Graphics.FillRectangle(vehBrush, x, y, 5, 5);
               double angle = Math.PI * veh.heading / 2048.0;
               // sin/cos swapped so N = 0, W = 1024
               int dx = (int)(17 * Math.Sin(angle));
               int dy = (int)(17 * Math.Cos(angle));
               e.Graphics.DrawLine(vehPen, x+2, y+2, x-dx+2, y-dy+2);
            }
         }
         e.Graphics.DrawRectangle(new Pen(Color.Black), 0, 0, Width - 1, Height - 1);
      }

      private void BlastCorpsViewer_MouseMove(object sender, MouseEventArgs e)
      {
         if (mouseStatus != null)
         {
            mouseStatus.Text = levelX(e.X) + "," + levelZ(e.Y) + " [" + e.X + "," + e.Y + "]";
         }
      }
   }
}
