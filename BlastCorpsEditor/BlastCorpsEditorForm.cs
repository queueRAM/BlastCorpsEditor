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

      private BlastCorpsItem itemSel;

      // TreeView
      private const int ICON_AMMO = 2;
      private const int ICON_COMM = 4;
      private const int ICON_RDU = 5;
      private const int ICON_TNT = 6;
      private const int ICON_BLOCK = 7;
      private const int ICON_VEHICLE = 11;
      private const int ICON_BUILDING = 28;
      // top level tree view nodes
      private TreeNode treeNodeCarrier;
      private TreeNode treeNodeAmmo;
      private TreeNode treeNodeCommPt;
      private TreeNode treeNodeRdu;
      private TreeNode treeNodeTnt;
      private TreeNode treeNodeBlock;
      private TreeNode treeNodeVehicle;
      private TreeNode treeNodeBuilding;

      // dynamic controls
      private NumericUpDown[] numericHeaders;
      // X,Y,Z
      private Label labelX, labelY, labelZ;
      private NumericUpDown numericX, numericY, numericZ;
      private Label labelType;
      private Label labelHeading;
      // Ammo
      private ComboBox comboBoxAmmo;
      // Communication Point
      private Label labelCommPtH6;
      private NumericUpDown numericCommPtH6;
      // TNT
      private NumericUpDown numericTntB6, numericTntTimer, numericTntH8, numericTntHA;
      private Label labelTntB6, labelTntTimer, labelTntHA, labelTntH8;
      // Blocks
      private Label labelBlockType1, labelBlockType2, labelBlockCount;
      private ComboBox comboBoxBlockType1, comboBoxBlockType2;
      private NumericUpDown numericBlockCount;
      // Vehicle
      private ComboBox comboBoxVehicle;
      private NumericUpDown numericHeading;
      // Carrier
      private Label labelSpeed, labelDistance;
      private NumericUpDown numericCarrierSpeed;
      private NumericUpDown numericCarrierDistance;
      // Buildings
      private Label labelBuildingCount, labelBuildingB9, labelBuildingMovement;
      private ComboBox comboBoxBuildingType;
      private NumericUpDown numericBuildingCounter;
      private NumericUpDown numericBuildingB9;
      private ComboBox comboBoxBuildingMovement;
      private NumericUpDown numericBuildingSpeed;
      // Dummy control for spacing
      private Label labelDummy;

      public BlastCorpsEditorForm()
      {
         InitializeComponent();
         toolStripComboBoxLevel.ComboBox.DataSource = BlastCorpsRom.levelMeta;
         blastCorpsViewer.PositionEvent += delegate(Object sender, PositionEventArgs e)
         {
            statusStripMessage.Text = e.Position;
         };
         blastCorpsViewer.ItemMovedEvent += delegate(Object sender, ItemMovedEventArgs e)
         {
            updateNode(e.SelectedItem);
            numericX.Value = e.SelectedItem.x;
            numericY.Value = e.SelectedItem.y;
            numericZ.Value = e.SelectedItem.z;
         };
         blastCorpsViewer.SelectionChangedEvent += delegate(Object sender, SelectionChangedEventArgs e)
         {
            // if new object, add it to the tree
            if (e.IsAdded)
            {
               if (e.SelectedItem is AmmoBox)
               {
                  addAmmoNode((AmmoBox)e.SelectedItem);
               }
               else if (e.SelectedItem is CommPoint)
               {
                  addCommPtNode((CommPoint)e.SelectedItem);
               }
               else if (e.SelectedItem is RDU)
               {
                  addRduNode((RDU)e.SelectedItem);
               }
               else if (e.SelectedItem is TNTCrate)
               {
                  addTntNode((TNTCrate)e.SelectedItem);
               }
               else if (e.SelectedItem is SquareBlock)
               {
                  addBlockNode((SquareBlock)e.SelectedItem);
               }
               else if (e.SelectedItem is Vehicle)
               {
                  addVehicleNode((Vehicle)e.SelectedItem);
               }
               else if (e.SelectedItem is Building)
               {
                  addBuildingNode((Building)e.SelectedItem);
               }
            }
            if (e.IsDeleted)
            {
               DeleteItem(e.SelectedItem);
            }
            else
            {
               SetSelectedItem(e.SelectedItem);
            }
         };

         // populate header NumericUpDowns
         numericHeaders = new NumericUpDown[] { 
            numericHeader00,
            numericHeader02,
            numericHeader04,
            numericHeader06,
            numericHeader08,
            numericHeader0A,
            numericHeader0C,
            numericHeader0E,
            numericHeader10,
            numericHeader12,
            numericHeader14,
            numericHeader16 };
         int index = 0;
         foreach (NumericUpDown numeric in numericHeaders)
         {
            numeric.Tag = index++;
            numeric.ValueChanged += new System.EventHandler(this.numericU16_ValueChanged);
         }

         // tree view references for convenience
         treeNodeCarrier = treeViewObjects.Nodes[0];
         treeNodeAmmo = treeViewObjects.Nodes[1];
         treeNodeCommPt = treeViewObjects.Nodes[2];
         treeNodeRdu = treeViewObjects.Nodes[3];
         treeNodeTnt = treeViewObjects.Nodes[4];
         treeNodeBlock = treeViewObjects.Nodes[5];
         treeNodeVehicle = treeViewObjects.Nodes[6];
         treeNodeBuilding = treeViewObjects.Nodes[7];

         // generate controls for the object properties
         generatePropertyControls();

         // handle arguments passed in the command line
         string[] args = Environment.GetCommandLineArgs();
         if (args.Length > 1)
         {
            LoadRom(args[1]);
         }
      }

      private void generatePropertyControls()
      {
         labelX = createLabel("X:");
         labelY = createLabel("Y:");
         labelZ = createLabel("Z:");
         numericX = createNumeric(-32768, 32767, new System.EventHandler(this.numericX_ValueChanged));
         numericY = createNumeric(-32768, 32767, new System.EventHandler(this.numericY_ValueChanged));
         numericZ = createNumeric(-32768, 32767, new System.EventHandler(this.numericZ_ValueChanged));

         labelType = createLabel("Type:");
         comboBoxAmmo = createComboBox(new object[] {
            "0: Ballista missile",
            "1: Sideswipe hydraulic"}, new System.EventHandler(this.comboBoxAmmo_SelectedIndexChanged));

         labelCommPtH6 = createLabel("H6:");
         numericCommPtH6 = createNumeric(0, 65535, new System.EventHandler(this.numericCommPtH6_ValueChanged));

         labelTntB6 = createLabel("U8_6:");
         labelTntTimer = createLabel("Timer:");
         labelTntH8 = createLabel("I16_8:");
         labelTntHA = createLabel("I16_A:");
         numericTntB6 = createNumeric(0, 255, new System.EventHandler(this.numericTntB6_ValueChanged));
         numericTntTimer = createNumeric(0, 255, new System.EventHandler(this.numericTntTimer_ValueChanged));
         numericTntH8 = createNumeric(-32768, 32767, new System.EventHandler(this.numericTntH8_ValueChanged));
         numericTntHA = createNumeric(-32768, 32767, new System.EventHandler(this.numericTntHA_ValueChanged));

         labelBlockType1 = createLabel("Type1:");
         comboBoxBlockType1 = createComboBox(new object[] {
            "0: Normal",
            "1: Diamond (Hole)",
            "2: Diamond (Hole)"}, new System.EventHandler(this.comboBoxBlockType1_SelectedIndexChanged));
         labelBlockType2 = createLabel("Type2:");
         comboBoxBlockType2 = createComboBox(new object[] {
            "0: Normal (Block)",
            "1: Diamond (Block)",
            "2: Diamond (Block)",
            "8: Hole"}, new System.EventHandler(this.comboBoxBlockType2_SelectedIndexChanged));
         labelBlockCount = createLabel("Count:");
         numericBlockCount = createNumeric(0, 65535, new System.EventHandler(this.numericBlockType3_ValueChanged));

         comboBoxVehicle = createComboBox(new object[] {
            "00: Player",
            "01: Sideswipe",
            "02: Thunderfist",
            "03: Skyfall",
            "04: Bulldozer",
            "05: Backlash",
            "06: Crane",
            "07: Train",
            "08: American Dream",
            "09: J-Bomb",
            "10: Ballista",
            "11: barge 0",
            "12: INVALID",
            "13: police",
            "14: A-Team Van",
            "15: Hotrod",
            "16: Cyclone Suit",
            "17: barge 1",
            "18: barge 2"}, new System.EventHandler(this.comboBoxVehicle_SelectedIndexChanged));
         labelHeading = createLabel("Heading:");
         numericHeading = createNumeric(0, 65535, new System.EventHandler(this.numericHeading_ValueChanged));

         labelSpeed = createLabel("Speed:");
         labelDistance = createLabel("Distance:");
         numericCarrierSpeed = createNumeric(0, 255, new System.EventHandler(this.numericCarrierSpeed_ValueChanged));
         numericCarrierDistance = createNumeric(0, 65535, new System.EventHandler(this.numericCarrierDistance_ValueChanged));

         comboBoxBuildingType = createComboBox(new object[] {
            "000: Angel City Fences #1",
            "001: Angel City Petrol Station",
            "002: Angel City Burger Shop",
            "003: Angel City Building with Stairs #1",
            "004: Angel City Shop #1",
            "005: Angel City Shop #1 Fences",
            "006: Angel City Building #1",
            "007: Angel City Building #1 Fences",
            "008: Angel City Building #2",
            "009: Angel City Building #3",
            "010: Angel City Building #3 Fences",
            "011: Angel City Building #4",
            "012: Angel City Building #4 Fences",
            "013: Angel City Shop #1",
            "014: Angel City Shop #1 Fences",
            "015: Angel City Building with Stairs #2",
            "016: Angel City Garage",
            "017: Angel City Building #5",
            "018: Angel City Building #6 (Unused)",
            "019: Angel City Building #7",
            "020: Angel City Building #8",
            "021: Angel City Building with Stairs #3",
            "022: Angel City Building #9 (Unused)",
            "023: Angel City Building #10",
            "024: Angel City Building #10 Fences",
            "025: Angel City Building #11",
            "026: Angel City Fences #2",
            "027: Angel City Oil Drum Fire",
            "028: Angel City Building #12",
            "029: Simian Acres American Dream Car (Destroyable) (Unused)",
            "030: Simian Acres Track Sign",
            "031: Simian Acres Building #1 (Unused)",
            "032: Simian Acres Barn #1",
            "033: Simian Acres Barn #1 Fences",
            "034: Simian Acres Wagon",
            "035: Simian Acres Building #2",
            "036: Simian Acres Toilet",
            "037: Simian Acres Bathtub",
            "038: Simian Acres Fences",
            "039: Simian Acres Windmill",
            "040: Simian Acres Building #3",
            "041: Simian Acres Building #3 Fences (Unused)",
            "042: Simian Acres Building #4",
            "043: Simian Acres Building #4 Fences",
            "044: Simian Acres Barn #2",
            "045: Simian Acres Barn #2 Fences",
            "046: Simian Acres Tractor (Unused)",
            "047: Simian Acres Building #5",
            "048: Simian Acres Building #6",
            "049: Simian Acres Barn #3",
            "050: Angel City Black Car (Destroyable) (Unused)",
            "051: Carrick Point Container (Red) (Unused)",
            "052: Carrick Point Container (Green) (Unused)",
            "053: Carrick Point Container (Whites) (Unused)",
            "054: Carrick Point Metal Plates",
            "055: Backlash (Unused)",
            "056: Blast Corps Semi",
            "057: Simian Acres Building #7",
            "058: Simian Acres Building #7 Fences (Unused)",
            "059: Simian Acres Wooden Container",
            "060: Simian Acres Gas Station House",
            "061: Simian Acres Gas Station Garage",
            "062: Simian Acres Gas Station",
            "063: Simian Acres Gas Station Hut",
            "064: Simian Acres Gas Station Sign",
            "065: Simian Acres Building #9",
            "066: Simian Acres Building #9 Fences",
            "067: Simian Acres Fences",
            "068: Simian Acres Building #9 House",
            "069: Simian Acres Building #9 Garage",
            "070: Simian Acres Building #11",
            "071: Simian Acres Building #11 Fences",
            "072: Simian Acres Building #12",
            "073: Simian Acres Building #12 Fences",
            "074: Simian Acres Building #13",
            "075: Simian Acres Building #13 Fences",
            "076: Simian Acres Building #14",
            "077: Simian Acres Building #14 Fences (Unused)",
            "078: Simian Acres Building #15",
            "079: Simian Acres Building #16",
            "080: Simian Acres Building #16 Fences",
            "081: Simian Acres Building #17 (Unused)",
            "082: Simian Acres Building #17",
            "083: Simian Acres Building #18 Fences",
            "084: Simian Acres Building #18",
            "085: Simian Acres Fences #2",
            "086: Simian Acres Building #19",
            "087: Simian Acres Building #20 Hut",
            "088: Simian Acres Building #20 ",
            "089: Simian Acres Building #20 Fences",
            "090: Simian Acres Building #21",
            "091: Simian Acres Building #21 Fences (Unused)",
            "092: Simian Acres Building #22 (Unused)",
            "093: Simian Acres Building #22 Fences (Unused)",
            "094: Simian Acres Building #23",
            "095: Simian Acres Tree #1 (Tall) (Unused)",
            "096: Simian Acres Tree #2 (Small) (Unused)",
            "097: Angel City Building #13",
            "098: Carrick Point Building #1",
            "099: Carrick Point Orange Container (Unused)",
            "100: Carrick Point Container (Red)",
            "101: Carrick Point Container (White)",
            "102: Carrick Point Container (Green)",
            "103: Carrick Point Wooden Box",
            "104: Carrick Point Gas Tank (Blue)",
            "105: Carrick Point Gas Tank (Red)",
            "106: Carrick Point Container (Red)",
            "107: Carrick Point Container (Blue)",
            "108: Carrick Point Container (White)",
            "109: Carrick Point Container (Orange)",
            "110: Carrick Point Container (Green)",
            "111: Carrick Point Building #1 (White Roof)",
            "112: Carrick Point Building #1 (Blue Roof)",
            "113: Carrick Point Building #1 (Red Roof)",
            "114: Carrick Point Building #1 (White Roof) (Unused)",
            "115: Carrick Point Building #2",
            "116: Carrick Point Long Building #1",
            "117: Carrick Point Long Building #2",
            "118: Havoc District Building #1",
            "119: Havoc District Building #2",
            "120: Havoc District Building #3",
            "121: Havoc District Building #4",
            "122: Havoc District Building #5",
            "123: Havoc District Building #6",
            "124: Havoc District Building #7 Part 1/2",
            "125: Havoc District Building #7 Part 2/2",
            "126: Havoc District Building #8 Part 1/2",
            "127: Havoc District Building #8 Part 2/2",
            "128: Havoc District Building #9 Part 1/3",
            "129: Havoc District Building #9 Part 2/3",
            "130: Havoc District Building #9 Part 3/3",
            "131: Outland Farm Building #1",
            "132: Outland Farm Building #2",
            "133: Outland Farm Building #3",
            "134: Outland Farm Building #4",
            "135: Outland Farm Silo (Set of 3)",
            "136: Outland Farm Silo",
            "137: Blackridge Works Building #1",
            "138: Blackridge Works Building #2",
            "139: Blackridge Works Building #3",
            "140: Blackridge Works Silo (Set of 3)",
            "141: Blackridge Works Building #4",
            "142: Blackridge Works Building #5",
            "143: Blackridge Works Fence",
            "144: Small Rectangular (Vertical) (Unused)",
            "145: Oyster Harbor Building #1",
            "146: Oyster Harbor Metal Crate",
            "147: Blackridge Works Large Orange Rectangular (Unused)",
            "148: Blackridge Works Tall Pyramid (Unused)",
            "149: Blackridge Works Decorations #1",
            "150: Carrick Point Containers #1",
            "151: Carrick Point Containers #2",
            "152: Carrick Point Containers #3",
            "153: Carrick Point Containers #4",
            "154: Carrick Point Containers #5",
            "155: Carrick Point Containers #6",
            "156: Carrick Point Ship #1",
            "157: Carrick Point Long Building #1 (Unused)",
            "158: Carrick Point Long Building #2 (Unused)",
            "159: Carrick Point Long Building #3",
            "160: Carrick Point Long Building #4",
            "161: Carrick Point Long Building #5",
            "162: Carrick Point Long Building #6",
            "163: Carrick Point Long Building #7",
            "164: Carrick Point Long Building #8",
            "165: Carrick Point Castle",
            "166: Carrick Point Crane (Unused)",
            "167: Carrick Point Crane (Unused)",
            "168: Carrick Point Ship #2",
            "169: Blackridge Works Decorations #2",
            "170: Building",
            "171: Tempest City Wall Block",
            "172: Tempest City Decorations",
            "173: Tempest City Fences",
            "174: Ebony Coast Building #1",
            "175: Ebony Coast Building #2",
            "176: Ebony Coast Building #3",
            "177: Building",
            "178: Building",
            "179: Tempest City Building #1",
            "180: Ebony Coast Building #4",
            "181: Ebony Coast Building #5",
            "182: Billiards Cue",
            "183: Havoc District Red Statue",
            "184: Tempest City Gray Boxes #1",
            "185: Tempest City Gray Boxes #2",
            "186: Sphere",
            "187: Sphere (Dark)",
            "188: Sphere (Light)",
            "189: Raft",
            "190: Long Building #1 (Unused)",
            "191: Long Building #2 (Unused)",
            "192: Saline",
            "193: Beeton Tracks Building",
            "194: Beeton Tracks Blockade",
            "195: Beeton Tracks Donut Shop",
            "196: Beeton Tracks Glass Roof",
            "197: Beeton Tracks Crane Building",
            "198: Beeton Tracks Building With Glass Roof",
            "199: Gray Thing #1 (Unused)",
            "200: Wood Huts (Set of 2)",
            "201: Gray Thing #2 (Unused)",
            "202: Havoc District Lighthouse",
            "203: Havoc District Crane (Unused)",
            "204: Argent Tower Building #1",
            "205: Argent Tower Building #2",
            "206: Argent Tower Building #3",
            "207: Argent Tower Building #4",
            "208: Argent Tower Building #5",
            "209: Argent Towers Door",
            "210: Argent Towers Door (Set of 2)",
            "211: Ebony Coast Big Building #1 Part 1/4",
            "212: Ebony Coast Big Building #1 Part 2/4",
            "213: Ebony Coast Big Building #1 Part 3/4",
            "214: Ebony Coast Big Building #1 Part 4/4",
            "215: Ebony Coast Big Building #2 Part 1/4",
            "216: Ebony Coast Big Building #2 Part 2/4",
            "217: Ebony Coast Big Building #2 Part 3/4",
            "218: Ebony Coast Big Building #2 Part 4/4",
            "219: Ebony Coast Big Building #3 Part 1/4",
            "220: Ebony Coast Big Building #3 Part 2/4",
            "221: Ebony Coast Big Building #3 Part 3/4",
            "222: Ebony Coast Big Building #3 Part 4/4",
            "223: Ebony Coast Island Statue #1",
            "224: Ebony Coast Island Statue #2",
            "225: Ebony Coast Island Statue #3",
            "226: Ebony Coast Island Statue #4",
            "227: Ebony Coast Station Building",
            "228: Ebony Coast Stone Block",
            "229: Pac-Truck (Red)",
            "230: Wood Box",
            "231: Pac-Truck (Blue)",
            "232: Pac-Truck (Green)",
            "233: Pac-Truck (Yellow)",
            "234: Oyster Harbor Metal Crates",
            "235: Ironstone Mine Long Building #1",
            "236: Ironstone Mine Long Building #2",
            "237: Ironstone Mine Long Building #3",
            "238: Ironstone Mine Long Building #4",
            "239: Ironstone Mine Donut-Building",
            "240: Ironstone Mine Building #1",
            "241: Ironstone Mine Wood Huts (Set of 2)",
            "242: Ironstone Mine Wood Hut",
            "243: Ironstone Mine Long Building #5 (Triangular Shaped Roof)",
            "244: Ironstone Mine Chimney",
            "245: Ironstone Mine Tall Building with Wheel",
            "246: Ironstone Mine Building #2",
            "247: Ironstone Mine Building #3",
            "248: Ironstone Mine Long Building #4",
            "249: Moon Machines Center",
            "250: Moon Machines Box with Pipe #1",
            "251: Moon Machines Box with Pipe #2",
            "252: Moon Machines Box with Pipe #3",
            "253: Moon Machines Box with Pipe #4",
            "254: Moon Red Box",
            "255: Moon Antenna #1",
            "256: Moon Antenna #2",
            "257: Moon Antenna #3",
            "258: Moon Boxes (Set of 4)",
            "259: Moon Boxes (Set of 8)",
            "260: Moon Boxes (Set of 4)",
            "261: Moon Building",
            "262: Shuttle Island Shuttle Bridge",
            "263: Diamond Sands Big Building #1 Part 1/4",
            "264: Diamond Sands Big Building #1 Part 2/4",
            "265: Diamond Sands Big Building #1 Part 3/4",
            "266: Diamond Sands Big Building #1 Part 4/4",
            "267: Diamond Sands Big Building #2 Part 1/3",
            "268: Diamond Sands Big Building #2 Part 2/3",
            "269: Diamond Sands Big Building #2 Part 3/3",
            "270: Building",
            "271: Gas Station (Black)",
            "272: Gas Station (Red)",
            "273: End Sequence Rocks #1",
            "274: End Sequence Rocks #2",
            "275: End Sequence Rocks #3",
            "276: End Sequence Rocks #4",
            "277: End Sequence Rocks #5"}, new System.EventHandler(this.comboBoxBuildingType_SelectedIndexChanged));
         comboBoxBuildingType.DropDownWidth = 400;
         comboBoxBuildingType.Width = 150;
         labelBuildingCount = createLabel("Count:");
         numericBuildingCounter = createNumeric(0, 255, new System.EventHandler(this.numericBuildingCounter_ValueChanged));
         labelBuildingB9 = createLabel("U8_9:");
         numericBuildingB9 = createNumeric(0, 255, new System.EventHandler(this.numericBuildingB9_ValueChanged));
         labelBuildingMovement = createLabel("Movement:");
         comboBoxBuildingMovement = createComboBox(new object[] {
            "0: Normal",
            "1: Vertically",
            "2: Circle",
            "3: Horizontally",
            "4: Following Player",
            "5: Rotation 90°"}, new System.EventHandler(this.comboBoxBuildingMovement_SelectedIndexChanged));
         numericBuildingSpeed = createNumeric(0, 65535, new System.EventHandler(this.numericBuildingSpeed_ValueChanged));

         labelDummy = new Label();
      }

      private Label createLabel(string text)
      {
         Label label = new Label();
         label.AutoSize = true;
         label.Text = text;
         label.TextAlign = ContentAlignment.MiddleLeft;
         label.Dock = DockStyle.Fill;
         return label;
      }

      private NumericUpDown createNumeric(int min, int max, System.EventHandler handler)
      {
         NumericUpDown numeric = new NumericUpDown();
         numeric.AutoSize = true;
         numeric.Maximum = new decimal(max);
         numeric.Minimum = new decimal(min);
         numeric.ValueChanged += handler;
         return numeric;
      }

      private ComboBox createComboBox(object[] options, System.EventHandler handler)
      {
         ComboBox combo = new ComboBox();
         combo.AutoSize = true;
         combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         combo.FormattingEnabled = true;
         combo.Items.AddRange(options);
         combo.SelectedIndexChanged += handler;
         return combo;
      }

      private void readData(byte[] levelData, byte[] displayList)
      {
         level = BlastCorpsLevel.decodeLevel(levelData, displayList);

         listViewHeaders.Items.Clear();
         for (int i = 0; i < level.header.u16s.Length; i++)
         {
            numericHeaders[i].Value = level.header.u16s[i];
         }
         numericGravity.Value = level.header.gravity;
         numeric1C.Value = level.header.u1C;
         for (int i = 0; i < level.header.offsets.Length-1; i++)
         {
            int offset = 2 * level.header.u16s.Length + 8 + 4 * i;
            ListViewItem item = new ListViewItem("0x" + offset.ToString("X2"));
            item.SubItems.Add(level.header.offsets[i].ToString("X8"));
            uint len;
            // most of them go in order, but some are switched around. 0x84 is last
            switch (offset)
            {
               case 0x74: len = level.header.offsets[(0xA0 - 0x20) / 4] - level.header.offsets[i]; break;
               case 0x78: len = level.header.offsets[(0x88 - 0x20) / 4] - level.header.offsets[i]; break;
               case 0x84: len = (uint)level.displayList.Length + (uint)level.copyLevelData.Length - level.header.offsets[i]; break;
               case 0x8C: len = level.header.offsets[(0x7C - 0x20) / 4] - level.header.offsets[i]; break;
               case 0xC0: len = level.header.offsets[(0x88 - 0x20) / 4] - level.header.offsets[i]; break;
               case 0x9C: len = level.header.offsets[(0x84 - 0x20) / 4] - level.header.offsets[i]; break;
               default: len = level.header.offsets[i+1] - level.header.offsets[i]; break;
            }
            if (len > 0)
            {
               item.SubItems.Add(len.ToString("X"));
            }
            listViewHeaders.Items.Add(item);
         }

         statusStripMessage.Text = level.bounds.ToString();

         blastCorpsViewer.ShowGridLines = gridLinesToolStripMenuItem.Checked;
         blastCorpsViewer.ShowBoundingBoxes40 = boundingBoxes0x40ToolStripMenuItem.Checked;
         blastCorpsViewer.ShowBoundingBoxes44 = boundingBoxes0x44ToolStripMenuItem.Checked;
         blastCorpsViewer.SetLevel(level);

         toolStripComboBoxLevel.Enabled = true;
         saveToolStripMenuItem.Enabled = true;
         saveRunToolStripMenuItem.Enabled = true;
         exportLevelToolStripMenuItem.Enabled = true;
         exportModelToolStripMenuItem.Enabled = true;

         populateObjectTreeView();

         SetSelectedItem(null);
      }

      private void deleteNode(TreeNode root, BlastCorpsItem item)
      {
         foreach (TreeNode node in root.Nodes)
         {
            if (node.Tag == item)
            {
               root.Nodes.Remove(node);
               break;
            }
         }
      }

      private void addAmmoNode(AmmoBox ammo)
      {
         int icon = ICON_AMMO + ammo.type;
         TreeNode ammoNode = new TreeNode(ammo.ToString(), icon, icon);
         ammoNode.Tag = ammo;
         treeNodeAmmo.Nodes.Add(ammoNode);
      }

      private void addCommPtNode(CommPoint comm)
      {
         TreeNode commNode = new TreeNode(comm.ToString(), ICON_COMM, ICON_COMM);
         commNode.Tag = comm;
         treeNodeCommPt.Nodes.Add(commNode);
      }

      private void addRduNode(RDU rdu)
      {
         TreeNode rduNode = new TreeNode(rdu.ToString(), ICON_RDU, ICON_RDU);
         rduNode.Tag = rdu;
         treeNodeRdu.Nodes.Add(rduNode);
      }

      private void addTntNode(TNTCrate tnt)
      {
         TreeNode tntNode = new TreeNode(tnt.ToString(), ICON_TNT, ICON_TNT);
         tntNode.Tag = tnt;
         treeNodeTnt.Nodes.Add(tntNode);
      }

      private void addBlockNode(SquareBlock block)
      {
         int icon = ICON_BLOCK;
         if (block.hole == 8)
         {
            icon = (block.type == 0) ? ICON_BLOCK + 2 : ICON_BLOCK + 3;
         }
         else
         {
            icon = (block.hole == 0) ? ICON_BLOCK : ICON_BLOCK + 1;
         }
         TreeNode blockNode = new TreeNode(block.ToString(), icon, icon);
         blockNode.Tag = block;
         treeNodeBlock.Nodes.Add(blockNode);
      }

      private int vehicleTypeToInt(byte type)
      {
         int vehicleImage = 0;
         switch (type)
         {
            case 0x00: vehicleImage = ICON_VEHICLE; break;      // 00 Player          driver
            case 0x01: vehicleImage = ICON_VEHICLE + 1; break;  // 01 Sideswipe       sideswipe
            case 0x02: vehicleImage = ICON_VEHICLE + 2; break;  // 02 Thunderfist     magoo
            case 0x03: vehicleImage = ICON_VEHICLE + 3; break;  // 03 Skyfall         buggy
            case 0x04: vehicleImage = ICON_VEHICLE + 4; break;  // 04 Ramdozer        ramdozer
            case 0x05: vehicleImage = ICON_VEHICLE + 5; break;  // 05 Backlash        truck
            case 0x06: vehicleImage = ICON_VEHICLE + 6; break;  // 06 Crane           crane
            case 0x07: vehicleImage = ICON_VEHICLE + 7; break;  // 07 Train           train
            case 0x08: vehicleImage = ICON_VEHICLE + 8; break;  // 08 American Dream  hotrod
            case 0x09: vehicleImage = ICON_VEHICLE + 9; break;  // 09 J-Bomb          jetpack
            case 0x0A: vehicleImage = ICON_VEHICLE + 10; break; // 0A Ballista        bike
            case 0x0B: vehicleImage = ICON_VEHICLE + 11; break; // 0B Boat 1          barge
            // 0C - - -
            case 0x0D: vehicleImage = ICON_VEHICLE + 12; break; // 0D Police Car      police
            case 0x0E: vehicleImage = ICON_VEHICLE + 13; break; // 0E A-Team Van      ateam
            case 0x0F: vehicleImage = ICON_VEHICLE + 14; break; // 0F Red Car         starski
            case 0x10: vehicleImage = ICON_VEHICLE + 15; break; // 10 Cyclone Suit    minimagoo
            case 0x11: vehicleImage = ICON_VEHICLE + 11; break; // 11 Boat 2          barge
            case 0x12: vehicleImage = ICON_VEHICLE + 11; break; // 12 Boat 3          barge
         }
         return vehicleImage;
      }

      private void addVehicleNode(Vehicle vehicle)
      {
         int vehicleImage = vehicleTypeToInt(vehicle.type);
         TreeNode vehicleNode = new TreeNode(vehicle.ToString(), vehicleImage, vehicleImage);
         vehicleNode.Tag = vehicle;
         treeNodeVehicle.Nodes.Add(vehicleNode);
      }

      private void addBuildingNode(Building building)
      {
         TreeNode buildingNode = new TreeNode(building.ToString(), ICON_BUILDING, ICON_BUILDING);
         buildingNode.Tag = building;
         treeNodeBuilding.Nodes.Add(buildingNode);
      }

      private void updateAmmoRoot()
      {
         treeNodeAmmo.Text = "Ammo Boxes [" + level.ammoBoxes.Count + "]";
      }

      private void updateCommPointRoot()
      {
         treeNodeCommPt.Text = "Communication Points [" + level.commPoints.Count + "]";
      }

      private void updateRduRoot()
      {
         treeNodeRdu.Text = "RDUs [" + level.rdus.Count + "]";
      }

      private void updateBlockRoot()
      {
         treeNodeBlock.Text = "Square Blocks [" + level.squareBlocks.Count + "]";
      }

      private void updateTntRoot()
      {
         treeNodeTnt.Text = "TNT Crates [" + level.tntCrates.Count + "]";
      }

      private void updateVehicleRoot()
      {
         treeNodeVehicle.Text = "Vehicles [" + level.vehicles.Count + "]";
      }

      private void updateBuildingRoot()
      {
         treeNodeBuilding.Text = "Buildings [" + level.buildings.Count + "]";
      }

      private void populateObjectTreeView()
      {
         treeNodeCarrier.Tag = level.carrier;

         treeNodeAmmo.Nodes.Clear();
         foreach (AmmoBox ammo in level.ammoBoxes)
         {
            addAmmoNode(ammo);
         }
         updateAmmoRoot();
         treeNodeAmmo.Expand();

         treeNodeCommPt.Nodes.Clear();
         foreach (CommPoint comm in level.commPoints)
         {
            addCommPtNode(comm);
         }
         updateCommPointRoot();
         treeNodeCommPt.Expand();

         treeNodeRdu.Nodes.Clear();
         foreach (RDU rdu in level.rdus)
         {
            addRduNode(rdu);
         }
         updateRduRoot();

         treeNodeTnt.Nodes.Clear();
         foreach (TNTCrate tnt in level.tntCrates)
         {
            addTntNode(tnt);
         }
         updateTntRoot();
         treeNodeTnt.Expand();

         treeNodeBlock.Nodes.Clear();
         foreach (SquareBlock block in level.squareBlocks)
         {
            addBlockNode(block);
         }
         updateBlockRoot();
         treeNodeBlock.Expand();

         treeNodeVehicle.Nodes.Clear();
         foreach (Vehicle vehicle in level.vehicles)
         {
            addVehicleNode(vehicle);
         }
         treeNodeVehicle.Expand();
         updateVehicleRoot();

         treeNodeBuilding.Nodes.Clear();
         foreach (Building building in level.buildings)
         {
            addBuildingNode(building);
         }
         updateBuildingRoot();
      }

      private void SelectNode(TreeNode root, BlastCorpsItem item)
      {
         foreach (TreeNode child in root.Nodes)
         {
            if (Object.ReferenceEquals(item, child.Tag))
            {
               treeViewObjects.SelectedNode = child;
               break;
            }
         }
      }

      private void SetSelectedItem(BlastCorpsItem item)
      {
         bool itemChanged = !Object.ReferenceEquals(itemSel, item);
         itemSel = item;
         blastCorpsViewer.SelectedItem = itemSel;
         if (itemChanged)
         {
            tableLayoutProperties.SuspendLayout();
            tableLayoutProperties.Controls.Clear();
            if (itemSel == null)
            {
               groupBoxProperties.Text = "Object Properties:";
            }
            else
            {
               int row = 0;
               numericX.Value = itemSel.x;
               numericY.Value = itemSel.y;
               numericZ.Value = itemSel.z;
               tableLayoutProperties.Controls.Add(labelX, 0, row);
               tableLayoutProperties.Controls.Add(numericX, 1, row++);
               tableLayoutProperties.Controls.Add(labelY, 0, row);
               tableLayoutProperties.Controls.Add(numericY, 1, row++);
               tableLayoutProperties.Controls.Add(labelZ, 0, row);
               tableLayoutProperties.Controls.Add(numericZ, 1, row++);
               row = 0;
               if (itemSel is AmmoBox)
               {
                  groupBoxProperties.Text = "Ammo Box Properties:";
                  AmmoBox ammo = (AmmoBox)itemSel;
                  comboBoxAmmo.SelectedIndex = ammo.type;
                  tableLayoutProperties.Controls.Add(labelType, 2, row);
                  tableLayoutProperties.Controls.Add(comboBoxAmmo, 3, row++);
                  SelectNode(treeNodeAmmo, itemSel);
               }
               else if (itemSel is CommPoint)
               {
                  groupBoxProperties.Text = "Communication Point Properties:";
                  CommPoint comm = (CommPoint)itemSel;
                  numericCommPtH6.Value = comm.h6;
                  tableLayoutProperties.Controls.Add(labelCommPtH6, 2, row);
                  tableLayoutProperties.Controls.Add(numericCommPtH6, 3, row++);
                  SelectNode(treeNodeCommPt, itemSel);
               }
               else if (itemSel is RDU)
               {
                  groupBoxProperties.Text = "RDU Properties:";
                  SelectNode(treeNodeRdu, itemSel);
               }
               else if (itemSel is TNTCrate)
               {
                  groupBoxProperties.Text = "TNT Crate Properties:";
                  TNTCrate tnt = (TNTCrate)itemSel;
                  numericTntB6.Value = tnt.b6;
                  numericTntTimer.Value = tnt.timer;
                  numericTntH8.Value = tnt.h8;
                  numericTntHA.Value = tnt.hA;
                  tableLayoutProperties.Controls.Add(labelTntB6, 2, row);
                  tableLayoutProperties.Controls.Add(numericTntB6, 3, row++);
                  tableLayoutProperties.Controls.Add(labelTntTimer, 2, row);
                  tableLayoutProperties.Controls.Add(numericTntTimer, 3, row++);
                  tableLayoutProperties.Controls.Add(labelTntH8, 2, row);
                  tableLayoutProperties.Controls.Add(numericTntH8, 3, row++);
                  tableLayoutProperties.Controls.Add(labelTntHA, 2, row);
                  tableLayoutProperties.Controls.Add(numericTntHA, 3, row++);
                  SelectNode(treeNodeTnt, itemSel);
               }
               else if (itemSel is SquareBlock)
               {
                  groupBoxProperties.Text = "Square Block Properties:";
                  SquareBlock block = (SquareBlock)itemSel;
                  comboBoxBlockType1.SelectedIndex = block.type;
                  switch (block.hole)
                  {
                     case 0:
                     case 1:
                     case 2:
                        comboBoxBlockType1.SelectedIndex = 0;
                        comboBoxBlockType2.SelectedIndex = block.hole;
                        numericBlockCount.Enabled = false;
                        break;
                     case 8:
                        comboBoxBlockType1.SelectedIndex = block.type;
                        comboBoxBlockType2.SelectedIndex = 3;
                        numericBlockCount.Enabled = true;
                        break;
                  }
                  numericBlockCount.Value = block.count;
                  tableLayoutProperties.Controls.Add(labelBlockType1, 2, row);
                  tableLayoutProperties.Controls.Add(comboBoxBlockType1, 3, row++);
                  tableLayoutProperties.Controls.Add(labelBlockType2, 2, row);
                  tableLayoutProperties.Controls.Add(comboBoxBlockType2, 3, row++);
                  tableLayoutProperties.Controls.Add(labelBlockCount, 2, row);
                  tableLayoutProperties.Controls.Add(numericBlockCount, 3, row++);
                  SelectNode(treeNodeBlock, itemSel);
               }
               else if (itemSel is Vehicle)
               {
                  groupBoxProperties.Text = "Vehicle Properties:";
                  Vehicle veh = (Vehicle)itemSel;
                  comboBoxVehicle.SelectedIndex = veh.type;
                  numericHeading.Value = veh.heading;
                  tableLayoutProperties.Controls.Add(labelType, 2, row);
                  tableLayoutProperties.Controls.Add(comboBoxVehicle, 3, row++);
                  tableLayoutProperties.Controls.Add(labelHeading, 2, row);
                  tableLayoutProperties.Controls.Add(numericHeading, 3, row++);
                  SelectNode(treeNodeVehicle, itemSel);
               }
               else if (itemSel is Carrier)
               {
                  groupBoxProperties.Text = "Carrier Properties:";
                  Carrier carrier = (Carrier)itemSel;
                  numericCarrierSpeed.Value = carrier.speed;
                  numericHeading.Value = carrier.heading;
                  numericCarrierDistance.Value = carrier.distance;
                  tableLayoutProperties.Controls.Add(labelSpeed, 2, row);
                  tableLayoutProperties.Controls.Add(numericCarrierSpeed, 3, row++);
                  tableLayoutProperties.Controls.Add(labelHeading, 2, row);
                  tableLayoutProperties.Controls.Add(numericHeading, 3, row++);
                  tableLayoutProperties.Controls.Add(labelDistance, 2, row);
                  tableLayoutProperties.Controls.Add(numericCarrierDistance, 3, row++);
                  treeViewObjects.SelectedNode = treeNodeCarrier;
               }
               else if (itemSel is Building)
               {
                  groupBoxProperties.Text = "Building Properties:";
                  Building building = (Building)itemSel;
                  comboBoxBuildingType.SelectedIndex = building.type;
                  numericBuildingCounter.Value = building.counter;
                  numericBuildingB9.Value = building.b9;
                  comboBoxBuildingMovement.SelectedIndex = building.movement;
                  numericBuildingSpeed.Value = building.speed;
                  tableLayoutProperties.Controls.Add(labelType, 2, row);
                  tableLayoutProperties.Controls.Add(comboBoxBuildingType, 3, row++);
                  tableLayoutProperties.Controls.Add(labelBuildingCount, 2, row);
                  tableLayoutProperties.Controls.Add(numericBuildingCounter, 3, row++);
                  tableLayoutProperties.Controls.Add(labelBuildingB9, 2, row);
                  tableLayoutProperties.Controls.Add(numericBuildingB9, 3, row++);
                  tableLayoutProperties.Controls.Add(labelBuildingMovement, 2, row);
                  tableLayoutProperties.Controls.Add(comboBoxBuildingMovement, 3, row++);
                  tableLayoutProperties.Controls.Add(labelSpeed, 2, row);
                  tableLayoutProperties.Controls.Add(numericBuildingSpeed, 3, row++);
                  SelectNode(treeNodeBuilding, itemSel);
               }
               tableLayoutProperties.Controls.Add(labelDummy, 4, row);
            }
            tableLayoutProperties.ResumeLayout();
         }
      }

      private void DeleteItem(BlastCorpsItem item)
      {
         if (item is AmmoBox)
         {
            level.ammoBoxes.Remove((AmmoBox)item);
            deleteNode(treeNodeAmmo, item);
            updateAmmoRoot();
         }
         else if (item is CommPoint)
         {
            level.commPoints.Remove((CommPoint)item);
            deleteNode(treeNodeCommPt, item);
            updateCommPointRoot();
         }
         else if (item is RDU)
         {
            level.rdus.Remove((RDU)item);
            deleteNode(treeNodeRdu, item);
            updateRduRoot();
         }
         else if (item is TNTCrate)
         {
            level.tntCrates.Remove((TNTCrate)item);
            deleteNode(treeNodeTnt, item);
            updateTntRoot();
         }
         else if (item is SquareBlock)
         {
            level.squareBlocks.Remove((SquareBlock)item);
            deleteNode(treeNodeBlock, item);
            updateBlockRoot();
         }
         else if (item is Vehicle)
         {
            level.vehicles.Remove((Vehicle)item);
            deleteNode(treeNodeVehicle, item);
            updateVehicleRoot();
         }
         else if (item is Building)
         {
            level.buildings.Remove((Building)item);
            deleteNode(treeNodeBuilding, item);
            updateBuildingRoot();
         }
         SetSelectedItem(null);
      }

      private void LoadRom(string filename)
      {
         rom = new BlastCorpsRom();
         if (rom.LoadRom(filename))
         {
            if (rom.type == BlastCorpsRom.RomType.Invalid || rom.region == BlastCorpsRom.Region.Invalid || rom.version == BlastCorpsRom.Version.Invalid)
            {
               MessageBox.Show("Error, this does not appear to be a valid Blast Corps ROM.", "Invalid ROM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (rom.type == BlastCorpsRom.RomType.Vanilla && rom.region != BlastCorpsRom.Region.US)
            {
               MessageBox.Show("Error, this tool only works with Blast Corps (U) (V1.0 or V1.1) ROMs currently.", "Invalid ROM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
               if (rom.type == BlastCorpsRom.RomType.Vanilla)
               {
                  MessageBox.Show("Vanilla ROM detected. The ROM will be extended to allow editing and when saving, you will be prompted with a \"Save As...\" dialog so this ROM is not overwritten.", "Vanilla ROM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                  rom.ExtendRom();
                  statusStripFile.ForeColor = Color.Blue;
                  string description = "Vanilla ROM";
                  switch (rom.region)
                  {
                     case BlastCorpsRom.Region.Europe: description += " (E)"; break;
                     case BlastCorpsRom.Region.Japan: description += " (J)"; break;
                     case BlastCorpsRom.Region.US: description += " (U)"; break;
                  }
                  switch (rom.version)
                  {
                     case BlastCorpsRom.Version.Ver1p0: description += " (V1.0)"; break;
                     case BlastCorpsRom.Version.Ver1p1: description += " (V1.1)"; break;
                  }
                  statusStripFile.Text = description;
               }
               else
               {
                  statusStripFile.ForeColor = Color.Black;
                  statusStripFile.Text = filename;
               }
               BlastCorpsLevelMeta levelMeta = (BlastCorpsLevelMeta)toolStripComboBoxLevel.SelectedItem;
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
            statusStripFile.Text = "Error loading " + filename;
         }
      }

      private void SaveFile()
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

      private void toolStripComboBoxLevel_SelectedIndexChanged(object sender, EventArgs e)
      {
         BlastCorpsLevelMeta levelMeta = (BlastCorpsLevelMeta)toolStripComboBoxLevel.SelectedItem;
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

      // Header data
      private void numericU16_ValueChanged(object sender, EventArgs e)
      {
         if (level != null)
         {
            NumericUpDown numeric = (NumericUpDown)sender;
            int index = (int)numeric.Tag;
            level.header.u16s[index] = (UInt16)numeric.Value;
         }
      }

      private void numericGravity_ValueChanged(object sender, EventArgs e)
      {
         if (level != null)
         {
            level.header.gravity = (Int32)numericGravity.Value;
         }
      }

      private void numericU1C_ValueChanged(object sender, EventArgs e)
      {
         if (level != null)
         {
            level.header.u1C = (Int32)numeric1C.Value;
         }
      }

      // Object X/Y/Z
      private void updateNode(BlastCorpsItem item)
      {
         if (item is AmmoBox)
         {
            updateAmmoNode((AmmoBox)item);
         }
         else if (item is CommPoint)
         {
            updateCommPointNode((CommPoint)item);
         }
         else if (item is RDU)
         {
            updateRduNode((RDU)item);
         }
         else if (item is TNTCrate)
         {
            updateTntNode((TNTCrate)item);
         }
         else if (item is SquareBlock)
         {
            updateBlockNode((SquareBlock)item);
         }
         else if (item is Vehicle)
         {
            updateVehicleNode((Vehicle)item);
         }
         else if (item is Building)
         {
            updateBuildingNode((Building)item);
         }
      }

      private void numericX_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null)
         {
            itemSel.x = (Int16)numericX.Value;
            blastCorpsViewer.Invalidate();
            updateNode(itemSel);
         }
      }

      private void numericY_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null)
         {
            itemSel.y = (Int16)numericY.Value;
            blastCorpsViewer.Invalidate();
            updateNode(itemSel);
         }
      }

      private void numericZ_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null)
         {
            itemSel.z = (Int16)numericZ.Value;
            blastCorpsViewer.Invalidate();
            updateNode(itemSel);
         }
      }

      // Ammo boxes
      private void updateAmmoNode(AmmoBox ammo)
      {
         foreach (TreeNode node in treeNodeAmmo.Nodes)
         {
            if (node.Tag == ammo)
            {
               node.Text = ammo.ToString();
               int ammoImage = ICON_AMMO + ammo.type;
               node.SelectedImageIndex = ammoImage;
               node.ImageIndex = ammoImage;
               break;
            }
         }
      }

      private void comboBoxAmmo_SelectedIndexChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is AmmoBox)
         {
            AmmoBox ammo = (AmmoBox)itemSel;
            ammo.type = (UInt16)comboBoxAmmo.SelectedIndex;
            blastCorpsViewer.Invalidate();
            updateAmmoNode(ammo);
         }
      }

      // Communication point
      private void updateCommPointNode(CommPoint comm)
      {
         foreach (TreeNode node in treeNodeCommPt.Nodes)
         {
            if (node.Tag == comm)
            {
               node.Text = comm.ToString();
               break;
            }
         }
      }

      private void numericCommPtH6_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is CommPoint)
         {
            CommPoint comm = (CommPoint)itemSel;
            comm.h6 = (UInt16)numericCommPtH6.Value;
            blastCorpsViewer.Invalidate();
            updateCommPointNode(comm);
         }
      }

      // RDU
      private void updateRduNode(RDU rdu)
      {
         foreach (TreeNode node in treeNodeRdu.Nodes)
         {
            if (node.Tag == rdu)
            {
               node.Text = rdu.ToString();
               break;
            }
         }
      }

      // TNT crates
      private void updateTntNode(TNTCrate tnt)
      {
         foreach (TreeNode node in treeNodeTnt.Nodes)
         {
            if (node.Tag == tnt)
            {
               node.Text = tnt.ToString();
               break;
            }
         }
      }

      private void numericTntB6_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is TNTCrate)
         {
            TNTCrate tnt = (TNTCrate)itemSel;
            tnt.b6 = (byte)numericTntB6.Value;
            blastCorpsViewer.Invalidate();
            updateTntNode(tnt);
         }
      }

      private void numericTntTimer_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is TNTCrate)
         {
            TNTCrate tnt = (TNTCrate)itemSel;
            tnt.timer = (byte)numericTntTimer.Value;
            blastCorpsViewer.Invalidate();
            updateTntNode(tnt);
         }
      }

      private void numericTntH8_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is TNTCrate)
         {
            TNTCrate tnt = (TNTCrate)itemSel;
            tnt.h8 = (Int16)numericTntH8.Value;
            blastCorpsViewer.Invalidate();
            updateTntNode(tnt);
         }
      }

      private void numericTntHA_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is TNTCrate)
         {
            TNTCrate tnt = (TNTCrate)itemSel;
            tnt.hA = (Int16)numericTntHA.Value;
            blastCorpsViewer.Invalidate();
            updateTntNode(tnt);
         }
      }

      // Blocks
      private void updateBlockNode(SquareBlock block)
      {
         foreach (TreeNode node in treeNodeBlock.Nodes)
         {
            if (node.Tag == block)
            {
               node.Text = block.ToString();
               break;
            }
         }
      }

      private void comboBoxBlockType1_SelectedIndexChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is SquareBlock)
         {
            SquareBlock block = (SquareBlock)itemSel;
            block.type = (byte)comboBoxBlockType1.SelectedIndex;
            blastCorpsViewer.Invalidate();
            updateBlockNode(block);
         }
      }

      private void comboBoxBlockType2_SelectedIndexChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is SquareBlock)
         {
            SquareBlock block = (SquareBlock)itemSel;
            bool blockCountEnable = false;
            switch (comboBoxBlockType2.SelectedIndex)
            {
               case 0: block.hole = 0; break;
               case 1: block.hole = 1; break;
               case 2: block.hole = 2; break;
               case 3:
                  block.hole = 8;
                  blockCountEnable = true;
                  break;
            }
            numericBlockCount.Enabled = blockCountEnable;
            blastCorpsViewer.Invalidate();
            updateBlockNode(block);
         }
      }

      private void numericBlockType3_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is SquareBlock)
         {
            SquareBlock block = (SquareBlock)itemSel;
            block.count = (UInt16)numericBlockCount.Value;
            blastCorpsViewer.Invalidate();
            updateBlockNode(block);
         }
      }

      // Vehicles
      private void updateVehicleNode(Vehicle vehicle)
      {
         foreach (TreeNode node in treeNodeVehicle.Nodes)
         {
            if (node.Tag == vehicle)
            {
               node.Text = vehicle.ToString();
               int vehicleImage = vehicleTypeToInt(vehicle.type);
               node.SelectedImageIndex = vehicleImage;
               node.ImageIndex = vehicleImage;
               break;
            }
         }
      }

      private void numericCarrierSpeed_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is Carrier)
         {
            Carrier carrier = (Carrier)itemSel;
            carrier.speed = (byte)numericCarrierSpeed.Value;
            blastCorpsViewer.Invalidate();
         }
      }

      private void numericCarrierDistance_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is Carrier)
         {
            Carrier carrier = (Carrier)itemSel;
            carrier.distance = (UInt16)numericCarrierDistance.Value;
            blastCorpsViewer.Invalidate();
         }
      }

      private void comboBoxVehicle_SelectedIndexChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is Vehicle)
         {
            Vehicle veh = (Vehicle)itemSel;
            veh.type = (byte)comboBoxVehicle.SelectedIndex;
            blastCorpsViewer.Invalidate();
            updateVehicleNode(veh);
         }
      }

      private void numericHeading_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null)
         {
            if (itemSel is Vehicle)
            {
               Vehicle veh = (Vehicle)itemSel;
               veh.heading = (Int16)numericHeading.Value;
               blastCorpsViewer.Invalidate();
               updateVehicleNode(veh);
            }
            else if (itemSel is Carrier)
            {
               level.carrier.heading = (UInt16)numericHeading.Value;
               blastCorpsViewer.Invalidate();
            }
         }
      }

      // Buildings
      private void updateBuildingNode(Building building)
      {
         foreach (TreeNode node in treeNodeBuilding.Nodes)
         {
            if (node.Tag == building)
            {
               node.Text = building.ToString();
               break;
            }
         }
      }
      
      private void comboBoxBuildingType_SelectedIndexChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is Building)
         {
            Building b = (Building)itemSel;
            b.type = (UInt16)comboBoxBuildingType.SelectedIndex;
            blastCorpsViewer.Invalidate();
            updateBuildingNode(b);
         }
      }

      private void numericBuildingCounter_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is Building)
         {
            Building b = (Building)itemSel;
            b.counter = (byte)numericBuildingCounter.Value;
            blastCorpsViewer.Invalidate();
            updateBuildingNode(b);
         }
      }

      private void numericBuildingB9_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is Building)
         {
            Building b = (Building)itemSel;
            b.b9 = (byte)numericBuildingB9.Value;
            blastCorpsViewer.Invalidate();
            updateBuildingNode(b);
         }
      }

      private void comboBoxBuildingMovement_SelectedIndexChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is Building)
         {
            Building b = (Building)itemSel;
            b.movement = (UInt16)comboBoxBuildingMovement.SelectedIndex;
            blastCorpsViewer.Invalidate();
            updateBuildingNode(b);
         }
      }

      private void numericBuildingSpeed_ValueChanged(object sender, EventArgs e)
      {
         if (itemSel != null && itemSel is Building)
         {
            Building b = (Building)itemSel;
            b.speed = (UInt16)numericBuildingSpeed.Value;
            blastCorpsViewer.Invalidate();
            updateBuildingNode(b);
         }
      }

      private void treeViewObjects_AfterSelect(object sender, TreeViewEventArgs e)
      {
         TreeNode node = treeViewObjects.SelectedNode;
         if (node != null && node.Tag != null)
         {
            BlastCorpsItem item = (BlastCorpsItem)node.Tag;
            SetSelectedItem(item);
         }
      }

      // File menu
      private void openToolStripMenuItem_Click(object sender, EventArgs e)
      {
         OpenFileDialog openFileDialog1 = new OpenFileDialog();

         openFileDialog1.Filter = "N64 ROM (*.n64;*.v64;*.z64)|*.n64;*.v64;*.z64|All Files (*.*)|*.*";
         openFileDialog1.FilterIndex = 1;

         DialogResult dresult = openFileDialog1.ShowDialog();

         if (dresult == DialogResult.OK)
         {
            LoadRom(openFileDialog1.FileName);
         }
      }

      private void saveToolStripMenuItem_Click(object sender, EventArgs e)
      {
         SaveFile();
      }

      private void saveRunToolStripMenuItem_Click(object sender, EventArgs e)
      {
         SaveFile();
         if (rom.savePath != null)
         {
            ProcessStartInfo psi = new ProcessStartInfo(rom.savePath);
            psi.UseShellExecute = true;
            Process.Start(psi);
         }
      }

      private void exportLevelToolStripMenuItem_Click(object sender, EventArgs e)
      {
         SaveFileDialog saveFileDialog = new SaveFileDialog();
         saveFileDialog.Filter = "Blast Corps RAW Level(*.raw)|*.raw";
         saveFileDialog.Title = "Export Blast Corps Raw Level";
         saveFileDialog.ShowDialog();

         if (saveFileDialog.FileName != "")
         {
            System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog.OpenFile();
            byte[] data = level.ToBytes();
            fs.Write(data, 0, data.Length);
            fs.Close();
         }
      }

      private void exportModelToolStripMenuItem_Click(object sender, EventArgs e)
      {
         ExportDialog exportDialog = new ExportDialog();
         DialogResult result = exportDialog.ShowDialog(this);

         if (result == DialogResult.OK)
         {
            switch (exportDialog.DataType)
            {
               case ExportType.Terrain30:
                  WavefrontObjExporter.ExportTerrain(level.terrainGroups, exportDialog.FileName, exportDialog.ScaleFactor);
                  break;
               case ExportType.Collision24:
                  WavefrontObjExporter.ExportCollision(level.collision24, exportDialog.FileName, exportDialog.ScaleFactor);
                  break;
               case ExportType.Collision6C:
                  WavefrontObjExporter.ExportCollision(level.collision6C, exportDialog.FileName, exportDialog.ScaleFactor);
                  break;
               case ExportType.Collision70:
                  WavefrontObjExporter.ExportCollision(level.collision70, exportDialog.FileName, exportDialog.ScaleFactor);
                  break;
            }
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
         var thanks = "Special thanks to :\n" +
            "  \u2022 SunakazeKun / Aurum for Blast Corps documentation and testing\n" +
            "  \u2022 SubDrag for the Universal N64 Compressor and notes\n" +
            "  \u2022 Everyone else who has helped along the way";
         MessageBox.Show(appName + " v" + version + "\n" + copyright + "\n\n" + thanks, appName, MessageBoxButtons.OK, MessageBoxIcon.Information);
      }

      // Toolstrip Buttons
      private void setAddTool(object sender)
      {
         foreach (ToolStripItem item in ((ToolStripButton)sender).GetCurrentParent().Items)
         {
            if (item is ToolStripButton)
            {
               ToolStripButton button = (ToolStripButton)item;
               button.Checked = (button == sender);
            }
         }
      }

      private void toolStripButtonSelect_Click(object sender, EventArgs e)
      {
         blastCorpsViewer.Mode = MouseMode.Select;
         setAddTool(sender);
      }

      private void toolStripButtonMove_Click(object sender, EventArgs e)
      {
         blastCorpsViewer.Mode = MouseMode.Move;
         setAddTool(sender);
      }

      private void toolStripButtonAmmo_Click(object sender, EventArgs e)
      {
         setAddTool(sender);
         blastCorpsViewer.Mode = MouseMode.Add;
         blastCorpsViewer.AddType = typeof(AmmoBox);
      }

      private void toolStripButtonCommPt_Click(object sender, EventArgs e)
      {
         setAddTool(sender);
         blastCorpsViewer.Mode = MouseMode.Add;
         blastCorpsViewer.AddType = typeof(CommPoint);
      }

      private void toolStripButtonRdu_Click(object sender, EventArgs e)
      {
         setAddTool(sender);
         blastCorpsViewer.Mode = MouseMode.Add;
         blastCorpsViewer.AddType = typeof(RDU);
      }

      private void toolStripButtonTnt_Click(object sender, EventArgs e)
      {
         setAddTool(sender);
         blastCorpsViewer.Mode = MouseMode.Add;
         blastCorpsViewer.AddType = typeof(TNTCrate);
      }

      private void toolStripButtonBlock_Click(object sender, EventArgs e)
      {
         setAddTool(sender);
         blastCorpsViewer.Mode = MouseMode.Add;
         blastCorpsViewer.AddType = typeof(SquareBlock);
      }

      private void toolStripButtonVehicle_Click(object sender, EventArgs e)
      {
         setAddTool(sender);
         blastCorpsViewer.Mode = MouseMode.Add;
         blastCorpsViewer.AddType = typeof(Vehicle);
      }

      private void toolStripButtonBuilding_Click(object sender, EventArgs e)
      {
         setAddTool(sender);
         blastCorpsViewer.Mode = MouseMode.Add;
         blastCorpsViewer.AddType = typeof(Building);
      }

      private void treeViewObjects_KeyDown(object sender, KeyEventArgs e)
      {
         if (e.KeyCode == Keys.Delete)
         {
            TreeNode node = treeViewObjects.SelectedNode;
            if (node != null && node.Tag != null)
            {
               BlastCorpsItem item = (BlastCorpsItem)node.Tag;
               DeleteItem(item);
               SetSelectedItem(null);
            }
            blastCorpsViewer.Invalidate();
         }
      }
   }
}
