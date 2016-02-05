using System;
using System.Drawing;
using System.Windows.Forms;

namespace BlastCorpsEditor
{
   public partial class BlastCorpsEditorForm : Form
   {
      private List<BlastCorpsLevelMeta> levels;
      private byte[] levelData;
      private BlastCorpsLevel level;
      BindingSource rduSource = new BindingSource();
      private string levelPath = @"C:\tools\blast_corps\levels";

      private RDU rduSel;

      public BlastCorpsEditorForm()
      {
         InitializeComponent();
         InitializeLevelMeta();
         comboBoxLevel.DataSource = levels;
         blastCorpsViewer.SetStatusLabel(statusStripMessage);
      }

      private void InitializeLevelMeta()
      {
         levels = new List<BlastCorpsLevelMeta>()
         {
            new BlastCorpsLevelMeta{ id =  0, start = 0x4ACC10, end = 0x4B8960, filename = "chimp.raw",   name = "Simian Acres" },
                new BlastCorpsLevelMeta{ id =  1, start = 0x4A5660, end = 0x4ACC10, filename = "lagp.raw",    name = "Angel City" },
                new BlastCorpsLevelMeta{ id =  2, start = 0x4B8960, end = 0x4BFD60, filename = "valley.raw",  name = "Outland Farm" },
                new BlastCorpsLevelMeta{ id =  3, start = 0x4BFD60, end = 0x4C3AC0, filename = "fact.raw",    name = "Blackridge Works" },
                new BlastCorpsLevelMeta{ id =  4, start = 0x4C3AC0, end = 0x4D5F90, filename = "dip.raw",     name = "Glory Crossing" },
                new BlastCorpsLevelMeta{ id =  5, start = 0x4D5F90, end = 0x4E2F70, filename = "beetle.raw",  name = "Shuttle Gully" },
                new BlastCorpsLevelMeta{ id =  6, start = 0x4E2F70, end = 0x4E4E80, filename = "bonus1.raw",  name = "Salvage Wharf" },
                new BlastCorpsLevelMeta{ id =  7, start = 0x4E4E80, end = 0x4E7C00, filename = "bonus2.raw",  name = "Skyfall" },
                new BlastCorpsLevelMeta{ id =  8, start = 0x4E7C00, end = 0x4E8F70, filename = "bonus3.raw",  name = "Twilight Foundry" },
                new BlastCorpsLevelMeta{ id =  9, start = 0x4E8F70, end = 0x4F5C10, filename = "level9.raw",  name = "Crystal Rift" },
                new BlastCorpsLevelMeta{ id = 10, start = 0x4F5C10, end = 0x500520, filename = "level10.raw", name = "Argent Towers" },
                new BlastCorpsLevelMeta{ id = 11, start = 0x500520, end = 0x507E80, filename = "level11.raw", name = "Skerries" },
                new BlastCorpsLevelMeta{ id = 12, start = 0x507E80, end = 0x511340, filename = "level12.raw", name = "Diamond Sands" },
                new BlastCorpsLevelMeta{ id = 13, start = 0x511340, end = 0x523080, filename = "level13.raw", name = "Ebony Coast" },
                new BlastCorpsLevelMeta{ id = 14, start = 0x523080, end = 0x52CD00, filename = "level14.raw", name = "Oyster Harbor" },
                new BlastCorpsLevelMeta{ id = 15, start = 0x52CD00, end = 0x532700, filename = "level15.raw", name = "Carrick Point" },
                new BlastCorpsLevelMeta{ id = 16, start = 0x532700, end = 0x53E9B0, filename = "level16.raw", name = "Havoc District" },
                new BlastCorpsLevelMeta{ id = 17, start = 0x53E9B0, end = 0x54A820, filename = "level17.raw", name = "Ironstone Mine" },
                new BlastCorpsLevelMeta{ id = 18, start = 0x54A820, end = 0x552DE0, filename = "level18.raw", name = "Beeton Tracks" },
                new BlastCorpsLevelMeta{ id = 19, start = 0x552DE0, end = 0x555000, filename = "level19.raw", name = "J-Bomb" },
                new BlastCorpsLevelMeta{ id = 20, start = 0x555000, end = 0x560E90, filename = "level20.raw", name = "Jade Plateau" },
                new BlastCorpsLevelMeta{ id = 21, start = 0x560E90, end = 0x5652D0, filename = "level21.raw", name = "Marine Quarter" },
                new BlastCorpsLevelMeta{ id = 22, start = 0x5652D0, end = 0x56F3F0, filename = "level22.raw", name = "Cooter Creek" },
                new BlastCorpsLevelMeta{ id = 23, start = 0x56F3F0, end = 0x5721E0, filename = "level23.raw", name = "Gibbon's Gate" },
                new BlastCorpsLevelMeta{ id = 24, start = 0x5721E0, end = 0x5736E0, filename = "level24.raw", name = "Baboon Catacomb" },
                new BlastCorpsLevelMeta{ id = 25, start = 0x5736E0, end = 0x57A2C0, filename = "level25.raw", name = "Sleek Streets" },
                new BlastCorpsLevelMeta{ id = 26, start = 0x57A2C0, end = 0x580B60, filename = "level26.raw", name = "Obsidian Mile" },
                new BlastCorpsLevelMeta{ id = 27, start = 0x580B60, end = 0x588CE0, filename = "level27.raw", name = "Corvine Bluff" },
                new BlastCorpsLevelMeta{ id = 28, start = 0x588CE0, end = 0x58BE80, filename = "level28.raw", name = "Sideswipe" },
                new BlastCorpsLevelMeta{ id = 29, start = 0x58BE80, end = 0x597B80, filename = "level29.raw", name = "Echo Marches" },
                new BlastCorpsLevelMeta{ id = 30, start = 0x597B80, end = 0x59B7D0, filename = "level30.raw", name = "Kipling Plant" },
                new BlastCorpsLevelMeta{ id = 31, start = 0x59B7D0, end = 0x5A5840, filename = "level31.raw", name = "Falchion Field" },
                new BlastCorpsLevelMeta{ id = 32, start = 0x5A5840, end = 0x5B0B10, filename = "level32.raw", name = "Morgan Hall" },
                new BlastCorpsLevelMeta{ id = 33, start = 0x5B0B10, end = 0x5B5A30, filename = "level33.raw", name = "Tempest City" },
                new BlastCorpsLevelMeta{ id = 34, start = 0x5B5A30, end = 0x5B8BB0, filename = "level34.raw", name = "Orion Plaza" },
                new BlastCorpsLevelMeta{ id = 35, start = 0x5B8BB0, end = 0x5C4C80, filename = "level35.raw", name = "Glander's Ranch" },
                new BlastCorpsLevelMeta{ id = 36, start = 0x5C4C80, end = 0x5CA9C0, filename = "level36.raw", name = "Dagger Pass" },
                new BlastCorpsLevelMeta{ id = 37, start = 0x5CA9C0, end = 0x5CCF50, filename = "level37.raw", name = "Geode Square" },
                new BlastCorpsLevelMeta{ id = 38, start = 0x5CCF50, end = 0x5D1060, filename = "level38.raw", name = "Shuttle Island" },
                new BlastCorpsLevelMeta{ id = 39, start = 0x5D1060, end = 0x5DC830, filename = "level39.raw", name = "Mica Park" },
                new BlastCorpsLevelMeta{ id = 40, start = 0x5DC830, end = 0x5E6EE0, filename = "level40.raw", name = "Moon" },
                new BlastCorpsLevelMeta{ id = 41, start = 0x5E6EE0, end = 0x5EC800, filename = "level41.raw", name = "Cobalt Quarry" },
                new BlastCorpsLevelMeta{ id = 42, start = 0x5EC800, end = 0x5F3A80, filename = "level42.raw", name = "Moraine Chase" },
                new BlastCorpsLevelMeta{ id = 43, start = 0x5F3A80, end = 0x6014B0, filename = "level43.raw", name = "Mercury" },
                new BlastCorpsLevelMeta{ id = 44, start = 0x6014B0, end = 0x60A710, filename = "level44.raw", name = "Venus" },
                new BlastCorpsLevelMeta{ id = 45, start = 0x60A710, end = 0x613AA0, filename = "level45.raw", name = "Mars" },
                new BlastCorpsLevelMeta{ id = 46, start = 0x613AA0, end = 0x61DD70, filename = "level46.raw", name = "Neptune" },
                new BlastCorpsLevelMeta{ id = 47, start = 0x61DD70, end = 0x621AF0, filename = "level47.raw", name = "CMO Intro" },
                new BlastCorpsLevelMeta{ id = 48, start = 0x621AF0, end = 0x6269E0, filename = "level48.raw", name = "Silver Junction" },
                new BlastCorpsLevelMeta{ id = 49, start = 0x6269E0, end = 0x630C30, filename = "level49.raw", name = "End Sequence" },
                new BlastCorpsLevelMeta{ id = 50, start = 0x630C30, end = 0x635700, filename = "level50.raw", name = "Shuttle Clear" },
                new BlastCorpsLevelMeta{ id = 51, start = 0x635700, end = 0x63CA10, filename = "level51.raw", name = "Dark Heartland" },
                new BlastCorpsLevelMeta{ id = 52, start = 0x63CA10, end = 0x641F30, filename = "level52.raw", name = "Magma Peak" },
                new BlastCorpsLevelMeta{ id = 53, start = 0x641F30, end = 0x644810, filename = "level53.raw", name = "Thunderfist" },
                new BlastCorpsLevelMeta{ id = 54, start = 0x644810, end = 0x646080, filename = "level54.raw", name = "Saline Watch" },
                new BlastCorpsLevelMeta{ id = 55, start = 0x646080, end = 0x647550, filename = "level55.raw", name = "Backlash" },
                new BlastCorpsLevelMeta{ id = 56, start = 0x647550, end = 0x654FC0, filename = "level56.raw", name = "Bison Ridge" },
                new BlastCorpsLevelMeta{ id = 57, start = 0x654FC0, end = 0x660950, filename = "level57.raw", name = "Ember Hamlet" },
                new BlastCorpsLevelMeta{ id = 58, start = 0x660950, end = 0x665F80, filename = "level58.raw", name = "Cromlech Court" },
                new BlastCorpsLevelMeta{ id = 59, start = 0x665F80, end = 0x66C900, filename = "level59.raw", name = "Lizard Island" },
         };
      }

