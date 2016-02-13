using System;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;

namespace BlastCorpsEditor
{
   public partial class BlastCorpsEditorForm : Form
   {
      private BlastCorpsRom rom = null;
      private int levelId = -1;
      private BlastCorpsLevel level;
      private BindingSource ammoSource = new BindingSource();
      private BindingSource commSource = new BindingSource();
      private BindingSource tntSource = new BindingSource();
      private BindingSource rduSource = new BindingSource();
      private BindingSource blockSource = new BindingSource();
      private BindingSource vehicleSource = new BindingSource();
      private BindingSource buildingSource = new BindingSource();

      private BlastCorpsItem itemSel;

      public BlastCorpsEditorForm()
      {
         InitializeComponent();
         comboBoxLevel.DataSource = BlastCorpsRom.levelMeta;
         blastCorpsViewer.SetStatusLabel(statusStripMessage);
      }

      private void readData(byte[] levelData, byte[] displayList)
      {
         level = BlastCorpsLevel.decodeLevel(levelData, displayList);

         listBoxHeader.Items.Clear();
         for (int i = 0; i < level.header.u16s.Length; i++)
         {
            listBoxHeader.Items.Add((2 * i).ToString("X2") + ": " + level.header.u16s[i].ToString("X4"));
         }
         for (int i = 0; i < level.header.twoVals.Length; i++)
         {
            int offset = 2 * level.header.u16s.Length + 4 * i;
            listBoxHeader.Items.Add(offset.ToString("X2") + ": " + level.header.twoVals[i]);
         }
         for (int i = 0; i < level.header.offsets.Length; i++)
         {
            int offset = 2 * level.header.u16s.Length + 4 * level.header.twoVals.Length + 4 * i;
            listBoxHeader.Items.Add(offset.ToString("X2") + ": " + level.header.offsets[i].ToString("X8"));
         }

         ammoSource.DataSource = level.ammoBoxes;
         commSource.DataSource = level.commPoints;
         rduSource.DataSource = level.rdus;
         tntSource.DataSource = level.tntCrates;
         blockSource.DataSource = level.squareBlocks;
         vehicleSource.DataSource = level.vehicles;
         buildingSource.DataSource = level.buildings;
         listBoxAmmo.DataSource = ammoSource;
         listBoxCommPt.DataSource = commSource;
         listBoxRdu.DataSource = rduSource;
         listBoxTnt.DataSource = tntSource;
         listBoxBlocks.DataSource = blockSource;
         listBoxVehicles.DataSource = vehicleSource;
         listBoxBuildings.DataSource = buildingSource;
         numericCarrierX.Value = level.carrier.x;
         numericCarrierY.Value = level.carrier.y;
         numericCarrierZ.Value = level.carrier.z;
         numericCarrierBearing.Value = level.carrier.heading;
         numericCarrierSpeed.Value = level.carrier.speed;
         numericCarrierDistance.Value = level.carrier.distance;

         statusStripMessage.Text = level.bounds.ToString();

         blastCorpsViewer.ShowGridLines = gridLinesToolStripMenuItem.Checked;
         blastCorpsViewer.ShowBoundingBoxes40 = boundingBoxes0x40ToolStripMenuItem.Checked;
         blastCorpsViewer.ShowBoundingBoxes44 = boundingBoxes0x44ToolStripMenuItem.Checked;
         blastCorpsViewer.SetLevel(level);

         comboBoxLevel.Enabled = true;
         saveToolStripMenuItem.Enabled = true;
         exportToolStripMenuItem.Enabled = true;
      }

      private void SetSelectedItem(BlastCorpsItem item)
      {
         if (itemSel != null)
         {
            itemSel.selected = false;
         }
         item.selected = true;
         itemSel = item;
         blastCorpsViewer.Invalidate();
      }

      private void comboBoxLevel_SelectedIndexChanged(object sender, EventArgs e)
      {
         BlastCorpsLevelMeta levelMeta = (BlastCorpsLevelMeta)comboBoxLevel.SelectedItem;
         if (rom != null && levelMeta != null)
         {
            // write current level out to rom structure first
            if (level != null)
            {
               rom.UpdateLevel(levelId, level.ToBytes(), level.displayList);
            }
            levelId = levelMeta.id;
            byte[] levelData = rom.GetLevelData(levelId);
            byte[] displayList = rom.GetDisplayList(levelId);
            readData(levelData, displayList);
         }
      }

