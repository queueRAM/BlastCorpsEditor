namespace BlastCorpsEditor
{
   partial class BlastCorps3DForm
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.glControlViewer = new OpenTK.GLControl();
         this.labelX = new System.Windows.Forms.Label();
         this.labelY = new System.Windows.Forms.Label();
         this.labelZ = new System.Windows.Forms.Label();
         this.labelHeading = new System.Windows.Forms.Label();
         this.labelPitch = new System.Windows.Forms.Label();
         this.checkBoxCollision6C = new System.Windows.Forms.CheckBox();
         this.checkBoxTerrain = new System.Windows.Forms.CheckBox();
         this.checkBoxDisplay = new System.Windows.Forms.CheckBox();
         this.checkBoxWireframe = new System.Windows.Forms.CheckBox();
         this.checkBoxWalls = new System.Windows.Forms.CheckBox();
         this.checkBoxCollision24 = new System.Windows.Forms.CheckBox();
         this.checkBox44 = new System.Windows.Forms.CheckBox();
         this.checkBox40 = new System.Windows.Forms.CheckBox();
         this.labelType = new System.Windows.Forms.Label();
         this.labelIndex = new System.Windows.Forms.Label();
         this.SuspendLayout();
         // 
         // glControlViewer
         // 
         this.glControlViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.glControlViewer.BackColor = System.Drawing.Color.Black;
         this.glControlViewer.Location = new System.Drawing.Point(0, 51);
         this.glControlViewer.Name = "glControlViewer";
         this.glControlViewer.Size = new System.Drawing.Size(914, 635);
         this.glControlViewer.TabIndex = 1;
         this.glControlViewer.VSync = false;
         this.glControlViewer.Load += new System.EventHandler(this.glControlViewer_Load);
         this.glControlViewer.Paint += new System.Windows.Forms.PaintEventHandler(this.glControlViewer_Paint);
         this.glControlViewer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.glControlViewer_KeyDown);
         this.glControlViewer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.glControlViewer_MouseDown);
         this.glControlViewer.MouseMove += new System.Windows.Forms.MouseEventHandler(this.glControlViewer_MouseMove);
         this.glControlViewer.Resize += new System.EventHandler(this.glControlViewer_Resize);
         // 
         // labelX
         // 
         this.labelX.AutoSize = true;
         this.labelX.Location = new System.Drawing.Point(12, 9);
         this.labelX.Name = "labelX";
         this.labelX.Size = new System.Drawing.Size(17, 13);
         this.labelX.TabIndex = 2;
         this.labelX.Text = "X:";
         // 
         // labelY
         // 
         this.labelY.AutoSize = true;
         this.labelY.Location = new System.Drawing.Point(12, 22);
         this.labelY.Name = "labelY";
         this.labelY.Size = new System.Drawing.Size(17, 13);
         this.labelY.TabIndex = 3;
         this.labelY.Text = "Y:";
         // 
         // labelZ
         // 
         this.labelZ.AutoSize = true;
         this.labelZ.Location = new System.Drawing.Point(12, 35);
         this.labelZ.Name = "labelZ";
         this.labelZ.Size = new System.Drawing.Size(17, 13);
         this.labelZ.TabIndex = 4;
         this.labelZ.Text = "Z:";
         // 
         // labelHeading
         // 
         this.labelHeading.AutoSize = true;
         this.labelHeading.Location = new System.Drawing.Point(74, 9);
         this.labelHeading.Name = "labelHeading";
         this.labelHeading.Size = new System.Drawing.Size(18, 13);
         this.labelHeading.TabIndex = 5;
         this.labelHeading.Text = "H:";
         // 
         // labelPitch
         // 
         this.labelPitch.AutoSize = true;
         this.labelPitch.Location = new System.Drawing.Point(74, 22);
         this.labelPitch.Name = "labelPitch";
         this.labelPitch.Size = new System.Drawing.Size(17, 13);
         this.labelPitch.TabIndex = 6;
         this.labelPitch.Text = "P:";
         // 
         // checkBoxCollision6C
         // 
         this.checkBoxCollision6C.AutoSize = true;
         this.checkBoxCollision6C.Location = new System.Drawing.Point(425, 5);
         this.checkBoxCollision6C.Name = "checkBoxCollision6C";
         this.checkBoxCollision6C.Size = new System.Drawing.Size(138, 17);
         this.checkBoxCollision6C.TabIndex = 7;
         this.checkBoxCollision6C.Text = "Show Collision H (0x6C)";
         this.checkBoxCollision6C.UseVisualStyleBackColor = true;
         this.checkBoxCollision6C.CheckedChanged += new System.EventHandler(this.settingCheckedChanged);
         // 
         // checkBoxTerrain
         // 
         this.checkBoxTerrain.AutoSize = true;
         this.checkBoxTerrain.Location = new System.Drawing.Point(192, 21);
         this.checkBoxTerrain.Name = "checkBoxTerrain";
         this.checkBoxTerrain.Size = new System.Drawing.Size(121, 17);
         this.checkBoxTerrain.TabIndex = 8;
         this.checkBoxTerrain.Text = "Show Terrain (0x30)";
         this.checkBoxTerrain.UseVisualStyleBackColor = true;
         this.checkBoxTerrain.CheckedChanged += new System.EventHandler(this.settingCheckedChanged);
         // 
         // checkBoxDisplay
         // 
         this.checkBoxDisplay.AutoSize = true;
         this.checkBoxDisplay.Checked = true;
         this.checkBoxDisplay.CheckState = System.Windows.Forms.CheckState.Checked;
         this.checkBoxDisplay.Location = new System.Drawing.Point(569, 23);
         this.checkBoxDisplay.Name = "checkBoxDisplay";
         this.checkBoxDisplay.Size = new System.Drawing.Size(109, 17);
         this.checkBoxDisplay.TabIndex = 9;
         this.checkBoxDisplay.Text = "Show Display List";
         this.checkBoxDisplay.UseVisualStyleBackColor = true;
         this.checkBoxDisplay.CheckedChanged += new System.EventHandler(this.settingCheckedChanged);
         // 
         // checkBoxWireframe
         // 
         this.checkBoxWireframe.AutoSize = true;
         this.checkBoxWireframe.Checked = true;
         this.checkBoxWireframe.CheckState = System.Windows.Forms.CheckState.Checked;
         this.checkBoxWireframe.Location = new System.Drawing.Point(569, 5);
         this.checkBoxWireframe.Name = "checkBoxWireframe";
         this.checkBoxWireframe.Size = new System.Drawing.Size(104, 17);
         this.checkBoxWireframe.TabIndex = 10;
         this.checkBoxWireframe.Text = "Show Wireframe";
         this.checkBoxWireframe.UseVisualStyleBackColor = true;
         this.checkBoxWireframe.CheckedChanged += new System.EventHandler(this.settingCheckedChanged);
         // 
         // checkBoxWalls
         // 
         this.checkBoxWalls.AutoSize = true;
         this.checkBoxWalls.Location = new System.Drawing.Point(425, 23);
         this.checkBoxWalls.Name = "checkBoxWalls";
         this.checkBoxWalls.Size = new System.Drawing.Size(114, 17);
         this.checkBoxWalls.TabIndex = 11;
         this.checkBoxWalls.Text = "Show Walls (0x64)";
         this.checkBoxWalls.UseVisualStyleBackColor = true;
         this.checkBoxWalls.CheckedChanged += new System.EventHandler(this.settingCheckedChanged);
         // 
         // checkBoxCollision24
         // 
         this.checkBoxCollision24.AutoSize = true;
         this.checkBoxCollision24.Location = new System.Drawing.Point(192, 5);
         this.checkBoxCollision24.Name = "checkBoxCollision24";
         this.checkBoxCollision24.Size = new System.Drawing.Size(136, 17);
         this.checkBoxCollision24.TabIndex = 12;
         this.checkBoxCollision24.Text = "Show Collision V (0x24)";
         this.checkBoxCollision24.UseVisualStyleBackColor = true;
         this.checkBoxCollision24.CheckedChanged += new System.EventHandler(this.settingCheckedChanged);
         // 
         // checkBox44
         // 
         this.checkBox44.AutoSize = true;
         this.checkBox44.Location = new System.Drawing.Point(334, 23);
         this.checkBox44.Name = "checkBox44";
         this.checkBox44.Size = new System.Drawing.Size(85, 17);
         this.checkBox44.TabIndex = 14;
         this.checkBox44.Text = "Show (0x44)";
         this.checkBox44.UseVisualStyleBackColor = true;
         this.checkBox44.CheckedChanged += new System.EventHandler(this.settingCheckedChanged);
         // 
         // checkBox40
         // 
         this.checkBox40.AutoSize = true;
         this.checkBox40.Location = new System.Drawing.Point(334, 5);
         this.checkBox40.Name = "checkBox40";
         this.checkBox40.Size = new System.Drawing.Size(85, 17);
         this.checkBox40.TabIndex = 13;
         this.checkBox40.Text = "Show (0x40)";
         this.checkBox40.UseVisualStyleBackColor = true;
         this.checkBox40.CheckedChanged += new System.EventHandler(this.settingCheckedChanged);
         // 
         // labelType
         // 
         this.labelType.AutoSize = true;
         this.labelType.Location = new System.Drawing.Point(726, 8);
         this.labelType.Name = "labelType";
         this.labelType.Size = new System.Drawing.Size(34, 13);
         this.labelType.TabIndex = 15;
         this.labelType.Text = "Type:";
         // 
         // labelIndex
         // 
         this.labelIndex.AutoSize = true;
         this.labelIndex.Location = new System.Drawing.Point(729, 25);
         this.labelIndex.Name = "labelIndex";
         this.labelIndex.Size = new System.Drawing.Size(36, 13);
         this.labelIndex.TabIndex = 16;
         this.labelIndex.Text = "Index:";
         // 
         // BlastCorps3DForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(914, 686);
         this.Controls.Add(this.labelIndex);
         this.Controls.Add(this.labelType);
         this.Controls.Add(this.checkBox44);
         this.Controls.Add(this.checkBox40);
         this.Controls.Add(this.checkBoxCollision24);
         this.Controls.Add(this.checkBoxWalls);
         this.Controls.Add(this.checkBoxWireframe);
         this.Controls.Add(this.checkBoxDisplay);
         this.Controls.Add(this.checkBoxTerrain);
         this.Controls.Add(this.checkBoxCollision6C);
         this.Controls.Add(this.labelPitch);
         this.Controls.Add(this.labelHeading);
         this.Controls.Add(this.labelZ);
         this.Controls.Add(this.labelY);
         this.Controls.Add(this.labelX);
         this.Controls.Add(this.glControlViewer);
         this.Name = "BlastCorps3DForm";
         this.ShowIcon = false;
         this.Text = "Blast Corps 3D Viewer";
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private OpenTK.GLControl glControlViewer;
      private System.Windows.Forms.Label labelX;
      private System.Windows.Forms.Label labelY;
      private System.Windows.Forms.Label labelZ;
      private System.Windows.Forms.Label labelHeading;
      private System.Windows.Forms.Label labelPitch;
      private System.Windows.Forms.CheckBox checkBoxCollision6C;
      private System.Windows.Forms.CheckBox checkBoxTerrain;
      private System.Windows.Forms.CheckBox checkBoxDisplay;
      private System.Windows.Forms.CheckBox checkBoxWireframe;
      private System.Windows.Forms.CheckBox checkBoxWalls;
      private System.Windows.Forms.CheckBox checkBoxCollision24;
      private System.Windows.Forms.CheckBox checkBox44;
      private System.Windows.Forms.CheckBox checkBox40;
      private System.Windows.Forms.Label labelType;
      private System.Windows.Forms.Label labelIndex;

   }
}