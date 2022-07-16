namespace DrumMidiEditor.pView.pEditer.pPlay
{
	partial class PlayerControl
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
            this.MainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.MainToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ExportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OutputMidiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OutputVideoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OutputTechManiaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.ImportToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ImportMidiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.MeasureConnectCheckBox = new System.Windows.Forms.CheckBox();
            this.PlayEndMeasureNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.MeasureConnectNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.PlayStartMeasureNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.RangePlayButton = new System.Windows.Forms.Button();
            this.PlayButton = new System.Windows.Forms.Button();
            this.StopButton = new System.Windows.Forms.Button();
            this.PlayerConfigCheckBox = new System.Windows.Forms.CheckBox();
            this.PlayerCheckBox = new System.Windows.Forms.CheckBox();
            this.NoteCheckBox = new System.Windows.Forms.CheckBox();
            this.BgmCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ChannelComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.MainMenuStrip.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PlayEndMeasureNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MeasureConnectNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlayStartMeasureNumericUpDown)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenuStrip
            // 
            this.MainMenuStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.MainMenuStrip.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MainMenuStrip.GripMargin = new System.Windows.Forms.Padding(0);
            this.MainMenuStrip.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.MainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MainToolStripMenuItem});
            this.MainMenuStrip.Location = new System.Drawing.Point(1, 0);
            this.MainMenuStrip.Name = "MainMenuStrip";
            this.MainMenuStrip.Padding = new System.Windows.Forms.Padding(0);
            this.MainMenuStrip.Size = new System.Drawing.Size(82, 35);
            this.MainMenuStrip.TabIndex = 49;
            this.MainMenuStrip.Text = "MenuStrip";
            // 
            // MainToolStripMenuItem
            // 
            this.MainToolStripMenuItem.AutoSize = false;
            this.MainToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.MainToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewToolStripMenuItem,
            this.OpenToolStripMenuItem,
            this.SaveToolStripMenuItem,
            this.SaveAsToolStripMenuItem,
            this.toolStripSeparator3,
            this.ExportToolStripMenuItem,
            this.toolStripSeparator4,
            this.ImportToolStripMenuItem1,
            this.toolStripSeparator1});
            this.MainToolStripMenuItem.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.MainToolStripMenuItem.Name = "MainToolStripMenuItem";
            this.MainToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0);
            this.MainToolStripMenuItem.Size = new System.Drawing.Size(80, 35);
            this.MainToolStripMenuItem.Text = "Menu";
            this.MainToolStripMenuItem.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // NewToolStripMenuItem
            // 
            this.NewToolStripMenuItem.Name = "NewToolStripMenuItem";
            this.NewToolStripMenuItem.Size = new System.Drawing.Size(209, 38);
            this.NewToolStripMenuItem.Text = "New";
            this.NewToolStripMenuItem.Click += new System.EventHandler(this.NewFileToolStripMenuItem_Click);
            // 
            // OpenToolStripMenuItem
            // 
            this.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem";
            this.OpenToolStripMenuItem.Size = new System.Drawing.Size(209, 38);
            this.OpenToolStripMenuItem.Text = "Open";
            this.OpenToolStripMenuItem.Click += new System.EventHandler(this.OpenFileToolStripMenuItem_Click);
            // 
            // SaveToolStripMenuItem
            // 
            this.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem";
            this.SaveToolStripMenuItem.Size = new System.Drawing.Size(209, 38);
            this.SaveToolStripMenuItem.Text = "Save";
            this.SaveToolStripMenuItem.Click += new System.EventHandler(this.SaveFileToolStripMenuItem_Click);
            // 
            // SaveAsToolStripMenuItem
            // 
            this.SaveAsToolStripMenuItem.Name = "SaveAsToolStripMenuItem";
            this.SaveAsToolStripMenuItem.Size = new System.Drawing.Size(209, 38);
            this.SaveAsToolStripMenuItem.Text = "Save As";
            this.SaveAsToolStripMenuItem.Click += new System.EventHandler(this.SaveAsFileToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(206, 6);
            // 
            // ExportToolStripMenuItem
            // 
            this.ExportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OutputMidiToolStripMenuItem,
            this.OutputVideoToolStripMenuItem,
            this.OutputTechManiaToolStripMenuItem});
            this.ExportToolStripMenuItem.Name = "ExportToolStripMenuItem";
            this.ExportToolStripMenuItem.Size = new System.Drawing.Size(209, 38);
            this.ExportToolStripMenuItem.Text = "Export";
            // 
            // OutputMidiToolStripMenuItem
            // 
            this.OutputMidiToolStripMenuItem.Name = "OutputMidiToolStripMenuItem";
            this.OutputMidiToolStripMenuItem.Size = new System.Drawing.Size(238, 38);
            this.OutputMidiToolStripMenuItem.Text = "Midi";
            this.OutputMidiToolStripMenuItem.Click += new System.EventHandler(this.OutputMidiToolStripMenuItem_Click);
            // 
            // OutputVideoToolStripMenuItem
            // 
            this.OutputVideoToolStripMenuItem.Name = "OutputVideoToolStripMenuItem";
            this.OutputVideoToolStripMenuItem.Size = new System.Drawing.Size(238, 38);
            this.OutputVideoToolStripMenuItem.Text = "Video";
            this.OutputVideoToolStripMenuItem.Click += new System.EventHandler(this.OutputVideoToolStripMenuItem_Click);
            // 
            // OutputTechManiaToolStripMenuItem
            // 
            this.OutputTechManiaToolStripMenuItem.Enabled = false;
            this.OutputTechManiaToolStripMenuItem.Name = "OutputTechManiaToolStripMenuItem";
            this.OutputTechManiaToolStripMenuItem.Size = new System.Drawing.Size(238, 38);
            this.OutputTechManiaToolStripMenuItem.Text = "TechMania";
            this.OutputTechManiaToolStripMenuItem.Click += new System.EventHandler(this.OutputTechManiaToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(206, 6);
            // 
            // ImportToolStripMenuItem1
            // 
            this.ImportToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ImportMidiToolStripMenuItem});
            this.ImportToolStripMenuItem1.Name = "ImportToolStripMenuItem1";
            this.ImportToolStripMenuItem1.Size = new System.Drawing.Size(209, 38);
            this.ImportToolStripMenuItem1.Text = "Import";
            // 
            // ImportMidiToolStripMenuItem
            // 
            this.ImportMidiToolStripMenuItem.Enabled = false;
            this.ImportMidiToolStripMenuItem.Name = "ImportMidiToolStripMenuItem";
            this.ImportMidiToolStripMenuItem.Size = new System.Drawing.Size(163, 38);
            this.ImportMidiToolStripMenuItem.Text = "Midi";
            this.ImportMidiToolStripMenuItem.Click += new System.EventHandler(this.ImportMidiToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(206, 6);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.MeasureConnectCheckBox);
            this.groupBox1.Controls.Add(this.PlayEndMeasureNumericUpDown);
            this.groupBox1.Controls.Add(this.MeasureConnectNumericUpDown);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.PlayStartMeasureNumericUpDown);
            this.groupBox1.Controls.Add(this.RangePlayButton);
            this.groupBox1.Controls.Add(this.PlayButton);
            this.groupBox1.Controls.Add(this.StopButton);
            this.groupBox1.Font = new System.Drawing.Font("MS UI Gothic", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.groupBox1.ForeColor = System.Drawing.Color.Black;
            this.groupBox1.Location = new System.Drawing.Point(269, -6);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(0);
            this.groupBox1.Size = new System.Drawing.Size(404, 42);
            this.groupBox1.TabIndex = 47;
            this.groupBox1.TabStop = false;
            // 
            // MeasureConnectCheckBox
            // 
            this.MeasureConnectCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.MeasureConnectCheckBox.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.MeasureConnectCheckBox.BackgroundImage = global::DrumMidiEditor.Properties.Resources.ic_repeat_black_48dp;
            this.MeasureConnectCheckBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.MeasureConnectCheckBox.Checked = true;
            this.MeasureConnectCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MeasureConnectCheckBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.MeasureConnectCheckBox.FlatAppearance.CheckedBackColor = System.Drawing.Color.SeaGreen;
            this.MeasureConnectCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MeasureConnectCheckBox.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MeasureConnectCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.MeasureConnectCheckBox.Location = new System.Drawing.Point(295, 6);
            this.MeasureConnectCheckBox.Margin = new System.Windows.Forms.Padding(0);
            this.MeasureConnectCheckBox.Name = "MeasureConnectCheckBox";
            this.MeasureConnectCheckBox.Size = new System.Drawing.Size(34, 34);
            this.MeasureConnectCheckBox.TabIndex = 52;
            this.MeasureConnectCheckBox.TabStop = false;
            this.MeasureConnectCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.MeasureConnectCheckBox.UseVisualStyleBackColor = false;
            this.MeasureConnectCheckBox.CheckedChanged += new System.EventHandler(this.MeasureConnectCheckBox_CheckedChanged);
            // 
            // PlayEndMeasureNumericUpDown
            // 
            this.PlayEndMeasureNumericUpDown.BackColor = System.Drawing.Color.Black;
            this.PlayEndMeasureNumericUpDown.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.PlayEndMeasureNumericUpDown.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.PlayEndMeasureNumericUpDown.ForeColor = System.Drawing.Color.Lime;
            this.PlayEndMeasureNumericUpDown.Location = new System.Drawing.Point(226, 9);
            this.PlayEndMeasureNumericUpDown.Margin = new System.Windows.Forms.Padding(0);
            this.PlayEndMeasureNumericUpDown.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.PlayEndMeasureNumericUpDown.Name = "PlayEndMeasureNumericUpDown";
            this.PlayEndMeasureNumericUpDown.Size = new System.Drawing.Size(64, 38);
            this.PlayEndMeasureNumericUpDown.TabIndex = 30;
            this.PlayEndMeasureNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.PlayEndMeasureNumericUpDown.ValueChanged += new System.EventHandler(this.PlayEndMeasureNumericUpDown_ValueChanged);
            // 
            // MeasureConnectNumericUpDown
            // 
            this.MeasureConnectNumericUpDown.BackColor = System.Drawing.Color.Black;
            this.MeasureConnectNumericUpDown.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.MeasureConnectNumericUpDown.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MeasureConnectNumericUpDown.ForeColor = System.Drawing.Color.Lime;
            this.MeasureConnectNumericUpDown.Location = new System.Drawing.Point(333, 9);
            this.MeasureConnectNumericUpDown.Margin = new System.Windows.Forms.Padding(0);
            this.MeasureConnectNumericUpDown.Name = "MeasureConnectNumericUpDown";
            this.MeasureConnectNumericUpDown.Size = new System.Drawing.Size(64, 38);
            this.MeasureConnectNumericUpDown.TabIndex = 31;
            this.MeasureConnectNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.MeasureConnectNumericUpDown.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.MeasureConnectNumericUpDown.ValueChanged += new System.EventHandler(this.MeasureConnectNumericUpDown_ValueChanged);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(193, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 26);
            this.label2.TabIndex = 34;
            this.label2.Text = "━";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PlayStartMeasureNumericUpDown
            // 
            this.PlayStartMeasureNumericUpDown.BackColor = System.Drawing.Color.Black;
            this.PlayStartMeasureNumericUpDown.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.PlayStartMeasureNumericUpDown.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.PlayStartMeasureNumericUpDown.ForeColor = System.Drawing.Color.Lime;
            this.PlayStartMeasureNumericUpDown.Location = new System.Drawing.Point(129, 9);
            this.PlayStartMeasureNumericUpDown.Margin = new System.Windows.Forms.Padding(0);
            this.PlayStartMeasureNumericUpDown.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.PlayStartMeasureNumericUpDown.Name = "PlayStartMeasureNumericUpDown";
            this.PlayStartMeasureNumericUpDown.Size = new System.Drawing.Size(64, 38);
            this.PlayStartMeasureNumericUpDown.TabIndex = 30;
            this.PlayStartMeasureNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.PlayStartMeasureNumericUpDown.ValueChanged += new System.EventHandler(this.PlayStartMeasureNumericUpDown_ValueChanged);
            // 
            // RangePlayButton
            // 
            this.RangePlayButton.BackColor = System.Drawing.Color.DodgerBlue;
            this.RangePlayButton.BackgroundImage = global::DrumMidiEditor.Properties.Resources.ic_slow_motion_video_black_48dp;
            this.RangePlayButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.RangePlayButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.RangePlayButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RangePlayButton.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.RangePlayButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.RangePlayButton.Location = new System.Drawing.Point(85, 6);
            this.RangePlayButton.Margin = new System.Windows.Forms.Padding(0);
            this.RangePlayButton.Name = "RangePlayButton";
            this.RangePlayButton.Size = new System.Drawing.Size(34, 34);
            this.RangePlayButton.TabIndex = 29;
            this.RangePlayButton.TabStop = false;
            this.RangePlayButton.UseVisualStyleBackColor = false;
            this.RangePlayButton.Click += new System.EventHandler(this.RangePlayButton_Click);
            // 
            // PlayButton
            // 
            this.PlayButton.BackColor = System.Drawing.Color.DodgerBlue;
            this.PlayButton.BackgroundImage = global::DrumMidiEditor.Properties.Resources.ic_play_arrow_black_48dp;
            this.PlayButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.PlayButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PlayButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PlayButton.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.PlayButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.PlayButton.Location = new System.Drawing.Point(7, 6);
            this.PlayButton.Margin = new System.Windows.Forms.Padding(0);
            this.PlayButton.Name = "PlayButton";
            this.PlayButton.Size = new System.Drawing.Size(34, 34);
            this.PlayButton.TabIndex = 21;
            this.PlayButton.TabStop = false;
            this.PlayButton.UseVisualStyleBackColor = false;
            this.PlayButton.Click += new System.EventHandler(this.PlayButton_Click);
            // 
            // StopButton
            // 
            this.StopButton.BackColor = System.Drawing.Color.DodgerBlue;
            this.StopButton.BackgroundImage = global::DrumMidiEditor.Properties.Resources.ic_stop_black_48dp;
            this.StopButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.StopButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.StopButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StopButton.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.StopButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.StopButton.Location = new System.Drawing.Point(46, 6);
            this.StopButton.Margin = new System.Windows.Forms.Padding(0);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(34, 34);
            this.StopButton.TabIndex = 29;
            this.StopButton.TabStop = false;
            this.StopButton.UseVisualStyleBackColor = false;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // PlayerConfigCheckBox
            // 
            this.PlayerConfigCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.PlayerConfigCheckBox.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.PlayerConfigCheckBox.BackgroundImage = global::DrumMidiEditor.Properties.Resources.ic_settings_applications_black_48dp;
            this.PlayerConfigCheckBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.PlayerConfigCheckBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PlayerConfigCheckBox.FlatAppearance.CheckedBackColor = System.Drawing.Color.SeaGreen;
            this.PlayerConfigCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PlayerConfigCheckBox.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.PlayerConfigCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.PlayerConfigCheckBox.Location = new System.Drawing.Point(45, 6);
            this.PlayerConfigCheckBox.Margin = new System.Windows.Forms.Padding(0);
            this.PlayerConfigCheckBox.Name = "PlayerConfigCheckBox";
            this.PlayerConfigCheckBox.Size = new System.Drawing.Size(34, 34);
            this.PlayerConfigCheckBox.TabIndex = 51;
            this.PlayerConfigCheckBox.TabStop = false;
            this.PlayerConfigCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.PlayerConfigCheckBox.UseVisualStyleBackColor = false;
            this.PlayerConfigCheckBox.CheckedChanged += new System.EventHandler(this.PlayerConfigCheckBox_CheckedChanged);
            // 
            // PlayerCheckBox
            // 
            this.PlayerCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.PlayerCheckBox.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.PlayerCheckBox.BackgroundImage = global::DrumMidiEditor.Properties.Resources.ic_stay_current_landscape_black_48dp;
            this.PlayerCheckBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.PlayerCheckBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PlayerCheckBox.FlatAppearance.CheckedBackColor = System.Drawing.Color.SeaGreen;
            this.PlayerCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PlayerCheckBox.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.PlayerCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.PlayerCheckBox.Location = new System.Drawing.Point(7, 6);
            this.PlayerCheckBox.Margin = new System.Windows.Forms.Padding(0);
            this.PlayerCheckBox.Name = "PlayerCheckBox";
            this.PlayerCheckBox.Size = new System.Drawing.Size(34, 34);
            this.PlayerCheckBox.TabIndex = 50;
            this.PlayerCheckBox.TabStop = false;
            this.PlayerCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.PlayerCheckBox.UseVisualStyleBackColor = false;
            this.PlayerCheckBox.CheckedChanged += new System.EventHandler(this.PlayerCheckBox_CheckedChanged);
            // 
            // NoteCheckBox
            // 
            this.NoteCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.NoteCheckBox.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.NoteCheckBox.BackgroundImage = global::DrumMidiEditor.Properties.Resources.ic_settings_input_svideo_black_48dp;
            this.NoteCheckBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.NoteCheckBox.Checked = true;
            this.NoteCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.NoteCheckBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.NoteCheckBox.FlatAppearance.CheckedBackColor = System.Drawing.Color.SeaGreen;
            this.NoteCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.NoteCheckBox.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.NoteCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.NoteCheckBox.Location = new System.Drawing.Point(44, 6);
            this.NoteCheckBox.Margin = new System.Windows.Forms.Padding(0);
            this.NoteCheckBox.Name = "NoteCheckBox";
            this.NoteCheckBox.Size = new System.Drawing.Size(34, 34);
            this.NoteCheckBox.TabIndex = 52;
            this.NoteCheckBox.TabStop = false;
            this.NoteCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.NoteCheckBox.UseVisualStyleBackColor = false;
            this.NoteCheckBox.CheckedChanged += new System.EventHandler(this.NoteCheckBox_CheckedChanged);
            // 
            // BgmCheckBox
            // 
            this.BgmCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.BgmCheckBox.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.BgmCheckBox.BackgroundImage = global::DrumMidiEditor.Properties.Resources.ic_volume_down_black_48dp;
            this.BgmCheckBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.BgmCheckBox.Checked = true;
            this.BgmCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.BgmCheckBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BgmCheckBox.FlatAppearance.CheckedBackColor = System.Drawing.Color.SeaGreen;
            this.BgmCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BgmCheckBox.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.BgmCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.BgmCheckBox.Location = new System.Drawing.Point(6, 6);
            this.BgmCheckBox.Margin = new System.Windows.Forms.Padding(0);
            this.BgmCheckBox.Name = "BgmCheckBox";
            this.BgmCheckBox.Size = new System.Drawing.Size(34, 34);
            this.BgmCheckBox.TabIndex = 51;
            this.BgmCheckBox.TabStop = false;
            this.BgmCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.BgmCheckBox.UseVisualStyleBackColor = false;
            this.BgmCheckBox.CheckedChanged += new System.EventHandler(this.BgmCheckBox_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.ChannelComboBox);
            this.groupBox2.Font = new System.Drawing.Font("MS UI Gothic", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.groupBox2.ForeColor = System.Drawing.Color.Black;
            this.groupBox2.Location = new System.Drawing.Point(74, -6);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(0);
            this.groupBox2.Size = new System.Drawing.Size(111, 42);
            this.groupBox2.TabIndex = 50;
            this.groupBox2.TabStop = false;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(72, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 26);
            this.label1.TabIndex = 51;
            this.label1.Text = "Ch";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ChannelComboBox
            // 
            this.ChannelComboBox.BackColor = System.Drawing.Color.Black;
            this.ChannelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ChannelComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ChannelComboBox.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ChannelComboBox.ForeColor = System.Drawing.Color.Lime;
            this.ChannelComboBox.FormatString = "N0";
            this.ChannelComboBox.Location = new System.Drawing.Point(11, 9);
            this.ChannelComboBox.Margin = new System.Windows.Forms.Padding(0);
            this.ChannelComboBox.Name = "ChannelComboBox";
            this.ChannelComboBox.Size = new System.Drawing.Size(59, 38);
            this.ChannelComboBox.TabIndex = 51;
            this.ChannelComboBox.TabStop = false;
            this.ChannelComboBox.SelectedIndexChanged += new System.EventHandler(this.ChannelComboBox_SelectedIndexChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.BgmCheckBox);
            this.groupBox3.Controls.Add(this.NoteCheckBox);
            this.groupBox3.Font = new System.Drawing.Font("MS UI Gothic", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.groupBox3.ForeColor = System.Drawing.Color.Black;
            this.groupBox3.Location = new System.Drawing.Point(184, -6);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(0);
            this.groupBox3.Size = new System.Drawing.Size(86, 42);
            this.groupBox3.TabIndex = 51;
            this.groupBox3.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.PlayerConfigCheckBox);
            this.groupBox4.Controls.Add(this.PlayerCheckBox);
            this.groupBox4.Font = new System.Drawing.Font("MS UI Gothic", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.groupBox4.ForeColor = System.Drawing.Color.Black;
            this.groupBox4.Location = new System.Drawing.Point(672, -6);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(0);
            this.groupBox4.Size = new System.Drawing.Size(86, 42);
            this.groupBox4.TabIndex = 52;
            this.groupBox4.TabStop = false;
            // 
            // ToolTip
            // 
            this.ToolTip.AutomaticDelay = 2000;
            // 
            // PlayerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.MainMenuStrip);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "PlayerControl";
            this.Size = new System.Drawing.Size(777, 40);
            this.MainMenuStrip.ResumeLayout(false);
            this.MainMenuStrip.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PlayEndMeasureNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MeasureConnectNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlayStartMeasureNumericUpDown)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.MenuStrip MainMenuStrip;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.NumericUpDown PlayEndMeasureNumericUpDown;
		private System.Windows.Forms.NumericUpDown MeasureConnectNumericUpDown;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown PlayStartMeasureNumericUpDown;
		private System.Windows.Forms.Button RangePlayButton;
		private System.Windows.Forms.Button PlayButton;
		private System.Windows.Forms.Button StopButton;
		private System.Windows.Forms.ToolStripMenuItem MainToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem NewToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem SaveToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem SaveAsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem OpenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OutputMidiToolStripMenuItem;
        private System.Windows.Forms.CheckBox PlayerCheckBox;
        private System.Windows.Forms.CheckBox PlayerConfigCheckBox;
        private System.Windows.Forms.CheckBox MeasureConnectCheckBox;
        private System.Windows.Forms.CheckBox NoteCheckBox;
        private System.Windows.Forms.CheckBox BgmCheckBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem OutputVideoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem ImportToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ImportMidiToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox ChannelComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ToolTip ToolTip;
        private System.Windows.Forms.ToolStripMenuItem OutputTechManiaToolStripMenuItem;
    }
}
