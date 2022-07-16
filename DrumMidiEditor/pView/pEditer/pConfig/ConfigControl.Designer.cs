namespace DrumMidiEditor.pView.pEditer.pConfig
{
	partial class ConfigControl
    {
		/// <summary> 
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region コンポーネント デザイナで生成されたコード

		/// <summary> 
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を 
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.MidiDeviceListBox = new System.Windows.Forms.ListBox();
            this.MidiOutLatencyNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.FpsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.CodecComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.MidiOutLatencyNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FpsNumericUpDown)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ToolTip
            // 
            this.ToolTip.AutomaticDelay = 2000;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(329, 192);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(129, 43);
            this.label5.TabIndex = 78;
            this.label5.Text = "ms";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MidiDeviceListBox
            // 
            this.MidiDeviceListBox.BackColor = System.Drawing.Color.Black;
            this.MidiDeviceListBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MidiDeviceListBox.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MidiDeviceListBox.ForeColor = System.Drawing.Color.Lime;
            this.MidiDeviceListBox.FormattingEnabled = true;
            this.MidiDeviceListBox.ItemHeight = 36;
            this.MidiDeviceListBox.Location = new System.Drawing.Point(37, 69);
            this.MidiDeviceListBox.Margin = new System.Windows.Forms.Padding(0);
            this.MidiDeviceListBox.Name = "MidiDeviceListBox";
            this.MidiDeviceListBox.Size = new System.Drawing.Size(497, 110);
            this.MidiDeviceListBox.TabIndex = 75;
            this.MidiDeviceListBox.SelectedIndexChanged += new System.EventHandler(this.MidiDeviceListBox_SelectedIndexChanged);
            // 
            // MidiOutLatencyNumericUpDown
            // 
            this.MidiOutLatencyNumericUpDown.BackColor = System.Drawing.Color.Black;
            this.MidiOutLatencyNumericUpDown.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.MidiOutLatencyNumericUpDown.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MidiOutLatencyNumericUpDown.ForeColor = System.Drawing.Color.LimeGreen;
            this.MidiOutLatencyNumericUpDown.Location = new System.Drawing.Point(218, 192);
            this.MidiOutLatencyNumericUpDown.Margin = new System.Windows.Forms.Padding(0);
            this.MidiOutLatencyNumericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.MidiOutLatencyNumericUpDown.Name = "MidiOutLatencyNumericUpDown";
            this.MidiOutLatencyNumericUpDown.Size = new System.Drawing.Size(98, 43);
            this.MidiOutLatencyNumericUpDown.TabIndex = 76;
            this.MidiOutLatencyNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.MidiOutLatencyNumericUpDown.ValueChanged += new System.EventHandler(this.MidiOutLatencyNumericUpDown_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(24, 41);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(318, 36);
            this.label2.TabIndex = 74;
            this.label2.Text = "Select Midi-out Device";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(24, 181);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(171, 87);
            this.label3.TabIndex = 77;
            this.label3.Text = "Midi-Out latency";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FpsNumericUpDown
            // 
            this.FpsNumericUpDown.BackColor = System.Drawing.Color.Black;
            this.FpsNumericUpDown.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FpsNumericUpDown.ForeColor = System.Drawing.Color.Lime;
            this.FpsNumericUpDown.Location = new System.Drawing.Point(148, 111);
            this.FpsNumericUpDown.Margin = new System.Windows.Forms.Padding(0);
            this.FpsNumericUpDown.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.FpsNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.FpsNumericUpDown.Name = "FpsNumericUpDown";
            this.FpsNumericUpDown.Size = new System.Drawing.Size(132, 43);
            this.FpsNumericUpDown.TabIndex = 82;
            this.FpsNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.FpsNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.FpsNumericUpDown.ValueChanged += new System.EventHandler(this.FpsNumericUpDown_ValueChanged);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.label6.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(37, 111);
            this.label6.Margin = new System.Windows.Forms.Padding(0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 41);
            this.label6.TabIndex = 81;
            this.label6.Text = "Fps";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.label1.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(37, 57);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 41);
            this.label1.TabIndex = 80;
            this.label1.Text = "Codec";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CodecComboBox
            // 
            this.CodecComboBox.BackColor = System.Drawing.Color.Black;
            this.CodecComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CodecComboBox.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.CodecComboBox.ForeColor = System.Drawing.Color.Lime;
            this.CodecComboBox.FormattingEnabled = true;
            this.CodecComboBox.Items.AddRange(new object[] {
            "AVC",
            "CVID",
            "DIB",
            "DIV3",
            "DIVX",
            "DV25",
            "DV50",
            "DVC",
            "DVH1",
            "DVHD",
            "DVSD",
            "DVSL",
            "H261",
            "H263",
            "H264",
            "H265",
            "HEVC",
            "I420",
            "IV32",
            "IV41",
            "IV50",
            "IYUB",
            "IYUV",
            "JPEG",
            "M4S2",
            "MJPG",
            "MP42",
            "MP43",
            "MP4S",
            "MP4V",
            "MPG1",
            "MPG2",
            "MPG4",
            "MSS1",
            "MSS2",
            "MSVC",
            "PIM1",
            "WMV1",
            "WMV2",
            "WMV3",
            "WVC1",
            "X264",
            "XVID"});
            this.CodecComboBox.Location = new System.Drawing.Point(148, 57);
            this.CodecComboBox.Margin = new System.Windows.Forms.Padding(0);
            this.CodecComboBox.Name = "CodecComboBox";
            this.CodecComboBox.Size = new System.Drawing.Size(376, 44);
            this.CodecComboBox.Sorted = true;
            this.CodecComboBox.TabIndex = 79;
            this.CodecComboBox.SelectedIndexChanged += new System.EventHandler(this.CodecComboBox_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.MidiOutLatencyNumericUpDown);
            this.groupBox1.Controls.Add(this.MidiDeviceListBox);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Font = new System.Drawing.Font("Meiryo UI", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(3, 4);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(565, 291);
            this.groupBox1.TabIndex = 83;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Midi device";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.CodecComboBox);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.FpsNumericUpDown);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox2.Font = new System.Drawing.Font("Meiryo UI", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(3, 286);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Size = new System.Drawing.Size(565, 180);
            this.groupBox2.TabIndex = 84;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Video";
            // 
            // ConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 26F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ConfigControl";
            this.Size = new System.Drawing.Size(1115, 850);
            ((System.ComponentModel.ISupportInitialize)(this.MidiOutLatencyNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FpsNumericUpDown)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

		}

        #endregion

        private System.Windows.Forms.ToolTip ToolTip;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListBox MidiDeviceListBox;
        private System.Windows.Forms.NumericUpDown MidiOutLatencyNumericUpDown;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown FpsNumericUpDown;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox CodecComboBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}