      // Ammo boxes
      private void listBoxAmmo_SelectedIndexChanged(object sender, EventArgs e)
      {
         AmmoBox ammo = (AmmoBox)listBoxAmmo.SelectedItem;
         if (ammo != null)
         {
            SetSelectedItem(ammo);
            numericAmmoX.Value = ammo.x;
            numericAmmoY.Value = ammo.y;
            numericAmmoZ.Value = ammo.z;
            comboBoxAmmo.SelectedIndex = ammo.type;
         }
      }

      private void numericAmmoX_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is AmmoBox)
         {
            itemSel.x = (Int16)numericAmmoX.Value;
            ammoSource.ResetBindings(false);
            blastCorpsViewer.Invalidate();
         }
      }

      private void numericAmmoY_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is AmmoBox)
         {
            itemSel.y = (Int16)numericAmmoY.Value;
            ammoSource.ResetBindings(false);
            blastCorpsViewer.Invalidate();
         }
      }

      private void numericAmmoZ_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is AmmoBox)
         {
            itemSel.z = (Int16)numericAmmoZ.Value;
            ammoSource.ResetBindings(false);
            blastCorpsViewer.Invalidate();
         }
      }

      private void comboBoxAmmo_SelectedIndexChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is AmmoBox)
         {
            AmmoBox ammo = (AmmoBox)itemSel;
            ammo.type = (UInt16)comboBoxAmmo.SelectedIndex;
            ammoSource.ResetBindings(false);
            blastCorpsViewer.Invalidate();
         }
      }

      // Communication point
      private void listBoxCommPt_SelectedIndexChanged(object sender, EventArgs e)
      {
         CommPoint comm = (CommPoint)listBoxCommPt.SelectedItem;
         if (comm != null)
         {
            SetSelectedItem(comm);
            numericCommPtX.Value = comm.x;
            numericCommPtY.Value = comm.y;
            numericCommPtZ.Value = comm.z;
            blastCorpsViewer.Invalidate();
         }
      }

      private void numericCommPtX_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is CommPoint)
         {
            itemSel.x = (Int16)numericCommPtX.Value;
            commSource.ResetBindings(false);
            blastCorpsViewer.Invalidate();
         }
      }

      private void numericCommPtY_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is CommPoint)
         {
            itemSel.y = (Int16)numericCommPtY.Value;
            commSource.ResetBindings(false);
            blastCorpsViewer.Invalidate();
         }
      }

      private void numericCommPtZ_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is CommPoint)
         {
            itemSel.z = (Int16)numericCommPtZ.Value;
            commSource.ResetBindings(false);
            blastCorpsViewer.Invalidate();
         }
      }

      // RDUs
      private void listBoxRdu_SelectedIndexChanged(object sender, EventArgs e)
      {
         RDU rdu = (RDU)listBoxRdu.SelectedItem;
         if (rdu != null)
         {
            SetSelectedItem(rdu);
            numericRduX.Value = rdu.x;
            numericRduY.Value = rdu.y;
            numericRduZ.Value = rdu.z;
         }
      }

      private void numericRduX_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is RDU)
         {
            itemSel.x = (Int16)numericRduX.Value;
            rduSource.ResetBindings(false);
            blastCorpsViewer.Invalidate();
         }
      }

      private void numericRduY_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is RDU)
         {
            itemSel.y = (Int16)numericRduY.Value;
            rduSource.ResetBindings(false);
            blastCorpsViewer.Invalidate();
         }
      }

      private void numericRduZ_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is RDU)
         {
            itemSel.z = (Int16)numericRduZ.Value;
            rduSource.ResetBindings(false);
            blastCorpsViewer.Invalidate();
         }
      }

      // TNT crates
      private void listBoxTnt_SelectedIndexChanged(object sender, EventArgs e)
      {
         TNTCrate tnt = (TNTCrate)listBoxTnt.SelectedItem;
         if (tnt != null)
         {
            SetSelectedItem(tnt);
            numericTntX.Value = tnt.x;
            numericTntY.Value = tnt.y;
            numericTntZ.Value = tnt.z;
            numericTntB6.Value = tnt.b6;
            numericTntTimer.Value = tnt.timer;
            numericTntH8.Value = tnt.h8;
            numericTntHA.Value = tnt.hA;
         }
      }

      private void numericTntX_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is TNTCrate)
         {
            itemSel.x = (Int16)numericTntX.Value;
            tntSource.ResetBindings(false);
            blastCorpsViewer.Invalidate();
         }
      }

      private void numericTntY_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is TNTCrate)
         {
            itemSel.y = (Int16)numericTntY.Value;
            tntSource.ResetBindings(false);
            blastCorpsViewer.Invalidate();
         }
      }

      private void numericTntZ_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is TNTCrate)
         {
            itemSel.z = (Int16)numericTntZ.Value;
            tntSource.ResetBindings(false);
            blastCorpsViewer.Invalidate();
         }
      }

      private void numericTntB6_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is TNTCrate)
         {
            TNTCrate tnt = (TNTCrate)itemSel;
            tnt.b6 = (byte)numericTntB6.Value;
            tntSource.ResetBindings(false);
            blastCorpsViewer.Invalidate();
         }
      }

      private void numericTntTimer_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is TNTCrate)
         {
            TNTCrate tnt = (TNTCrate)itemSel;
            tnt.timer = (byte)numericTntTimer.Value;
            tntSource.ResetBindings(false);
            blastCorpsViewer.Invalidate();
         }
      }

      private void numericTntH8_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is TNTCrate)
         {
            TNTCrate tnt = (TNTCrate)itemSel;
            tnt.h8 = (Int16)numericTntH8.Value;
            tntSource.ResetBindings(false);
            blastCorpsViewer.Invalidate();
         }
      }

      private void numericTntHA_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is TNTCrate)
         {
            TNTCrate tnt = (TNTCrate)itemSel;
            tnt.hA = (Int16)numericTntHA.Value;
            tntSource.ResetBindings(false);
            blastCorpsViewer.Invalidate();
         }
      }

      // Blocks
      private void listBoxBlocks_SelectedIndexChanged(object sender, EventArgs e)
      {
         SquareBlock block = (SquareBlock)listBoxBlocks.SelectedItem;
         if (block != null)
         {
            SetSelectedItem(block);
            numericBlockX.Value = block.x;
            numericBlockY.Value = block.y;
            numericBlockZ.Value = block.z;
            numericBlockT3.Value = block.extra;
            switch (block.hole)
            {
               case 0:
               case 1:
               case 2:
                  comboBoxBlockT1.SelectedIndex = 0;
                  comboBoxBlockT2.SelectedIndex = block.hole;
                  numericBlockT3.Enabled = false;
                  break;
               case 8:
                  comboBoxBlockT1.SelectedIndex = block.type;
                  comboBoxBlockT2.SelectedIndex = 3;
                  numericBlockT3.Enabled = true;
                  break;
            }
            blastCorpsViewer.Invalidate();
         }
      }

      private void numericBlockX_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is SquareBlock)
         {
            itemSel.x = (Int16)numericBlockX.Value;
            blockSource.ResetBindings(false);
            blastCorpsViewer.Invalidate();
         }
      }

      private void numericBlockY_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is SquareBlock)
         {
            itemSel.y = (Int16)numericBlockY.Value;
            blockSource.ResetBindings(false);
            blastCorpsViewer.Invalidate();
         }
      }

      private void numericBlockZ_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is SquareBlock)
         {
            itemSel.z = (Int16)numericBlockZ.Value;
            blockSource.ResetBindings(false);
            blastCorpsViewer.Invalidate();
         }
      }

      private void comboBoxBlockT1_SelectedIndexChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is SquareBlock)
         {
            SquareBlock block = (SquareBlock)itemSel;
            block.type = (byte)comboBoxBlockT1.SelectedIndex;
            blockSource.ResetBindings(false);
            blastCorpsViewer.Invalidate();
         }
      }

      private void comboBoxBlockT2_SelectedIndexChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is SquareBlock)
         {
            SquareBlock block = (SquareBlock)itemSel;
            switch (comboBoxBlockT2.SelectedIndex)
            {
               case 0: block.hole = 0; break;
               case 1: block.hole = 1; break;
               case 2: block.hole = 2; break;
               case 3: block.hole = 8; break;
                       // TODO: toggle T3 based on this value
            }
            blockSource.ResetBindings(false);
            blastCorpsViewer.Invalidate();
         }
      }

      private void numericBlockT3_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is SquareBlock)
         {
            SquareBlock block = (SquareBlock)itemSel;
            block.extra = (UInt16)numericBlockT3.Value;
            blockSource.ResetBindings(false);
            blastCorpsViewer.Invalidate();
         }
      }

      // Vehicles
      private void listBoxVehicles_SelectedIndexChanged(object sender, EventArgs e)
      {
         Vehicle veh = (Vehicle)listBoxVehicles.SelectedItem;
         if (veh != null)
         {
            SetSelectedItem(veh);
            if (veh.type < comboBoxVehicle.Items.Count)
            {
               comboBoxVehicle.SelectedIndex = veh.type;
            }
            numericVehicleX.Value = veh.x;
            numericVehicleY.Value = veh.y;
            numericVehicleZ.Value = veh.z;
            numericVehicleH.Value = veh.heading;
         }
      }


      private void comboBoxVehicle_SelectedIndexChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is Vehicle)
         {
            Vehicle veh = (Vehicle)itemSel;
            veh.type = (byte)comboBoxVehicle.SelectedIndex;
            vehicleSource.ResetBindings(false);
            blastCorpsViewer.Invalidate();
         }
      }

      private void numericVehicleX_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is Vehicle)
         {
            itemSel.x = (Int16)numericVehicleX.Value;
            vehicleSource.ResetBindings(false);
            blastCorpsViewer.Invalidate();
         }
      }

      private void numericVehicleY_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is Vehicle)
         {
            itemSel.y = (Int16)numericVehicleY.Value;
            vehicleSource.ResetBindings(false);
            blastCorpsViewer.Invalidate();
         }
      }

      private void numericVehicleZ_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is Vehicle)
         {
            itemSel.z = (Int16)numericVehicleZ.Value;
            vehicleSource.ResetBindings(false);
            blastCorpsViewer.Invalidate();
         }
      }

      private void numericVehicleH_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is Vehicle)
         {
            Vehicle veh = (Vehicle)itemSel;
            veh.heading = (Int16)numericVehicleH.Value;
            vehicleSource.ResetBindings(false);
            blastCorpsViewer.Invalidate();
         }
      }

      // Buildings
      private void listBoxBuildings_SelectedIndexChanged(object sender, EventArgs e)
      {
         Building building = (Building)listBoxBuildings.SelectedItem;
         if (building != null)
         {
            SetSelectedItem(building);
            numericBuildingX.Value = building.x;
            numericBuildingY.Value = building.y;
            numericBuildingZ.Value = building.z;
            numericBuildingT.Value = building.type;
            numericBuildingB8.Value = building.b8;
            numericBuildingB9.Value = building.b9;
            numericBuildingHA.Value = building.hA;
            numericBuildingHC.Value = building.hC;
         }
      }

      private void numericBuildingX_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is Building)
         {
            itemSel.x = (Int16)numericBuildingX.Value;
            buildingSource.ResetBindings(false);
            blastCorpsViewer.Invalidate();
         }
      }

      private void numericBuildingY_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is Building)
         {
            itemSel.y = (Int16)numericBuildingY.Value;
            buildingSource.ResetBindings(false);
            blastCorpsViewer.Invalidate();
         }
      }

      private void numericBuildingZ_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is Building)
         {
            itemSel.z = (Int16)numericBuildingZ.Value;
            buildingSource.ResetBindings(false);
            blastCorpsViewer.Invalidate();
         }
      }

      private void numericBuildingT_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is Building)
         {
            Building b = (Building)itemSel;
            b.type = (UInt16)numericBuildingT.Value;
            buildingSource.ResetBindings(false);
            blastCorpsViewer.Invalidate();
         }
      }

      private void numericBuildingB8_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is Building)
         {
            Building b = (Building)itemSel;
            b.b8 = (byte)numericBuildingB8.Value;
            buildingSource.ResetBindings(false);
            blastCorpsViewer.Invalidate();
         }
      }

      private void numericBuildingB9_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is Building)
         {
            Building b = (Building)itemSel;
            b.b9 = (byte)numericBuildingB9.Value;
            buildingSource.ResetBindings(false);
            blastCorpsViewer.Invalidate();
         }
      }

      private void numericBuildingHA_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is Building)
         {
            Building b = (Building)itemSel;
            b.hA = (UInt16)numericBuildingHA.Value;
            buildingSource.ResetBindings(false);
            blastCorpsViewer.Invalidate();
         }
      }

      private void numericBuildingHC_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is Building)
         {
            Building b = (Building)itemSel;
            b.hC = (UInt16)numericBuildingHC.Value;
            buildingSource.ResetBindings(false);
            blastCorpsViewer.Invalidate();
         }
      }

      // File menu
      private void openToolStripMenuItem_Click(object sender, EventArgs e)
      {
         OpenFileDialog openFileDialog1 = new OpenFileDialog();

         openFileDialog1.Filter = "N64 ROM (.z64)|*.z64|All Files (*.*)|*.*";
         openFileDialog1.FilterIndex = 1;

         DialogResult dresult = openFileDialog1.ShowDialog();

         if (dresult == DialogResult.OK)
         {
            rom = new BlastCorpsRom();
            if (rom.LoadRom(openFileDialog1.FileName))
            {
               if (rom.type == BlastCorpsRom.RomType.Vanilla && (rom.region != BlastCorpsRom.Region.US || rom.version != BlastCorpsRom.Version.Ver1p1))
               {
                  MessageBox.Show("Error, this tool only works with Blast Corps (U) (V1.1) ROMs currently.");
               }
               else
               {
                  if (rom.type == BlastCorpsRom.RomType.Vanilla)
                  {
                     MessageBox.Show("Vanilla ROM detected. The ROM will be extended to allow editing and when saving, you will be prompted with a \"Save As...\" dialog so this ROM is not overwritten.");
                     rom.ExtendRom();
                     statusStripFile.ForeColor = Color.Blue;
                     statusStripFile.Text = "Vanilla ROM";
                  }
                  else
                  {
                     statusStripFile.ForeColor = Color.Black;
                     statusStripFile.Text = openFileDialog1.FileName;
                  }
                  BlastCorpsLevelMeta levelMeta = (BlastCorpsLevelMeta)comboBoxLevel.SelectedItem;
                  if (levelMeta != null)
                  {
                     levelId = levelMeta.id;
                     byte[] levelData = rom.GetLevelData(levelId);
                     byte[] displayList = rom.GetDisplayList(levelId);
                     readData(levelData, displayList);
                  }
               }
            }
            else
            {
               statusStripFile.ForeColor = Color.Red;
               statusStripFile.Text = "Error loading " + openFileDialog1.FileName;
            }
         }
      }

      private void saveToolStripMenuItem_Click(object sender, EventArgs e)
      {
         if (rom != null)
         {
            // opening a vanilla ROM will not record the path, forcing a save as... dialog here
            if (rom.savePath == null)
            {
               SaveFileDialog saveFileDialog = new SaveFileDialog();
               saveFileDialog.Filter = "N64 ROM (.z64)|*.z64";
               saveFileDialog.ShowDialog();

               if (saveFileDialog.FileName != "")
               {
                  rom.savePath = saveFileDialog.FileName;
                  statusStripFile.Text = saveFileDialog.FileName;
                  statusStripFile.ForeColor = Color.Black;
               }
            }
            if (rom.savePath != null)
            {
               if (level != null)
               {
                  rom.UpdateLevel(levelId, level.ToBytes(), level.displayList);
               }
               rom.SaveRom(rom.savePath);
            }
         }
      }

      private void exportToolStripMenuItem_Click(object sender, EventArgs e)
      {
         SaveFileDialog saveFileDialog = new SaveFileDialog();
         saveFileDialog.Filter = "Blast Corps RAW Level|*.raw";
         saveFileDialog.Title = "Export Blast Corps Level";
         saveFileDialog.ShowDialog();

         if (saveFileDialog.FileName != "")
         {
            System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog.OpenFile();
            byte[] data = level.ToBytes();
            fs.Write(data, 0, data.Length);
            fs.Close();
         }
      }

      private void exitToolStripMenuItem_Click(object sender, EventArgs e)
      {
         // TODO: confirm save
         this.Close();
      }

      // View menu
      private void gridLinesToolStripMenuItem_Click(object sender, EventArgs e)
      {
         blastCorpsViewer.ShowGridLines = gridLinesToolStripMenuItem.Checked;
      }

      private void boundingBoxes0x40ToolStripMenuItem_Click(object sender, EventArgs e)
      {
         blastCorpsViewer.ShowBoundingBoxes40 = boundingBoxes0x40ToolStripMenuItem.Checked;
      }

      private void boundingBoxes0x44ToolStripMenuItem_Click(object sender, EventArgs e)
      {
         blastCorpsViewer.ShowBoundingBoxes44 = boundingBoxes0x44ToolStripMenuItem.Checked;
      }

      // Help menu
      private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
      {
         var versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
         var version = versionInfo.ProductVersion;
         var copyright = versionInfo.LegalCopyright;
         var assembly = Assembly.GetExecutingAssembly();
         var appName = "Blast Corps Editor";
         MessageBox.Show(appName + " v" + version + "\n" + copyright, appName, MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
   }
}
