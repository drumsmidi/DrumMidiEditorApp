namespace DrumMidiEditor.pView.pEditer.pScore
{
	partial class ScoreControl
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
            this.ScorePanel = new DrumMidiEditor.pView.pEditer.pScore.ScorePanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.VolumeZeroCheckBox = new System.Windows.Forms.CheckBox();
            this.VolumeSizeCheckBox = new System.Windows.Forms.CheckBox();
            this.NoteWidthNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.NoteHeightNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NoteWidthNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NoteHeightNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // ScorePanel
            // 
            this.ScorePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ScorePanel.Location = new System.Drawing.Point(136, 0);
            this.ScorePanel.Margin = new System.Windows.Forms.Padding(0);
            this.ScorePanel.Name = "ScorePanel";
            this.ScorePanel.Size = new System.Drawing.Size(979, 850);
            this.ScorePanel.TabIndex = 0;
            this.ScorePanel.Resize += new System.EventHandler(this.ScorePanel_Resize);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(136, 850);
            this.panel1.TabIndex = 0;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.VolumeZeroCheckBox);
            this.groupBox5.Controls.Add(this.VolumeSizeCheckBox);
            this.groupBox5.Controls.Add(this.NoteWidthNumericUpDown);
            this.groupBox5.Controls.Add(this.NoteHeightNumericUpDown);
            this.groupBox5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox5.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.groupBox5.ForeColor = System.Drawing.Color.White;
            this.groupBox5.Location = new System.Drawing.Point(0, 2);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox5.Size = new System.Drawing.Size(132, 141);
            this.groupBox5.TabIndex = 35;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Note";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label2.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(6, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 45);
            this.label2.TabIndex = 16;
            this.label2.Text = "Note Size";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // VolumeZeroCheckBox
            // 
            this.VolumeZeroCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.VolumeZeroCheckBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.VolumeZeroCheckBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.VolumeZeroCheckBox.FlatAppearance.CheckedBackColor = System.Drawing.Color.DarkSlateGray;
            this.VolumeZeroCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VolumeZeroCheckBox.Font = new System.Drawing.Font("Meiryo UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.VolumeZeroCheckBox.ForeColor = System.Drawing.Color.White;
            this.VolumeZeroCheckBox.Location = new System.Drawing.Point(6, 18);
            this.VolumeZeroCheckBox.Margin = new System.Windows.Forms.Padding(0);
            this.VolumeZeroCheckBox.Name = "VolumeZeroCheckBox";
            this.VolumeZeroCheckBox.Size = new System.Drawing.Size(53, 40);
            this.VolumeZeroCheckBox.TabIndex = 26;
            this.VolumeZeroCheckBox.Text = "Vol Zero";
            this.VolumeZeroCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.VolumeZeroCheckBox.UseVisualStyleBackColor = false;
            this.VolumeZeroCheckBox.CheckedChanged += new System.EventHandler(this.VolumeZeroCheckBox_CheckedChanged);
            // 
            // VolumeSizeCheckBox
            // 
            this.VolumeSizeCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.VolumeSizeCheckBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.VolumeSizeCheckBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.VolumeSizeCheckBox.FlatAppearance.CheckedBackColor = System.Drawing.Color.DarkSlateGray;
            this.VolumeSizeCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VolumeSizeCheckBox.Font = new System.Drawing.Font("Meiryo UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.VolumeSizeCheckBox.ForeColor = System.Drawing.Color.White;
            this.VolumeSizeCheckBox.Location = new System.Drawing.Point(66, 18);
            this.VolumeSizeCheckBox.Margin = new System.Windows.Forms.Padding(0);
            this.VolumeSizeCheckBox.Name = "VolumeSizeCheckBox";
            this.VolumeSizeCheckBox.Size = new System.Drawing.Size(53, 40);
            this.VolumeSizeCheckBox.TabIndex = 15;
            this.VolumeSizeCheckBox.Text = "Vol Size";
            this.VolumeSizeCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.VolumeSizeCheckBox.UseVisualStyleBackColor = false;
            this.VolumeSizeCheckBox.CheckedChanged += new System.EventHandler(this.VolumeSizeCheckBox_CheckedChanged);
            // 
            // NoteWidthNumericUpDown
            // 
            this.NoteWidthNumericUpDown.BackColor = System.Drawing.Color.Black;
            this.NoteWidthNumericUpDown.ForeColor = System.Drawing.Color.Lime;
            this.NoteWidthNumericUpDown.Location = new System.Drawing.Point(66, 65);
            this.NoteWidthNumericUpDown.Margin = new System.Windows.Forms.Padding(0);
            this.NoteWidthNumericUpDown.Maximum = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.NoteWidthNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NoteWidthNumericUpDown.Name = "NoteWidthNumericUpDown";
            this.NoteWidthNumericUpDown.Size = new System.Drawing.Size(53, 34);
            this.NoteWidthNumericUpDown.TabIndex = 15;
            this.NoteWidthNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NoteWidthNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NoteWidthNumericUpDown.ValueChanged += new System.EventHandler(this.NoteWidthNumericUpDown_ValueChanged);
            // 
            // NoteHeightNumericUpDown
            // 
            this.NoteHeightNumericUpDown.BackColor = System.Drawing.Color.Black;
            this.NoteHeightNumericUpDown.ForeColor = System.Drawing.Color.Lime;
            this.NoteHeightNumericUpDown.Location = new System.Drawing.Point(66, 87);
            this.NoteHeightNumericUpDown.Margin = new System.Windows.Forms.Padding(0);
            this.NoteHeightNumericUpDown.Maximum = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.NoteHeightNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NoteHeightNumericUpDown.Name = "NoteHeightNumericUpDown";
            this.NoteHeightNumericUpDown.Size = new System.Drawing.Size(53, 34);
            this.NoteHeightNumericUpDown.TabIndex = 4;
            this.NoteHeightNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NoteHeightNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NoteHeightNumericUpDown.ValueChanged += new System.EventHandler(this.NoteHeightNumericUpDown_ValueChanged);
            // 
            // ToolTip
            // 
            this.ToolTip.AutomaticDelay = 2000;
            // 
            // ScoreControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(168F, 168F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.Controls.Add(this.ScorePanel);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ScoreControl";
            this.Size = new System.Drawing.Size(1115, 850);
            this.panel1.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.NoteWidthNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NoteHeightNumericUpDown)).EndInit();
            this.ResumeLayout(false);

		}

        #endregion

        private ScorePanel ScorePanel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox VolumeZeroCheckBox;
        private System.Windows.Forms.CheckBox VolumeSizeCheckBox;
        private System.Windows.Forms.NumericUpDown NoteWidthNumericUpDown;
        private System.Windows.Forms.NumericUpDown NoteHeightNumericUpDown;
        private System.Windows.Forms.ToolTip ToolTip;
    }
}