      private void readData(String filePath)
      {
         if (File.Exists(filePath))
         {
            statusStripFile.ForeColor = Color.Black;
            statusStripFile.Text = filePath;
            levelData = System.IO.File.ReadAllBytes(filePath);
            level = BlastCorpsLevel.decodeLevel(levelData);

            rduSource.DataSource = level.rdus;
            listBoxAmmo.DataSource = level.ammoBoxes;
            listBoxCommPt.DataSource = level.commPoints;
            listBoxRdu.DataSource = rduSource;
            listBoxTnt.DataSource = level.tntCrates;
            listBoxBlocks.DataSource = level.squareBlocks;
            listBoxVehicles.DataSource = level.vehicles;

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
            statusStripMessage.Text = level.bounds.ToString();

            blastCorpsViewer.UseGridLines = gridLinesToolStripMenuItem.Checked;
            blastCorpsViewer.SetLevel(level);
         }
         else
         {
            statusStripFile.ForeColor = Color.Red;
            statusStripFile.Text = "Error opening " + filePath;
         }
      }

      private void openToolStripMenuItem_Click(object sender, EventArgs e)
      {
         // Create an instance of the open file dialog box.
         OpenFileDialog openFileDialog1 = new OpenFileDialog();

         // Set filter options and filter index.
         openFileDialog1.Filter = "Blast Corps Levels (.raw)|*.raw|All Files (*.*)|*.*";
         openFileDialog1.FilterIndex = 1;

         // Call the ShowDialog method to show the dialog box.
         DialogResult dresult = openFileDialog1.ShowDialog();

         // Process input if the user clicked OK.
         if (dresult == DialogResult.OK)
         {
            readData(openFileDialog1.FileName);
            levelPath = Path.GetDirectoryName(openFileDialog1.FileName);
         }
      }

