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
         this.checkBoxCollision6C.Location = new System.Drawing.Point(186, 4);
         this.checkBoxCollision6C.Name = "checkBoxCollision6C";
         this.checkBoxCollision6C.Size = new System.Drawing.Size(110, 17);
         this.checkBoxCollision6C.TabIndex = 7;
         this.checkBoxCollision6C.Text = "Show Collision 6C";
         this.checkBoxCollision6C.UseVisualStyleBackColor = true;
         this.checkBoxCollision6C.CheckedChanged += new System.EventHandler(this.settingCheckedChanged);
         // 
         // checkBoxTerrain
         // 
         this.checkBoxTerrain.AutoSize = true;
         this.checkBoxTerrain.Location = new System.Drawing.Point(186, 22);
         this.checkBoxTerrain.Name = "checkBoxTerrain";
         this.checkBoxTerrain.Size = new System.Drawing.Size(89, 17);
         this.checkBoxTerrain.TabIndex = 8;
         this.checkBoxTerrain.Text = "Show Terrain";
         this.checkBoxTerrain.UseVisualStyleBackColor = true;
         this.checkBoxTerrain.CheckedChanged += new System.EventHandler(this.settingCheckedChanged);
         // 
         // checkBoxDisplay
         // 
         this.checkBoxDisplay.AutoSize = true;
         this.checkBoxDisplay.Checked = true;
         this.checkBoxDisplay.CheckState = System.Windows.Forms.CheckState.Checked;
         this.checkBoxDisplay.Location = new System.Drawing.Point(303, 4);
         this.checkBoxDisplay.Name = "checkBoxDisplay";
         this.checkBoxDisplay.Size = new System.Drawing.Size(109, 17);
         this.checkBoxDisplay.TabIndex = 9;
         this.checkBoxDisplay.Text = "Show Display List";
         this.checkBoxDisplay.UseVisualStyleBackColor = true;
         this.checkBoxDisplay.CheckedChanged += new System.EventHandler(this.settingCheckedChanged);
         // 
         // BlastCorps3DForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(914, 686);
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

   }
}