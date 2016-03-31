using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlastCorpsEditor
{
   public enum ExportType { Terrain30, Collision24, Collision6C, Collision70, DisplayList };

   public partial class ExportDialog : Form
   {
      public string FileName { get; internal set; }
      public float ScaleFactor { get; internal set; }
      public ExportType DataType { get; internal set; }

      public ExportDialog()
      {
         InitializeComponent();
         ScaleFactor = (float)numericScale.Value;
         comboBoxExport.SelectedIndex = 0;
      }

      private void buttonChoose_Click(object sender, EventArgs e)
      {
         SaveFileDialog saveFileDialog = new SaveFileDialog();
         saveFileDialog.Filter = "Wavefront OBJ(*.obj)|*.obj";
         saveFileDialog.Title = "Export Blast Corps Level Terrain";
         saveFileDialog.ShowDialog();

         if (saveFileDialog.FileName != "")
         {
            FileName = saveFileDialog.FileName;
            textBoxFilename.Text = FileName;
            buttonExport.Enabled = true;
         }
      }

      private void comboBoxExport_SelectedIndexChanged(object sender, EventArgs e)
      {
         // TODO: avoid dual maintenance
         switch (comboBoxExport.SelectedIndex)
         {
            case 0: DataType = ExportType.Terrain30; break;
            case 1: DataType = ExportType.Collision24; break;
            case 2: DataType = ExportType.Collision6C; break;
            case 3: DataType = ExportType.Collision70; break;
            case 4: DataType = ExportType.DisplayList; break;
         }
      }
   }
}