      private void listBoxRdu_SelectedIndexChanged(object sender, EventArgs e)
      {
         RDU rdu = (RDU)listBoxRdu.SelectedItem;
         if (rduSel != null)
         {
            rduSel.selected = false;
         }
         if (rdu != null)
         {
            rdu.selected = true;
            rduSel = rdu;
            numericRduX.Value = rdu.x;
            numericRduY.Value = rdu.y;
            numericRduZ.Value = rdu.z;
            blastCorpsViewer.Invalidate();
         }
      }

      private void listBoxTnt_SelectedIndexChanged(object sender, EventArgs e)
      {
         TNTCrate tnt = (TNTCrate)listBoxTnt.SelectedItem;
         if (tnt != null)
         {
            textBoxTntX.Text  = tnt.x.ToString();
            textBoxTntY.Text  = tnt.y.ToString();
            textBoxTntZ.Text  = tnt.z.ToString();
            textBoxTntB6.Text = tnt.b6.ToString();
            textBoxTntT.Text  = tnt.timer.ToString();
            textBoxTntH8.Text = tnt.h8.ToString();
            textBoxTntHA.Text = tnt.hA.ToString();
         }
      }

      private void exitToolStripMenuItem_Click(object sender, EventArgs e)
      {
         this.Close();
      }

