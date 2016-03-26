using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.IO;

namespace BlastCorpsEditor
{
   public partial class BlastCorpsViewer : UserControl
   {
      private BlastCorpsLevel level;
      private bool showGridLines;
      private bool showBoundingBoxes40;
      private bool showBoundingBoxes44;
      private float zoom = 1;
      private int offX, offY;

      // cursors
      private Cursor cursorPlus;
      private Cursor cursorMove;

      private MouseMode mouseMode;

      // item selected either in list or by clicking
      private BlastCorpsItem selectedItem = null;
      // click and drag related
      private BlastCorpsItem dragItem = null;

      public MouseMode Mode
      {
         get
         {
            return mouseMode;
         }
         set
         {
            mouseMode = value;
            switch (mouseMode)
            {
               case MouseMode.Move: this.Cursor = cursorMove; break;
               case MouseMode.Add: this.Cursor = cursorPlus; break;
               default: this.Cursor = Cursors.Default; break;
            }
         }
      }

      public Type AddType { get; set; }

      public BlastCorpsViewer()
      {
         InitializeComponent();
         DoubleBuffered = true;
         ResizeRedraw = true;

         // create cursors
         cursorPlus = new Cursor(new MemoryStream(BlastCorpsEditor.Properties.Resources.cursor_plus));
         cursorMove = new Cursor(new MemoryStream(BlastCorpsEditor.Properties.Resources.cursor_move));
      }

      // selected item changed event
      public event SelectionChangedEventHandler SelectionChangedEvent;
      protected virtual void OnSelectionChangedEvent(SelectionChangedEventArgs e)
      {
         SelectionChangedEvent(this, e);
      }

      // item moved
      public event ItemMovedEventHandler ItemMovedEvent;
      protected virtual void OnItemMovedEvent(ItemMovedEventArgs e)
      {
         ItemMovedEvent(this, e);
      }

      // mouse position change event
      public event PositionEventHandler PositionEvent;
      protected virtual void OnPositionEvent(PositionEventArgs e)
      {
         PositionEvent(this, e);
      }

