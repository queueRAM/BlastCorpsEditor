using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace BlastCorpsEditor
{
   public partial class BlastCorpsViewer : UserControl
   {
      private BlastCorpsLevel level;
      private bool showGridLines;
      private bool showBoundingBoxes40;
      private bool showBoundingBoxes44;
      private ToolStripStatusLabel mouseStatus;
      private float zoom = 1;
      private int offX, offY;

      // click and drag related
      enum DragType { None, Ammo, Comm, RDU, TNT, Block, Building, Vehicle, Carrier };
      DragType dragType = DragType.None;
      private AmmoBox dragAmmo;
      private CommPoint dragComm;
      private RDU dragRdu;
      private TNTCrate dragTnt;
      private SquareBlock dragBlock;
      private Vehicle dragVehicle;
      private Building dragBuilding;

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

      public bool ShowGridLines
      {
         get { return showGridLines; }
         set { showGridLines = value; Invalidate(); }
      }

      public bool ShowBoundingBoxes40
      {
         get { return showBoundingBoxes40; }
         set { showBoundingBoxes40 = value; Invalidate(); }
      }

      public bool ShowBoundingBoxes44
      {
         get { return showBoundingBoxes44; }
         set { showBoundingBoxes44 = value; Invalidate(); }
      }

      public void SetLevel(BlastCorpsLevel level)
      {
         this.level = level;
         computeBounds();
         Invalidate();
      }

      private void computeBounds()
      {
         // pick higher level/screen ratio
         float ratX = (float)(level.bounds.x2 - level.bounds.x1) / (float)Width;
         float ratY = (float)(level.bounds.z2 - level.bounds.z1) / (float)Height;
         if (ratX > ratY)
         {
            zoom = ratX;
            offX = 0;
            offY = (Height - (int)((level.bounds.z2 - level.bounds.z1) / zoom)) / 2;
         }
         else
         {
            zoom = ratY;
            offX = (Width - (int)((level.bounds.x2 - level.bounds.x1) / zoom)) / 2;
            offY = 0;
         }
      }

      private int pixelX(int levelX)
      {
         return (int)(Width - (levelX - level.bounds.x1) / zoom) - offX;
      }

      private int pixelY(int levelZ)
      {
         return (int)(Height - (levelZ - level.bounds.z1) / zoom) - offY;
      }

      private int levelX(int pixelX)
      {
         // pixelX = (Width - (levelX - level.bounds.x1) / zoom) - offX;
         return (int)((Width - pixelX - offX) * zoom) + level.bounds.x1;
      }

      private int levelZ(int pixelY)
      {
         // pixelY = (Height - (levelZ - level.bounds.z1) / zoom) - offY;
         return (int)((Height - pixelY - offY) * zoom) + level.bounds.z1;
      }

      private void BlastCorpsViewer_Paint(object sender, PaintEventArgs e)
      {
         Brush ammoBrush = new SolidBrush(Color.LightBlue);
         Brush commBrush = new SolidBrush(Color.Orange);
         Brush rduBrush = new SolidBrush(Color.Goldenrod);
         Brush tntBrush = new SolidBrush(Color.Black);
         Brush blockBrush = new SolidBrush(Color.DarkGray);
         Brush bounds40Brush = new HatchBrush(HatchStyle.DiagonalCross, Color.Green, Color.Transparent);
         Brush bounds44Brush = new HatchBrush(HatchStyle.DiagonalCross, Color.Aqua, Color.Transparent);
         Pen blockPen = new Pen(Color.DarkGray);
         Pen buildingPen = new Pen(Color.Brown, 2);
         Brush selectedBrush = new SolidBrush(Color.Red);
         Pen vehPen = new Pen(Color.Blue);
         vehPen.StartCap = LineCap.SquareAnchor;
         vehPen.EndCap = LineCap.ArrowAnchor;
         Pen playerPen = new Pen(Color.Green, 3);
         playerPen.StartCap = LineCap.SquareAnchor;
         playerPen.EndCap = LineCap.ArrowAnchor;
         Pen carrierPen = new Pen(Color.Red, 3);
         carrierPen.DashStyle = DashStyle.DashDot;
         carrierPen.StartCap = LineCap.SquareAnchor;
         carrierPen.EndCap = LineCap.ArrowAnchor;
         Pen zonePen = new Pen(Color.Plum);
         Pen blackPen = new Pen(Color.Black);
         double angle;
         int x, y, dx, dy;
         if (level != null)
         {
            e.Graphics.FillRectangle(new SolidBrush(Color.White), offX, offY, Width - 2*offX, Height - 2*offY);
            if (showGridLines)
            {
               // something like every 500
               Pen linePen = new Pen(Color.Black);
               linePen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
               const int every = 500;
               int start = ((level.bounds.x1 + every - 1) / every) * every;
               int end = ((level.bounds.x2 + every - 1) / every) * every;
               for (x = start; x < end; x += every)
               {
                  int px = pixelX(x);
                  e.Graphics.DrawLine(linePen, px, offY, px, Height-offY);
               }
               start = ((level.bounds.z1 + every - 1) / every) * every;
               end = ((level.bounds.z2 + every - 1) / every) * every;
               for (y = start; y < end; y += every)
               {
                  int py = pixelY(y);
                  e.Graphics.DrawLine(linePen, offX, py, Width-offX, py);
               }
            }
            if (showBoundingBoxes40)
            {
               foreach (Bounds bounds in level.bounds40)
               {
                  if (bounds.todo > 0x00A9)
                  {
                     int x1 = pixelX(bounds.x1);
                     int y1 = pixelY(bounds.z1);
                     int x2 = pixelX(bounds.x2);
                     int y2 = pixelY(bounds.z2);
                     e.Graphics.FillRectangle(bounds40Brush, x2, y2, x1 - x2, y1 - y2);
                     e.Graphics.DrawRectangle(blackPen, x2, y2, x1 - x2, y1 - y2);
                  }
               }
            }
            if (showBoundingBoxes44)
            {
               foreach (Bounds bounds in level.bounds44)
               {
                  int x1 = pixelX(bounds.x1);
                  int y1 = pixelY(bounds.z1);
                  int x2 = pixelX(bounds.x2);
                  int y2 = pixelY(bounds.z2);
                  e.Graphics.FillRectangle(bounds44Brush, x2, y2, x1 - x2, y1 - y2);
                  e.Graphics.DrawRectangle(blackPen, x2, y2, x1 - x2, y1 - y2);
               }
            }

            // missile carrier
            x = pixelX(level.carrier.x);
            y = pixelY(level.carrier.z);
            angle = Math.PI * level.carrier.heading / 2048.0;
            // sin/cos swapped so N = 0, W = 1024
            int length = pixelY(0) - pixelY(level.carrier.distance);
            dx = (int)(length * Math.Sin(angle));
            dy = (int)(length * Math.Cos(angle));
            e.Graphics.DrawLine(carrierPen, x, y, x - dx, y - dy);

            foreach (AmmoBox ammo in level.ammoBoxes)
            {
               e.Graphics.FillRectangle(ammoBrush, pixelX(ammo.x)-4, pixelY(ammo.z)-4, 7, 7);
            }
            Point[] points = new Point[4];
            foreach (Collision24 zone in level.collisions)
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
               e.Graphics.FillRectangle(commBrush, pixelX(comm.x)-5, pixelY(comm.z)-5, 9, 9);
            }
            foreach (RDU rdu in level.rdus)
            {
               e.Graphics.FillEllipse(rdu.selected ? selectedBrush : rduBrush, pixelX(rdu.x)-4, pixelY(rdu.z)-4, 7, 7);
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
               x = pixelX(veh.x);
               y = pixelY(veh.z);
               angle = Math.PI * veh.heading / 2048.0;
               // sin/cos swapped so N = 0, W = 1024
               dx = (int)(12 * Math.Sin(angle));
               dy = (int)(12 * Math.Cos(angle));
               if (veh.type == 0x00)
               {
                  e.Graphics.DrawLine(playerPen, x, y, x - dx, y - dy);
               }
               else
               {
                  e.Graphics.DrawLine(vehPen, x, y, x - dx, y - dy);
               }
            }
            foreach (Building b in level.buildings)
            {
               e.Graphics.DrawRectangle(buildingPen, pixelX(b.x)-5, pixelY(b.z)-5, 9, 9);
            }
         }
         e.Graphics.DrawRectangle(blackPen, 0, 0, Width - 1, Height - 1);
      }

      private void BlastCorpsViewer_Resize(object sender, EventArgs e)
      {
         if (level != null)
         {
            computeBounds();
         }
      }

      private void BlastCorpsViewer_MouseMove(object sender, MouseEventArgs e)
      {
         if (mouseStatus != null)
         {
            mouseStatus.Text = levelX(e.X) + "," + levelZ(e.Y);
         }

         if (dragType != DragType.None)
         {
            int x = levelX(e.X);
            int z = levelZ(e.Y);
            switch (dragType)
            {
               case DragType.Ammo:
                  dragAmmo.x = (Int16)x;
                  dragAmmo.z = (Int16)z;
                  break;
               case DragType.Comm:
                  dragComm.x = (Int16)x;
                  dragComm.z = (Int16)z;
                  break;
               case DragType.RDU:
                  dragRdu.x = (Int16)x;
                  dragRdu.z = (Int16)z;
                  break;
               case DragType.TNT:
                  dragTnt.x = (Int16)x;
                  dragTnt.z = (Int16)z;
                  break;
               case DragType.Block:
                  dragBlock.x = (Int16)x;
                  dragBlock.z = (Int16)z;
                  break;
               case DragType.Vehicle:
                  dragVehicle.x = (Int16)x;
                  dragVehicle.z = (Int16)z;
                  break;
               case DragType.Carrier:
                  level.carrier.x = (Int16)x;
                  level.carrier.z = (Int16)z;
                  break;
               case DragType.Building:
                  dragBuilding.x = (Int16)x;
                  dragBuilding.z = (Int16)z;
                  break;
            }
            Invalidate();
         }
      }

      private void BlastCorpsViewer_MouseDown(object sender, MouseEventArgs e)
      {
         if (level != null)
         {
            // find something to move
            int x = levelX(e.X);
            int z = levelZ(e.Y);
            int diff = levelX(0) - levelX(4);

            foreach (AmmoBox ammo in level.ammoBoxes)
            {
               if (Math.Abs(x - ammo.x) < diff && Math.Abs(z - ammo.z) < diff)
               {
                  dragType = DragType.Ammo;
                  dragAmmo = ammo;
                  break;
               }
            }

            foreach (CommPoint comm in level.commPoints)
            {
               if (Math.Abs(x - comm.x) < diff && Math.Abs(z - comm.z) < diff)
               {
                  dragType = DragType.Comm;
                  dragComm = comm;
                  break;
               }
            }

            foreach (RDU rdu in level.rdus)
            {
               if (Math.Abs(x - rdu.x) < diff && Math.Abs(z - rdu.z) < diff)
               {
                  dragType = DragType.RDU;
                  dragRdu = rdu;
                  break;
               }
            }

            foreach (TNTCrate tnt in level.tntCrates)
            {
               if (Math.Abs(x - tnt.x) < diff && Math.Abs(z - tnt.z) < diff)
               {
                  dragType = DragType.TNT;
                  dragTnt = tnt;
                  break;
               }
            }

            foreach (SquareBlock block in level.squareBlocks)
            {
               if (Math.Abs(x - block.x) < diff && Math.Abs(z - block.z) < diff)
               {
                  dragType = DragType.Block;
                  dragBlock = block;
                  break;
               }
            }

            foreach (Vehicle veh in level.vehicles)
            {
               if (Math.Abs(x - veh.x) < diff && Math.Abs(z - veh.z) < diff)
               {
                  dragType = DragType.Vehicle;
                  dragVehicle = veh;
                  break;
               }
            }

            if (Math.Abs(x - level.carrier.x) < diff && Math.Abs(z - level.carrier.z) < diff)
            {
               dragType = DragType.Carrier;
            }

            foreach (Building b in level.buildings)
            {
               if (Math.Abs(x - b.x) < diff && Math.Abs(z - b.z) < diff)
               {
                  dragType = DragType.Building;
                  dragBuilding = b;
                  break;
               }
            }
         }
      }

      private void BlastCorpsViewer_MouseUp(object sender, MouseEventArgs e)
      {
         dragType = DragType.None;
      }
   }
}
