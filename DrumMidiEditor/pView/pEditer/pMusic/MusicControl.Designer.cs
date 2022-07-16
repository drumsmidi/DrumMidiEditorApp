namespace DrumMidiEditor.pView.pEditer.pMusic
{
	partial class MusicControl
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
            this.label1 = new System.Windows.Forms.Label();
            this.EqulizerGroupBox = new System.Windows.Forms.GroupBox();
            this.WaveOnCheckBox = new System.Windows.Forms.CheckBox();
            this.EqualizerPanel = new DrumMidiEditor.pView.pEditer.pMusic.EqualizerPanel();
            this.EqualizerResetButton = new System.Windows.Forms.Button();
            this.EqualizerOnCheckBox = new System.Windows.Forms.CheckBox();
            this.InfoTextBox = new System.Windows.Forms.TextBox();
            this.BgmVolumeNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.BgmRpmNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.BgmBpmNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.Label0007 = new System.Windows.Forms.Label();
            this.Label0013 = new System.Windows.Forms.Label();
            this.BgmFilePathTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.EqulizerGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BgmVolumeNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BgmRpmNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BgmBpmNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(501, 79);
            this.label1.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 52);
            this.label1.TabIndex = 60;
            this.label1.Text = "Memo";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // EqulizerGroupBox
            // 
            this.EqulizerGroupBox.Controls.Add(this.WaveOnCheckBox);
            this.EqulizerGroupBox.Controls.Add(this.EqualizerPanel);
            this.EqulizerGroupBox.Controls.Add(this.EqualizerResetButton);
            this.EqulizerGroupBox.Controls.Add(this.EqualizerOnCheckBox);
            this.EqulizerGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.EqulizerGroupBox.Font = new System.Drawing.Font("Meiryo UI", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.EqulizerGroupBox.ForeColor = System.Drawing.Color.White;
            this.EqulizerGroupBox.Location = new System.Drawing.Point(16, 380);
            this.EqulizerGroupBox.Margin = new System.Windows.Forms.Padding(0);
            this.EqulizerGroupBox.Name = "EqulizerGroupBox";
            this.EqulizerGroupBox.Padding = new System.Windows.Forms.Padding(0);
            this.EqulizerGroupBox.Size = new System.Drawing.Size(1200, 696);
            this.EqulizerGroupBox.TabIndex = 59;
            this.EqulizerGroupBox.TabStop = false;
            this.EqulizerGroupBox.Text = "Equlizer";
            // 
            // WaveOnCheckBox
            // 
            this.WaveOnCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.WaveOnCheckBox.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.WaveOnCheckBox.BackgroundImage = global::DrumMidiEditor.Properties.Resources.ic_broken_image_black_48dp;
            this.WaveOnCheckBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.WaveOnCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.WaveOnCheckBox.Enabled = false;
            this.WaveOnCheckBox.FlatAppearance.CheckedBackColor = System.Drawing.Color.SeaGreen;
            this.WaveOnCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.WaveOnCheckBox.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.WaveOnCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.WaveOnCheckBox.Location = new System.Drawing.Point(1061, 58);
            this.WaveOnCheckBox.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.WaveOnCheckBox.Name = "WaveOnCheckBox";
            this.WaveOnCheckBox.Size = new System.Drawing.Size(117, 64);
            this.WaveOnCheckBox.TabIndex = 63;
            this.WaveOnCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.WaveOnCheckBox.UseVisualStyleBackColor = false;
            this.WaveOnCheckBox.CheckedChanged += new System.EventHandler(this.WaveOnCheckBox_CheckedChanged);
            // 
            // EqualizerPanel
            // 
            this.EqualizerPanel.Location = new System.Drawing.Point(17, 130);
            this.EqualizerPanel.Margin = new System.Windows.Forms.Padding(0);
            this.EqualizerPanel.Name = "EqualizerPanel";
            this.EqualizerPanel.Size = new System.Drawing.Size(1161, 544);
            this.EqualizerPanel.TabIndex = 62;
            // 
            // EqualizerResetButton
            // 
            this.EqualizerResetButton.BackColor = System.Drawing.Color.DodgerBlue;
            this.EqualizerResetButton.BackgroundImage = global::DrumMidiEditor.Properties.Resources.ic_copyright_black_48dp;
            this.EqualizerResetButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.EqualizerResetButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.EqualizerResetButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.EqualizerResetButton.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.EqualizerResetButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.EqualizerResetButton.Location = new System.Drawing.Point(170, 58);
            this.EqualizerResetButton.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.EqualizerResetButton.Name = "EqualizerResetButton";
            this.EqualizerResetButton.Size = new System.Drawing.Size(117, 64);
            this.EqualizerResetButton.TabIndex = 37;
            this.EqualizerResetButton.UseVisualStyleBackColor = false;
            this.EqualizerResetButton.Click += new System.EventHandler(this.EqualizerResetButton_Click);
            // 
            // EqualizerOnCheckBox
            // 
            this.EqualizerOnCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.EqualizerOnCheckBox.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.EqualizerOnCheckBox.BackgroundImage = global::DrumMidiEditor.Properties.Resources.ic_power_settings_new_black_48dp;
            this.EqualizerOnCheckBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.EqualizerOnCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.EqualizerOnCheckBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.EqualizerOnCheckBox.FlatAppearance.CheckedBackColor = System.Drawing.Color.SeaGreen;
            this.EqualizerOnCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.EqualizerOnCheckBox.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.EqualizerOnCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.EqualizerOnCheckBox.Location = new System.Drawing.Point(17, 58);
            this.EqualizerOnCheckBox.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.EqualizerOnCheckBox.Name = "EqualizerOnCheckBox";
            this.EqualizerOnCheckBox.Size = new System.Drawing.Size(117, 64);
            this.EqualizerOnCheckBox.TabIndex = 35;
            this.EqualizerOnCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.EqualizerOnCheckBox.UseVisualStyleBackColor = false;
            this.EqualizerOnCheckBox.CheckedChanged += new System.EventHandler(this.EqualizerOnCheckBox_CheckedChanged);
            // 
            // InfoTextBox
            // 
            this.InfoTextBox.BackColor = System.Drawing.Color.Black;
            this.InfoTextBox.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.InfoTextBox.ForeColor = System.Drawing.Color.LimeGreen;
            this.InfoTextBox.Location = new System.Drawing.Point(535, 131);
            this.InfoTextBox.Margin = new System.Windows.Forms.Padding(0);
            this.InfoTextBox.MaxLength = 512;
            this.InfoTextBox.Multiline = true;
            this.InfoTextBox.Name = "InfoTextBox";
            this.InfoTextBox.ShortcutsEnabled = false;
            this.InfoTextBox.Size = new System.Drawing.Size(681, 228);
            this.InfoTextBox.TabIndex = 58;
            this.InfoTextBox.TabStop = false;
            this.InfoTextBox.TextChanged += new System.EventHandler(this.InfoTextBox_TextChanged);
            // 
            // BgmVolumeNumericUpDown
            // 
            this.BgmVolumeNumericUpDown.BackColor = System.Drawing.Color.Black;
            this.BgmVolumeNumericUpDown.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.BgmVolumeNumericUpDown.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.BgmVolumeNumericUpDown.ForeColor = System.Drawing.Color.Lime;
            this.BgmVolumeNumericUpDown.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.BgmVolumeNumericUpDown.Location = new System.Drawing.Point(156, 305);
            this.BgmVolumeNumericUpDown.Margin = new System.Windows.Forms.Padding(0);
            this.BgmVolumeNumericUpDown.Name = "BgmVolumeNumericUpDown";
            this.BgmVolumeNumericUpDown.Size = new System.Drawing.Size(259, 43);
            this.BgmVolumeNumericUpDown.TabIndex = 55;
            this.BgmVolumeNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.BgmVolumeNumericUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.BgmVolumeNumericUpDown.ValueChanged += new System.EventHandler(this.BgmVolumeNumericUpDown_ValueChanged);
            // 
            // BgmRpmNumericUpDown
            // 
            this.BgmRpmNumericUpDown.BackColor = System.Drawing.Color.Black;
            this.BgmRpmNumericUpDown.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.BgmRpmNumericUpDown.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.BgmRpmNumericUpDown.ForeColor = System.Drawing.Color.Lime;
            this.BgmRpmNumericUpDown.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.BgmRpmNumericUpDown.Location = new System.Drawing.Point(156, 131);
            this.BgmRpmNumericUpDown.Margin = new System.Windows.Forms.Padding(0);
            this.BgmRpmNumericUpDown.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.BgmRpmNumericUpDown.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.BgmRpmNumericUpDown.Name = "BgmRpmNumericUpDown";
            this.BgmRpmNumericUpDown.Size = new System.Drawing.Size(259, 43);
            this.BgmRpmNumericUpDown.TabIndex = 52;
            this.BgmRpmNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.BgmRpmNumericUpDown.ValueChanged += new System.EventHandler(this.BgmRpmNumericUpDown_ValueChanged);
            // 
            // BgmBpmNumericUpDown
            // 
            this.BgmBpmNumericUpDown.BackColor = System.Drawing.Color.Black;
            this.BgmBpmNumericUpDown.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.BgmBpmNumericUpDown.DecimalPlaces = 2;
            this.BgmBpmNumericUpDown.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.BgmBpmNumericUpDown.ForeColor = System.Drawing.Color.Lime;
            this.BgmBpmNumericUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.BgmBpmNumericUpDown.Location = new System.Drawing.Point(156, 217);
            this.BgmBpmNumericUpDown.Margin = new System.Windows.Forms.Padding(0);
            this.BgmBpmNumericUpDown.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.BgmBpmNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.BgmBpmNumericUpDown.Name = "BgmBpmNumericUpDown";
            this.BgmBpmNumericUpDown.Size = new System.Drawing.Size(259, 43);
            this.BgmBpmNumericUpDown.TabIndex = 51;
            this.BgmBpmNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.BgmBpmNumericUpDown.Value = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.BgmBpmNumericUpDown.ValueChanged += new System.EventHandler(this.BgmBpmNumericUpDown_ValueChanged);
            // 
            // Label0007
            // 
            this.Label0007.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Label0007.ForeColor = System.Drawing.Color.White;
            this.Label0007.Location = new System.Drawing.Point(75, 212);
            this.Label0007.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.Label0007.Name = "Label0007";
            this.Label0007.Size = new System.Drawing.Size(69, 56);
            this.Label0007.TabIndex = 53;
            this.Label0007.Text = "Bpm";
            this.Label0007.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label0013
            // 
            this.Label0013.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Label0013.ForeColor = System.Drawing.Color.White;
            this.Label0013.Location = new System.Drawing.Point(9, 116);
            this.Label0013.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.Label0013.Name = "Label0013";
            this.Label0013.Size = new System.Drawing.Size(135, 76);
            this.Label0013.TabIndex = 54;
            this.Label0013.Text = "  Bgm start time";
            this.Label0013.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // BgmFilePathTextBox
            // 
            this.BgmFilePathTextBox.BackColor = System.Drawing.Color.Black;
            this.BgmFilePathTextBox.CausesValidation = false;
            this.BgmFilePathTextBox.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.BgmFilePathTextBox.ForeColor = System.Drawing.Color.Lime;
            this.BgmFilePathTextBox.Location = new System.Drawing.Point(156, 26);
            this.BgmFilePathTextBox.Margin = new System.Windows.Forms.Padding(7, 10, 7, 10);
            this.BgmFilePathTextBox.Name = "BgmFilePathTextBox";
            this.BgmFilePathTextBox.Size = new System.Drawing.Size(1060, 43);
            this.BgmFilePathTextBox.TabIndex = 50;
            this.BgmFilePathTextBox.WordWrap = false;
            this.BgmFilePathTextBox.TextChanged += new System.EventHandler(this.BgmFilePathTextBox_TextChanged);
            this.BgmFilePathTextBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.BgmFilePathTextBox_MouseDoubleClick);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(43, 302);
            this.label4.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 52);
            this.label4.TabIndex = 57;
            this.label4.Text = "Volume";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(67, 22);
            this.label2.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 56);
            this.label2.TabIndex = 61;
            this.label2.Text = "Bgm";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(422, 131);
            this.label6.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 56);
            this.label6.TabIndex = 64;
            this.label6.Text = "ms";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ToolTip
            // 
            this.ToolTip.AutomaticDelay = 2000;
            // 
            // MusicControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.EqulizerGroupBox);
            this.Controls.Add(this.InfoTextBox);
            this.Controls.Add(this.BgmVolumeNumericUpDown);
            this.Controls.Add(this.BgmRpmNumericUpDown);
            this.Controls.Add(this.BgmBpmNumericUpDown);
            this.Controls.Add(this.Label0007);
            this.Controls.Add(this.Label0013);
            this.Controls.Add(this.BgmFilePathTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MusicControl";
            this.Size = new System.Drawing.Size(1245, 1105);
            this.EqulizerGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BgmVolumeNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BgmRpmNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BgmBpmNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox EqulizerGroupBox;
        private System.Windows.Forms.CheckBox EqualizerOnCheckBox;
        private System.Windows.Forms.TextBox InfoTextBox;
        private System.Windows.Forms.NumericUpDown BgmVolumeNumericUpDown;
        private System.Windows.Forms.NumericUpDown BgmRpmNumericUpDown;
        private System.Windows.Forms.NumericUpDown BgmBpmNumericUpDown;
        private System.Windows.Forms.Label Label0007;
        private System.Windows.Forms.Label Label0013;
        private System.Windows.Forms.TextBox BgmFilePathTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button EqualizerResetButton;
        private System.Windows.Forms.Label label2;
        private DrumMidiEditor.pView.pEditer.pMusic.EqualizerPanel EqualizerPanel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox WaveOnCheckBox;
        private System.Windows.Forms.ToolTip ToolTip;
    }
}