      public BlastCorpsItem SelectedItem
      {
         get { return selectedItem; }
         set { selectedItem = value; Invalidate(); }
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

      private BlastCorpsItem FindNearbyItem(int pixelX, int pixelY)
      {
         BlastCorpsItem itemFound = null;
         if (level != null)
         {
            // find something to select
            int x = levelX(pixelX);
            int z = levelZ(pixelY);
            int diff = levelX(0) - levelX(4);

            foreach (AmmoBox ammo in level.ammoBoxes)
            {
               if (Math.Abs(x - ammo.x) < diff && Math.Abs(z - ammo.z) < diff)
               {
                  itemFound = ammo;
                  break;
               }
            }

            foreach (CommPoint comm in level.commPoints)
            {
               if (Math.Abs(x - comm.x) < diff && Math.Abs(z - comm.z) < diff)
               {
                  itemFound = comm;
                  break;
               }
            }

            foreach (RDU rdu in level.rdus)
            {
               if (Math.Abs(x - rdu.x) < diff && Math.Abs(z - rdu.z) < diff)
               {
                  itemFound = rdu;
                  break;
               }
            }

            foreach (TNTCrate tnt in level.tntCrates)
            {
               if (Math.Abs(x - tnt.x) < diff && Math.Abs(z - tnt.z) < diff)
               {
                  itemFound = tnt;
                  break;
               }
            }

            foreach (SquareBlock block in level.squareBlocks)
            {
               if (Math.Abs(x - block.x) < diff && Math.Abs(z - block.z) < diff)
               {
                  itemFound = block;
                  break;
               }
            }

            foreach (Vehicle veh in level.vehicles)
            {
               if (Math.Abs(x - veh.x) < diff && Math.Abs(z - veh.z) < diff)
               {
                  itemFound = veh;
                  break;
               }
            }

            if (Math.Abs(x - level.carrier.x) < diff && Math.Abs(z - level.carrier.z) < diff)
            {
               itemFound = level.carrier;
            }

            foreach (Building b in level.buildings)
            {
               if (Math.Abs(x - b.x) < diff && Math.Abs(z - b.z) < diff)
               {
                  itemFound = b;
                  break;
               }
            }
         }
         return itemFound;
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
         Pen stoppingPen = new Pen(Color.SaddleBrown, 2);
         Pen platformPen = new Pen(Color.DarkKhaki, 2);
         Pen blackPen = new Pen(Color.Black);
         Pen selectedPen = new Pen(Color.Magenta, 2);
         selectedPen.EndCap = LineCap.ArrowAnchor;
         double angle;
         int x, y, dx, dy;
         if (level != null)
         {
            e.Graphics.FillRectangle(new SolidBrush(Color.White), offX, offY, Width - 2*offX, Height - 2*offY);
            if (showGridLines)
            {
               // every 500 level units
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
               e.Graphics.FillRectangle(ammoBrush, pixelX(ammo.x) - 4, pixelY(ammo.z) - 4, 7, 7);
            }
            Point[] points = new Point[4];
            foreach (Collision24 zone in level.collision24)
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
            foreach (TrainPlatform platform in level.trainPlatforms)
            {
               foreach (TrainPlatform.StoppingTriangle zone in platform.stoppingZone)
               {
                  points[0].X = pixelX(zone.x1);
                  points[0].Y = pixelY(zone.z1);
                  points[1].X = pixelX(zone.x2);
                  points[1].Y = pixelY(zone.z2);
                  points[2].X = pixelX(zone.x3);
                  points[2].Y = pixelY(zone.z3);
                  points[3].X = pixelX(zone.x1);
                  points[3].Y = pixelY(zone.z1);
                  e.Graphics.DrawLines(stoppingPen, points);
               }
               foreach (TrainPlatform.PlatformCollision collision in platform.collision)
               {
                  points[0].X = pixelX(collision.x1);
                  points[0].Y = pixelY(collision.z1);
                  points[1].X = pixelX(collision.x2);
                  points[1].Y = pixelY(collision.z2);
                  points[2].X = pixelX(collision.x3);
                  points[2].Y = pixelY(collision.z3);
                  points[3].X = pixelX(collision.x1);
                  points[3].Y = pixelY(collision.z1);
                  e.Graphics.DrawLines(platformPen, points);
               }
            }
            foreach (CommPoint comm in level.commPoints)
            {
               e.Graphics.FillRectangle(commBrush, pixelX(comm.x) - 5, pixelY(comm.z) - 5, 9, 9);
            }
            foreach (RDU rdu in level.rdus)
            {
               e.Graphics.FillEllipse(rduBrush, pixelX(rdu.x)-4, pixelY(rdu.z)-4, 7, 7);
            }
            foreach (TNTCrate tnt in level.tntCrates)
            {
               e.Graphics.FillRectangle(tntBrush, pixelX(tnt.x)-4, pixelY(tnt.z)-4, 7, 7);
            }
            foreach (SquareBlock block in level.squareBlocks)
            {
               if ((block.hole == 8 && block.type > 0) || (block.hole != 8 && block.hole > 0))
               {
                  e.Graphics.RotateTransform(45);
                  e.Graphics.TranslateTransform(pixelX(block.x), pixelY(block.z)-6, MatrixOrder.Append);
               }
               else
               {
                  e.Graphics.TranslateTransform(pixelX(block.x)-4, pixelY(block.z)-4, MatrixOrder.Append);
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
               e.Graphics.DrawRectangle(buildingPen, pixelX(b.x) - 5, pixelY(b.z) - 5, 9, 9);
            }

            if (selectedItem != null)
            {
               x = pixelX(selectedItem.x);
               y = pixelY(selectedItem.z);
               const int selOuter = 10;
               const int selInner = 4;
               e.Graphics.DrawLine(selectedPen, x - selOuter, y - selOuter, x - selInner, y - selInner);
               e.Graphics.DrawLine(selectedPen, x + selOuter - 1, y - selOuter, x + selInner - 1, y - selInner);
               e.Graphics.DrawLine(selectedPen, x + selOuter - 1, y + selOuter - 1, x + selInner - 1, y + selInner - 1);
               e.Graphics.DrawLine(selectedPen, x - selOuter, y + selOuter - 1, x - selInner, y + selInner - 1);
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
         switch (Mode)
         {
            case MouseMode.Move:
               if (level != null)
               {
                  Int16 x = (Int16)levelX(e.X);
                  Int16 z = (Int16)levelZ(e.Y);
                  string text = x + "," + z + " ( " + x.ToString("X4") + "," + z.ToString("X4") + " )";
                  OnPositionEvent(new PositionEventArgs(text));
                  if (dragItem != null)
                  {
                     dragItem.x = x;
                     dragItem.z = z;
                     OnItemMovedEvent(new ItemMovedEventArgs(dragItem));
                     Invalidate();
                  }
               }
               break;
            case MouseMode.Add:
               break;
         }
      }

      private void BlastCorpsViewer_MouseDown(object sender, MouseEventArgs e)
      {
         if (e.Button == System.Windows.Forms.MouseButtons.Left)
         {
            switch (Mode)
            {
               case MouseMode.Select:
                  BlastCorpsItem item = FindNearbyItem(e.X, e.Y);
                  if (item != selectedItem)
                  {
                     selectedItem = item;
                     OnSelectionChangedEvent(new SelectionChangedEventArgs(selectedItem, false, false));
                     Invalidate();
                  }
                  break;
               case MouseMode.Move:
                  dragItem = FindNearbyItem(e.X, e.Y);
                  if (dragItem != selectedItem)
                  {
                     selectedItem = dragItem;
                     OnSelectionChangedEvent(new SelectionChangedEventArgs(selectedItem, false, false));
                     Invalidate();
                  }
                  break;
               case MouseMode.Add:
                  break;
            }
         }
      }

      private void BlastCorpsViewer_MouseUp(object sender, MouseEventArgs e)
      {
         if (level != null && e.Button == System.Windows.Forms.MouseButtons.Left)
         {
            switch (Mode)
            {
               case MouseMode.Move:
                  if (dragItem != null)
                  {
                     selectedItem = dragItem;
                     OnSelectionChangedEvent(new SelectionChangedEventArgs(selectedItem, false, false));
                     dragItem = null;
                     Invalidate();
                  }
                  break;
               case MouseMode.Add:
                  Int16 x = (Int16)levelX(e.X);
                  Int16 z = (Int16)levelZ(e.Y);
                  if (AddType == typeof(AmmoBox))
                  {
                     AmmoBox box = new AmmoBox(x, level.carrier.y, z, 0);
                     selectedItem = box;
                     level.ammoBoxes.Add(box);
                  }
                  else if (AddType == typeof(CommPoint))
                  {
                     CommPoint comm = new CommPoint(x, level.carrier.y, z, 0);
                     selectedItem = comm;
                     level.commPoints.Add(comm);
                  }
                  else if (AddType == typeof(RDU))
                  {
                     RDU rdu = new RDU(x, level.carrier.y, z);
                     selectedItem = rdu;
                     level.rdus.Add(rdu);
                  }
                  else if (AddType == typeof(TNTCrate))
                  {
                     TNTCrate tnt = new TNTCrate(x, level.carrier.y, z, 0, 0, 0, 0);
                     selectedItem = tnt;
                     level.tntCrates.Add(tnt);
                  }
                  else if (AddType == typeof(SquareBlock))
                  {
                     SquareBlock block = new SquareBlock(x, level.carrier.y, z, 0, 0);
                     selectedItem = block;
                     level.squareBlocks.Add(block);
                  }
                  else if (AddType == typeof(Vehicle))
                  {
                     Vehicle vehicle = new Vehicle(0, x, level.carrier.y, z, 0);
                     selectedItem = vehicle;
                     level.vehicles.Add(vehicle);
                  }
                  else if (AddType == typeof(Building))
                  {
                     Building building = new Building(x, level.carrier.y, z, 0, 0, 0, 0, 0);
                     selectedItem = building;
                     level.buildings.Add(building);
                  }
                  OnSelectionChangedEvent(new SelectionChangedEventArgs(selectedItem, true, false));
                  Invalidate();
                  break;
            }
         }
      }

      private void BlastCorpsViewer_KeyDown(object sender, KeyEventArgs e)
      {
         if (selectedItem != null)
         {
            if (e.KeyCode == Keys.Delete)
            {
               OnSelectionChangedEvent(new SelectionChangedEventArgs(selectedItem, false, true));
               selectedItem = null;
               Invalidate();
            }
         }
      }
   }

   // Event Handlers
   public delegate void SelectionChangedEventHandler(object sender, SelectionChangedEventArgs e);
   public class SelectionChangedEventArgs : EventArgs
   {
      public BlastCorpsItem SelectedItem { get; set; }
      public bool IsAdded { get; set; }
      public bool IsDeleted { get; set; }
      public SelectionChangedEventArgs(BlastCorpsItem item, bool added, bool deleted)
      {
         this.SelectedItem = item;
         this.IsAdded = added;
         this.IsDeleted = deleted;
      }
   }

   public delegate void ItemMovedEventHandler(object sender, ItemMovedEventArgs e);
   public class ItemMovedEventArgs : EventArgs
   {
      public BlastCorpsItem SelectedItem { get; set; }
      public ItemMovedEventArgs(BlastCorpsItem item)
      {
         this.SelectedItem = item;
      }
   }

   public delegate void PositionEventHandler(object sender, PositionEventArgs e);
   public class PositionEventArgs : EventArgs
   {
      public string Position { get; set; }
      public PositionEventArgs(string position)
      {
         this.Position = position;
      }
   }

   public enum MouseMode { Select, Move, Add };
}
