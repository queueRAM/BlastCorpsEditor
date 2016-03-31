namespace BlastCorpsEditor
{
   partial class ExportDialog
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
         this.label1 = new System.Windows.Forms.Label();
         this.comboBoxExport = new System.Windows.Forms.ComboBox();
         this.label2 = new System.Windows.Forms.Label();
         this.textBoxFilename = new System.Windows.Forms.TextBox();
         this.label3 = new System.Windows.Forms.Label();
         this.buttonChoose = new System.Windows.Forms.Button();
         this.numericScale = new System.Windows.Forms.NumericUpDown();
         this.buttonExport = new System.Windows.Forms.Button();
         this.buttonCancel = new System.Windows.Forms.Button();
         ((System.ComponentModel.ISupportInitialize)(this.numericScale)).BeginInit();
         this.SuspendLayout();
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(12, 70);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(70, 13);
         this.label1.TabIndex = 0;
         this.label1.Text = "Data Source:";
         // 
         // comboBoxExport
         // 
         this.comboBoxExport.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.comboBoxExport.FormattingEnabled = true;
         this.comboBoxExport.Items.AddRange(new object[] {
            "0x30: Terrain",
            "0x24: Y Collision",
            "0x6C: X/Z Collision",
            "0x70: Player Collision",
            "Display List"});
         this.comboBoxExport.Location = new System.Drawing.Point(88, 67);
         this.comboBoxExport.Name = "comboBoxExport";
         this.comboBoxExport.Size = new System.Drawing.Size(135, 21);
         this.comboBoxExport.TabIndex = 1;
         this.comboBoxExport.SelectedIndexChanged += new System.EventHandler(this.comboBoxExport_SelectedIndexChanged);
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(12, 15);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(56, 13);
         this.label2.TabIndex = 2;
         this.label2.Text = "Export To:";
         // 
         // textBoxFilename
         // 
         this.textBoxFilename.Location = new System.Drawing.Point(88, 12);
         this.textBoxFilename.Name = "textBoxFilename";
         this.textBoxFilename.ReadOnly = true;
         this.textBoxFilename.Size = new System.Drawing.Size(384, 20);
         this.textBoxFilename.TabIndex = 3;
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(12, 96);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(52, 13);
         this.label3.TabIndex = 4;
         this.label3.Text = "Scale By:";
         // 
         // buttonChoose
         // 
         this.buttonChoose.Location = new System.Drawing.Point(88, 38);
         this.buttonChoose.Name = "buttonChoose";
         this.buttonChoose.Size = new System.Drawing.Size(75, 23);
         this.buttonChoose.TabIndex = 6;
         this.buttonChoose.Text = "Choose...";
         this.buttonChoose.UseVisualStyleBackColor = true;
         this.buttonChoose.Click += new System.EventHandler(this.buttonChoose_Click);
         // 
         // numericScale
         // 
         this.numericScale.DecimalPlaces = 1;
         this.numericScale.Location = new System.Drawing.Point(88, 94);
         this.numericScale.Maximum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
         this.numericScale.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
         this.numericScale.Name = "numericScale";
         this.numericScale.Size = new System.Drawing.Size(135, 20);
         this.numericScale.TabIndex = 7;
         this.numericScale.Value = new decimal(new int[] {
            1024,
            0,
            0,
            0});
         // 
         // buttonExport
         // 
         this.buttonExport.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.buttonExport.Enabled = false;
         this.buttonExport.Location = new System.Drawing.Point(397, 67);
         this.buttonExport.Name = "buttonExport";
         this.buttonExport.Size = new System.Drawing.Size(75, 23);
         this.buttonExport.TabIndex = 8;
         this.buttonExport.Text = "Export";
         this.buttonExport.UseVisualStyleBackColor = true;
         // 
         // buttonCancel
         // 
         this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.buttonCancel.Location = new System.Drawing.Point(397, 96);
         this.buttonCancel.Name = "buttonCancel";
         this.buttonCancel.Size = new System.Drawing.Size(75, 23);
         this.buttonCancel.TabIndex = 9;
         this.buttonCancel.Text = "Cancel";
         this.buttonCancel.UseVisualStyleBackColor = true;
         // 
         // ExportDialog
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(484, 134);
         this.Controls.Add(this.buttonCancel);
         this.Controls.Add(this.buttonExport);
         this.Controls.Add(this.numericScale);
         this.Controls.Add(this.buttonChoose);
         this.Controls.Add(this.label3);
         this.Controls.Add(this.textBoxFilename);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.comboBoxExport);
         this.Controls.Add(this.label1);
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "ExportDialog";
         this.ShowIcon = false;
         this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
         this.Text = "Export Model...";
         this.TopMost = true;
         ((System.ComponentModel.ISupportInitialize)(this.numericScale)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.ComboBox comboBoxExport;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.TextBox textBoxFilename;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Button buttonChoose;
      private System.Windows.Forms.NumericUpDown numericScale;
      private System.Windows.Forms.Button buttonExport;
      private System.Windows.Forms.Button buttonCancel;
   }
}