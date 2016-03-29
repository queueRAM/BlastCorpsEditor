namespace BlastCorpsEditor
{
   partial class TextureForm
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
         this.numericWidth = new System.Windows.Forms.NumericUpDown();
         this.label1 = new System.Windows.Forms.Label();
         this.numericHeight = new System.Windows.Forms.NumericUpDown();
         this.label2 = new System.Windows.Forms.Label();
         this.label3 = new System.Windows.Forms.Label();
         this.numericImage = new System.Windows.Forms.NumericUpDown();
         this.buttonReload = new System.Windows.Forms.Button();
         this.groupBox1 = new System.Windows.Forms.GroupBox();
         this.label7 = new System.Windows.Forms.Label();
         this.textBoxUncompressed = new System.Windows.Forms.TextBox();
         this.label6 = new System.Windows.Forms.Label();
         this.textBoxType = new System.Windows.Forms.TextBox();
         this.label5 = new System.Windows.Forms.Label();
         this.textBoxLength = new System.Windows.Forms.TextBox();
         this.textBoxOffset = new System.Windows.Forms.TextBox();
         this.label4 = new System.Windows.Forms.Label();
         this.label8 = new System.Windows.Forms.Label();
         this.comboBoxFormat = new System.Windows.Forms.ComboBox();
         this.pictureBoxActual = new System.Windows.Forms.PictureBox();
         this.buttonExport = new System.Windows.Forms.Button();
         this.numericLut = new System.Windows.Forms.NumericUpDown();
         this.label9 = new System.Windows.Forms.Label();
         this.pictureBoxTexture = new BlastCorpsEditor.PictureBoxInterpolation();
         ((System.ComponentModel.ISupportInitialize)(this.numericWidth)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numericHeight)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numericImage)).BeginInit();
         this.groupBox1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.pictureBoxActual)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numericLut)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTexture)).BeginInit();
         this.SuspendLayout();
         // 
         // numericWidth
         // 
         this.numericWidth.Location = new System.Drawing.Point(67, 172);
         this.numericWidth.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
         this.numericWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
         this.numericWidth.Name = "numericWidth";
         this.numericWidth.Size = new System.Drawing.Size(98, 20);
         this.numericWidth.TabIndex = 0;
         this.numericWidth.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(12, 174);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(38, 13);
         this.label1.TabIndex = 1;
         this.label1.Text = "Width:";
         // 
         // numericHeight
         // 
         this.numericHeight.Location = new System.Drawing.Point(67, 199);
         this.numericHeight.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
         this.numericHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
         this.numericHeight.Name = "numericHeight";
         this.numericHeight.Size = new System.Drawing.Size(98, 20);
         this.numericHeight.TabIndex = 2;
         this.numericHeight.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(12, 201);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(41, 13);
         this.label2.TabIndex = 3;
         this.label2.Text = "Height:";
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(12, 14);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(39, 13);
         this.label3.TabIndex = 5;
         this.label3.Text = "Image:";
         // 
         // numericImage
         // 
         this.numericImage.Hexadecimal = true;
         this.numericImage.Location = new System.Drawing.Point(67, 12);
         this.numericImage.Maximum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
         this.numericImage.Name = "numericImage";
         this.numericImage.Size = new System.Drawing.Size(98, 20);
         this.numericImage.TabIndex = 6;
         this.numericImage.ValueChanged += new System.EventHandler(this.numericImage_ValueChanged);
         // 
         // buttonReload
         // 
         this.buttonReload.Location = new System.Drawing.Point(15, 280);
         this.buttonReload.Name = "buttonReload";
         this.buttonReload.Size = new System.Drawing.Size(150, 23);
         this.buttonReload.TabIndex = 7;
         this.buttonReload.Text = "Reload";
         this.buttonReload.UseVisualStyleBackColor = true;
         this.buttonReload.Click += new System.EventHandler(this.buttonReload_Click);
         // 
         // groupBox1
         // 
         this.groupBox1.Controls.Add(this.label7);
         this.groupBox1.Controls.Add(this.textBoxUncompressed);
         this.groupBox1.Controls.Add(this.label6);
         this.groupBox1.Controls.Add(this.textBoxType);
         this.groupBox1.Controls.Add(this.label5);
         this.groupBox1.Controls.Add(this.textBoxLength);
         this.groupBox1.Controls.Add(this.textBoxOffset);
         this.groupBox1.Controls.Add(this.label4);
         this.groupBox1.Location = new System.Drawing.Point(15, 38);
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.Size = new System.Drawing.Size(156, 128);
         this.groupBox1.TabIndex = 8;
         this.groupBox1.TabStop = false;
         this.groupBox1.Text = "Properties:";
         // 
         // label7
         // 
         this.label7.AutoSize = true;
         this.label7.Location = new System.Drawing.Point(8, 101);
         this.label7.Name = "label7";
         this.label7.Size = new System.Drawing.Size(45, 13);
         this.label7.TabIndex = 7;
         this.label7.Text = "Inflated:";
         // 
         // textBoxUncompressed
         // 
         this.textBoxUncompressed.Location = new System.Drawing.Point(52, 98);
         this.textBoxUncompressed.Name = "textBoxUncompressed";
         this.textBoxUncompressed.ReadOnly = true;
         this.textBoxUncompressed.Size = new System.Drawing.Size(98, 20);
         this.textBoxUncompressed.TabIndex = 6;
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.Location = new System.Drawing.Point(8, 75);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(34, 13);
         this.label6.TabIndex = 5;
         this.label6.Text = "Type:";
         // 
         // textBoxType
         // 
         this.textBoxType.Location = new System.Drawing.Point(52, 72);
         this.textBoxType.Name = "textBoxType";
         this.textBoxType.ReadOnly = true;
         this.textBoxType.Size = new System.Drawing.Size(98, 20);
         this.textBoxType.TabIndex = 4;
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.Location = new System.Drawing.Point(8, 49);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(43, 13);
         this.label5.TabIndex = 3;
         this.label5.Text = "Length:";
         // 
         // textBoxLength
         // 
         this.textBoxLength.Location = new System.Drawing.Point(52, 46);
         this.textBoxLength.Name = "textBoxLength";
         this.textBoxLength.ReadOnly = true;
         this.textBoxLength.Size = new System.Drawing.Size(98, 20);
         this.textBoxLength.TabIndex = 2;
         // 
         // textBoxOffset
         // 
         this.textBoxOffset.Location = new System.Drawing.Point(52, 19);
         this.textBoxOffset.Name = "textBoxOffset";
         this.textBoxOffset.ReadOnly = true;
         this.textBoxOffset.Size = new System.Drawing.Size(98, 20);
         this.textBoxOffset.TabIndex = 1;
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(8, 22);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(38, 13);
         this.label4.TabIndex = 0;
         this.label4.Text = "Offset:";
         // 
         // label8
         // 
         this.label8.AutoSize = true;
         this.label8.Location = new System.Drawing.Point(12, 229);
         this.label8.Name = "label8";
         this.label8.Size = new System.Drawing.Size(42, 13);
         this.label8.TabIndex = 9;
         this.label8.Text = "Format:";
         // 
         // comboBoxFormat
         // 
         this.comboBoxFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.comboBoxFormat.FormattingEnabled = true;
         this.comboBoxFormat.Items.AddRange(new object[] {
            "RBGA16 (5551)",
            "RGBA32 (8888)",
            "IA16",
            "IA8",
            "IA4",
            "IA1"});
         this.comboBoxFormat.Location = new System.Drawing.Point(67, 226);
         this.comboBoxFormat.Name = "comboBoxFormat";
         this.comboBoxFormat.Size = new System.Drawing.Size(98, 21);
         this.comboBoxFormat.TabIndex = 10;
         // 
         // pictureBoxActual
         // 
         this.pictureBoxActual.BackColor = System.Drawing.SystemColors.Control;
         this.pictureBoxActual.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.pictureBoxActual.Location = new System.Drawing.Point(177, 12);
         this.pictureBoxActual.Name = "pictureBoxActual";
         this.pictureBoxActual.Size = new System.Drawing.Size(64, 64);
         this.pictureBoxActual.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
         this.pictureBoxActual.TabIndex = 11;
         this.pictureBoxActual.TabStop = false;
         // 
         // buttonExport
         // 
         this.buttonExport.Enabled = false;
         this.buttonExport.Location = new System.Drawing.Point(15, 322);
         this.buttonExport.Name = "buttonExport";
         this.buttonExport.Size = new System.Drawing.Size(150, 23);
         this.buttonExport.TabIndex = 12;
         this.buttonExport.Text = "Export PNG";
         this.buttonExport.UseVisualStyleBackColor = true;
         this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
         // 
         // numericLut
         // 
         this.numericLut.Enabled = false;
         this.numericLut.Hexadecimal = true;
         this.numericLut.Location = new System.Drawing.Point(67, 254);
         this.numericLut.Maximum = new decimal(new int[] {
            8388608,
            0,
            0,
            0});
         this.numericLut.Name = "numericLut";
         this.numericLut.Size = new System.Drawing.Size(98, 20);
         this.numericLut.TabIndex = 13;
         this.numericLut.Value = new decimal(new int[] {
            1386864,
            0,
            0,
            0});
         // 
         // label9
         // 
         this.label9.AutoSize = true;
         this.label9.Location = new System.Drawing.Point(12, 256);
         this.label9.Name = "label9";
         this.label9.Size = new System.Drawing.Size(31, 13);
         this.label9.TabIndex = 14;
         this.label9.Text = "LUT:";
         // 
         // pictureBoxTexture
         // 
         this.pictureBoxTexture.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.pictureBoxTexture.BackColor = System.Drawing.SystemColors.Control;
         this.pictureBoxTexture.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
         this.pictureBoxTexture.Location = new System.Drawing.Point(177, 82);
         this.pictureBoxTexture.Name = "pictureBoxTexture";
         this.pictureBoxTexture.Size = new System.Drawing.Size(265, 264);
         this.pictureBoxTexture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
         this.pictureBoxTexture.TabIndex = 4;
         this.pictureBoxTexture.TabStop = false;
         // 
         // TextureForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(454, 357);
         this.Controls.Add(this.label9);
         this.Controls.Add(this.numericLut);
         this.Controls.Add(this.buttonExport);
         this.Controls.Add(this.pictureBoxActual);
         this.Controls.Add(this.comboBoxFormat);
         this.Controls.Add(this.label8);
         this.Controls.Add(this.groupBox1);
         this.Controls.Add(this.buttonReload);
         this.Controls.Add(this.numericImage);
         this.Controls.Add(this.label3);
         this.Controls.Add(this.pictureBoxTexture);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.numericHeight);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.numericWidth);
         this.Name = "TextureForm";
         this.Text = "Blast Corps Textures";
         this.Load += new System.EventHandler(this.TextureForm_Load);
         ((System.ComponentModel.ISupportInitialize)(this.numericWidth)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numericHeight)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numericImage)).EndInit();
         this.groupBox1.ResumeLayout(false);
         this.groupBox1.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.pictureBoxActual)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numericLut)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTexture)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.NumericUpDown numericWidth;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.NumericUpDown numericHeight;
      private System.Windows.Forms.Label label2;
      private PictureBoxInterpolation pictureBoxTexture;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.NumericUpDown numericImage;
      private System.Windows.Forms.Button buttonReload;
      private System.Windows.Forms.GroupBox groupBox1;
      private System.Windows.Forms.Label label7;
      private System.Windows.Forms.TextBox textBoxUncompressed;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.TextBox textBoxType;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.TextBox textBoxLength;
      private System.Windows.Forms.TextBox textBoxOffset;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.Label label8;
      private System.Windows.Forms.ComboBox comboBoxFormat;
      private System.Windows.Forms.PictureBox pictureBoxActual;
      private System.Windows.Forms.Button buttonExport;
      private System.Windows.Forms.NumericUpDown numericLut;
      private System.Windows.Forms.Label label9;
   }
}