      private void numericRduX_ValueChanged(object sender, EventArgs e)
      {
         if (rduSel != null)
         {
            rduSel.x = (Int16)numericRduX.Value;
            rduSource.ResetBindings(false);
            blastCorpsViewer.Invalidate();
         }
      }

      private void numericRduY_ValueChanged(object sender, EventArgs e)
      {
         if (rduSel != null)
         {
            rduSel.y = (Int16)numericRduY.Value;
            rduSource.ResetBindings(false);
            blastCorpsViewer.Invalidate();
         }
      }

      private void numericRduZ_ValueChanged(object sender, EventArgs e)
      {
         if (rduSel != null)
         {
            rduSel.z = (Int16)numericRduZ.Value;
            rduSource.ResetBindings(false);
            blastCorpsViewer.Invalidate();
         }
      }

      private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
      {
         MessageBox.Show("Blast Corps Editor v0.1 by queueRAM\nCopyright 2016", "Blast Corps Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }

      private void gridLinesToolStripMenuItem_Click(object sender, EventArgs e)
      {
         blastCorpsViewer.UseGridLines = gridLinesToolStripMenuItem.Checked;
      }

      private void comboBoxLevel_SelectedIndexChanged(object sender, EventArgs e)
      {
         BlastCorpsLevelMeta level = (BlastCorpsLevelMeta)comboBoxLevel.SelectedItem;
         if (level != null)
         {
            var filePath = Path.Combine(levelPath, level.filename);
            readData(filePath);
         }
      }

      private void listBoxAmmo_SelectedIndexChanged(object sender, EventArgs e)
      {
         AmmoBox ammo = (AmmoBox)listBoxAmmo.SelectedItem;
         if (ammo != null)
         {
            numericAmmoX.Value = ammo.x;
            numericAmmoY.Value = ammo.y;
            numericAmmoZ.Value = ammo.z;
            comboBoxAmmo.SelectedIndex = ammo.type;
            blastCorpsViewer.Invalidate();
         }
      }
   }
}
