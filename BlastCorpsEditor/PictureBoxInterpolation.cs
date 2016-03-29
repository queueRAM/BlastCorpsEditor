using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace BlastCorpsEditor
{
   /// <summary>
   /// Inherits from PictureBox; adds Interpolation Mode Setting
   /// </summary>
   public class PictureBoxInterpolation : PictureBox
   {
      public InterpolationMode InterpolationMode { get; set; }

      protected override void OnPaint(PaintEventArgs paintEventArgs)
      {
         paintEventArgs.Graphics.InterpolationMode = InterpolationMode;
         base.OnPaint(paintEventArgs);
      }
   }
}
