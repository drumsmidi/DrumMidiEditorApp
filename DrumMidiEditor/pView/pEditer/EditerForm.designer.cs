namespace DrumMidiEditor.pView.pEditer
{
    partial class EditerForm
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

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditerForm));
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.MidiMapSetTabPage = new System.Windows.Forms.TabPage();
            this.MidiMapSetControl = new DrumMidiEditor.pView.pEditer.pMidiMapSet.MidiMapSetControl();
            this.EditTabPage = new System.Windows.Forms.TabPage();
            this.EditerControl = new DrumMidiEditor.pView.pEditer.pEdit.EditerControl();
            this.MainTabControl = new System.Windows.Forms.TabControl();
            this.ConfigTabPage = new System.Windows.Forms.TabPage();
            this.configControl1 = new DrumMidiEditor.pView.pEditer.pConfig.ConfigControl();
            this.MusicTabPage = new System.Windows.Forms.TabPage();
            this.MusicControl = new DrumMidiEditor.pView.pEditer.pMusic.MusicControl();
            this.ScoreTabPage = new System.Windows.Forms.TabPage();
            this.ScoreControl = new DrumMidiEditor.pView.pEditer.pScore.ScoreControl();
            this.MainTabImageList = new System.Windows.Forms.ImageList(this.components);
            this.PlayerControl = new DrumMidiEditor.pView.pEditer.pPlay.PlayerControl();
            this.MainStatusStrip = new System.Windows.Forms.StatusStrip();
            this.LogToolStripSplitButton = new System.Windows.Forms.ToolStripSplitButton();
            this.MainToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.MidiMapSetTabPage.SuspendLayout();
            this.EditTabPage.SuspendLayout();
            this.MainTabControl.SuspendLayout();
            this.ConfigTabPage.SuspendLayout();
            this.MusicTabPage.SuspendLayout();
            this.ScoreTabPage.SuspendLayout();
            this.MainStatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.BackColor = System.Drawing.Color.Black;
            this.tableLayoutPanel4.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel4.ColumnCount = 4;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 83F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 218F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel5, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.label8, 0, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(200, 100);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.BackColor = System.Drawing.Color.Black;
            this.tableLayoutPanel5.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel5.ColumnCount = 4;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 83F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 218F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel5.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.label6, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.textBox4, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.textBox5, 1, 1);
            this.tableLayoutPanel5.Controls.Add(this.label7, 2, 0);
            this.tableLayoutPanel5.Controls.Add(this.textBox6, 3, 0);
            this.tableLayoutPanel5.ForeColor = System.Drawing.Color.White;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(1, 22);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(83, 77);
            this.tableLayoutPanel5.TabIndex = 21;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(4, 1);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 25);
            this.label5.TabIndex = 8;
            this.label5.Text = "Title";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 39);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 50);
            this.label6.TabIndex = 10;
            this.label6.Text = "Comment";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox4
            // 
            this.textBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox4.BackColor = System.Drawing.Color.Black;
            this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox4.ForeColor = System.Drawing.Color.White;
            this.textBox4.Location = new System.Drawing.Point(88, 4);
            this.textBox4.MaxLength = 512;
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(212, 31);
            this.textBox4.TabIndex = 4;
            // 
            // textBox5
            // 
            this.textBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox5.BackColor = System.Drawing.Color.Black;
            this.textBox5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox5.ForeColor = System.Drawing.Color.White;
            this.textBox5.Location = new System.Drawing.Point(88, 42);
            this.textBox5.MaxLength = 1024;
            this.textBox5.Multiline = true;
            this.textBox5.Name = "textBox5";
            this.textBox5.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox5.Size = new System.Drawing.Size(212, 80);
            this.textBox5.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(307, 1);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 25);
            this.label7.TabIndex = 9;
            this.label7.Text = "Artist";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox6
            // 
            this.textBox6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox6.BackColor = System.Drawing.Color.Black;
            this.textBox6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox6.ForeColor = System.Drawing.Color.White;
            this.textBox6.Location = new System.Drawing.Point(370, 4);
            this.textBox6.MaxLength = 512;
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(2, 31);
            this.textBox6.TabIndex = 5;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(4, 1);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 20);
            this.label8.TabIndex = 8;
            this.label8.Text = "Title";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(88, 27);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 10;
            this.label9.Text = "Comment";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.BackColor = System.Drawing.Color.Black;
            this.tableLayoutPanel6.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel6.ColumnCount = 1;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel6.ForeColor = System.Drawing.Color.White;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 2;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(200, 100);
            this.tableLayoutPanel6.TabIndex = 0;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.BackColor = System.Drawing.Color.Black;
            this.tableLayoutPanel7.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel7.ColumnCount = 1;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel7.ForeColor = System.Drawing.Color.White;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel7.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 2;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(200, 100);
            this.tableLayoutPanel7.TabIndex = 0;
            // 
            // MidiMapSetTabPage
            // 
            this.MidiMapSetTabPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.MidiMapSetTabPage.Controls.Add(this.MidiMapSetControl);
            this.MidiMapSetTabPage.ImageKey = "ic_straighten_black_48dp.png";
            this.MidiMapSetTabPage.Location = new System.Drawing.Point(34, 4);
            this.MidiMapSetTabPage.Margin = new System.Windows.Forms.Padding(0);
            this.MidiMapSetTabPage.Name = "MidiMapSetTabPage";
            this.MidiMapSetTabPage.Size = new System.Drawing.Size(1205, 923);
            this.MidiMapSetTabPage.TabIndex = 2;
            // 
            // MidiMapSetControl
            // 
            this.MidiMapSetControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.MidiMapSetControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MidiMapSetControl.Location = new System.Drawing.Point(0, 0);
            this.MidiMapSetControl.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.MidiMapSetControl.Name = "MidiMapSetControl";
            this.MidiMapSetControl.Size = new System.Drawing.Size(1205, 923);
            this.MidiMapSetControl.TabIndex = 0;
            // 
            // EditTabPage
            // 
            this.EditTabPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.EditTabPage.Controls.Add(this.EditerControl);
            this.EditTabPage.ImageKey = "ic_border_color_black_48dp.png";
            this.EditTabPage.Location = new System.Drawing.Point(34, 4);
            this.EditTabPage.Margin = new System.Windows.Forms.Padding(0);
            this.EditTabPage.Name = "EditTabPage";
            this.EditTabPage.Size = new System.Drawing.Size(1205, 923);
            this.EditTabPage.TabIndex = 1;
            // 
            // EditerControl
            // 
            this.EditerControl.BackColor = System.Drawing.Color.LightSlateGray;
            this.EditerControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EditerControl.Location = new System.Drawing.Point(0, 0);
            this.EditerControl.Margin = new System.Windows.Forms.Padding(0);
            this.EditerControl.Name = "EditerControl";
            this.EditerControl.Size = new System.Drawing.Size(1205, 923);
            this.EditerControl.TabIndex = 0;
            // 
            // MainTabControl
            // 
            this.MainTabControl.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.MainTabControl.Controls.Add(this.ConfigTabPage);
            this.MainTabControl.Controls.Add(this.MusicTabPage);
            this.MainTabControl.Controls.Add(this.MidiMapSetTabPage);
            this.MainTabControl.Controls.Add(this.EditTabPage);
            this.MainTabControl.Controls.Add(this.ScoreTabPage);
            this.MainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTabControl.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.MainTabControl.ImageList = this.MainTabImageList;
            this.MainTabControl.Location = new System.Drawing.Point(0, 60);
            this.MainTabControl.Margin = new System.Windows.Forms.Padding(0);
            this.MainTabControl.Multiline = true;
            this.MainTabControl.Name = "MainTabControl";
            this.MainTabControl.Padding = new System.Drawing.Point(0, 0);
            this.MainTabControl.SelectedIndex = 0;
            this.MainTabControl.Size = new System.Drawing.Size(1243, 926);
            this.MainTabControl.TabIndex = 35;
            this.MainTabControl.TabStop = false;
            // 
            // ConfigTabPage
            // 
            this.ConfigTabPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.ConfigTabPage.Controls.Add(this.configControl1);
            this.ConfigTabPage.ForeColor = System.Drawing.Color.White;
            this.ConfigTabPage.ImageKey = "ic_settings_black_48dp.png";
            this.ConfigTabPage.Location = new System.Drawing.Point(34, 4);
            this.ConfigTabPage.Margin = new System.Windows.Forms.Padding(0);
            this.ConfigTabPage.Name = "ConfigTabPage";
            this.ConfigTabPage.Size = new System.Drawing.Size(1205, 918);
            this.ConfigTabPage.TabIndex = 5;
            // 
            // configControl1
            // 
            this.configControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.configControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.configControl1.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.configControl1.Location = new System.Drawing.Point(0, 0);
            this.configControl1.Margin = new System.Windows.Forms.Padding(0);
            this.configControl1.Name = "configControl1";
            this.configControl1.Size = new System.Drawing.Size(1205, 918);
            this.configControl1.TabIndex = 0;
            // 
            // MusicTabPage
            // 
            this.MusicTabPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.MusicTabPage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.MusicTabPage.Controls.Add(this.MusicControl);
            this.MusicTabPage.ImageKey = "ic_music_video_black_48dp.png";
            this.MusicTabPage.Location = new System.Drawing.Point(34, 4);
            this.MusicTabPage.Margin = new System.Windows.Forms.Padding(0);
            this.MusicTabPage.Name = "MusicTabPage";
            this.MusicTabPage.Size = new System.Drawing.Size(1205, 923);
            this.MusicTabPage.TabIndex = 3;
            // 
            // MusicControl
            // 
            this.MusicControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.MusicControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MusicControl.Location = new System.Drawing.Point(0, 0);
            this.MusicControl.Margin = new System.Windows.Forms.Padding(0);
            this.MusicControl.Name = "MusicControl";
            this.MusicControl.Size = new System.Drawing.Size(1205, 923);
            this.MusicControl.TabIndex = 0;
            // 
            // ScoreTabPage
            // 
            this.ScoreTabPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.ScoreTabPage.Controls.Add(this.ScoreControl);
            this.ScoreTabPage.ImageKey = "ic_queue_music_black_48dp.png";
            this.ScoreTabPage.Location = new System.Drawing.Point(34, 4);
            this.ScoreTabPage.Margin = new System.Windows.Forms.Padding(0);
            this.ScoreTabPage.Name = "ScoreTabPage";
            this.ScoreTabPage.Size = new System.Drawing.Size(1205, 918);
            this.ScoreTabPage.TabIndex = 4;
            // 
            // ScoreControl
            // 
            this.ScoreControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.ScoreControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.ScoreControl.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ScoreControl.Location = new System.Drawing.Point(0, 0);
            this.ScoreControl.Margin = new System.Windows.Forms.Padding(4, 7, 4, 7);
            this.ScoreControl.Name = "ScoreControl";
            this.ScoreControl.Size = new System.Drawing.Size(1205, 331);
            this.ScoreControl.TabIndex = 0;
            // 
            // MainTabImageList
            // 
            this.MainTabImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.MainTabImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("MainTabImageList.ImageStream")));
            this.MainTabImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.MainTabImageList.Images.SetKeyName(0, "ic_music_video_black_48dp.png");
            this.MainTabImageList.Images.SetKeyName(1, "ic_straighten_black_48dp.png");
            this.MainTabImageList.Images.SetKeyName(2, "ic_border_color_black_48dp.png");
            this.MainTabImageList.Images.SetKeyName(3, "ic_cancel_black_48dp.png");
            this.MainTabImageList.Images.SetKeyName(4, "ic_add_circle_black_48dp.png");
            this.MainTabImageList.Images.SetKeyName(5, "ic_queue_music_black_48dp.png");
            this.MainTabImageList.Images.SetKeyName(6, "ic_blur_on_black_48dp.png");
            this.MainTabImageList.Images.SetKeyName(7, "ic_settings_black_48dp.png");
            // 
            // PlayerControl
            // 
            this.PlayerControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.PlayerControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.PlayerControl.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.PlayerControl.Location = new System.Drawing.Point(0, 0);
            this.PlayerControl.Margin = new System.Windows.Forms.Padding(0);
            this.PlayerControl.Name = "PlayerControl";
            this.PlayerControl.Size = new System.Drawing.Size(1243, 60);
            this.PlayerControl.TabIndex = 36;
            // 
            // MainStatusStrip
            // 
            this.MainStatusStrip.AllowMerge = false;
            this.MainStatusStrip.BackColor = System.Drawing.Color.LightGray;
            this.MainStatusStrip.GripMargin = new System.Windows.Forms.Padding(0);
            this.MainStatusStrip.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.MainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LogToolStripSplitButton,
            this.MainToolStripStatusLabel});
            this.MainStatusStrip.Location = new System.Drawing.Point(0, 986);
            this.MainStatusStrip.Name = "MainStatusStrip";
            this.MainStatusStrip.Padding = new System.Windows.Forms.Padding(2, 0, 21, 0);
            this.MainStatusStrip.Size = new System.Drawing.Size(1243, 32);
            this.MainStatusStrip.TabIndex = 37;
            // 
            // LogToolStripSplitButton
            // 
            this.LogToolStripSplitButton.AutoToolTip = false;
            this.LogToolStripSplitButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.LogToolStripSplitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.LogToolStripSplitButton.DropDownButtonWidth = 0;
            this.LogToolStripSplitButton.ForeColor = System.Drawing.Color.White;
            this.LogToolStripSplitButton.Image = ((System.Drawing.Image)(resources.GetObject("LogToolStripSplitButton.Image")));
            this.LogToolStripSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.LogToolStripSplitButton.Margin = new System.Windows.Forms.Padding(0);
            this.LogToolStripSplitButton.Name = "LogToolStripSplitButton";
            this.LogToolStripSplitButton.Size = new System.Drawing.Size(33, 32);
            this.LogToolStripSplitButton.Text = "toolStripSplitButton1";
            this.LogToolStripSplitButton.ButtonClick += new System.EventHandler(this.LogToolStripSplitButton_ButtonClick);
            // 
            // MainToolStripStatusLabel
            // 
            this.MainToolStripStatusLabel.AutoSize = false;
            this.MainToolStripStatusLabel.BackColor = System.Drawing.Color.LightGray;
            this.MainToolStripStatusLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.MainToolStripStatusLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.MainToolStripStatusLabel.ForeColor = System.Drawing.SystemColors.Highlight;
            this.MainToolStripStatusLabel.Margin = new System.Windows.Forms.Padding(0);
            this.MainToolStripStatusLabel.Name = "MainToolStripStatusLabel";
            this.MainToolStripStatusLabel.Size = new System.Drawing.Size(1187, 32);
            this.MainToolStripStatusLabel.Spring = true;
            this.MainToolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(0, 0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(200, 100);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            // 
            // EditerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.ClientSize = new System.Drawing.Size(1243, 1018);
            this.Controls.Add(this.MainTabControl);
            this.Controls.Add(this.MainStatusStrip);
            this.Controls.Add(this.PlayerControl);
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.MinimumSize = new System.Drawing.Size(842, 257);
            this.Name = "EditerForm";
            this.Text = "MidiMapMidiEditor";
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.MidiMapSetTabPage.ResumeLayout(false);
            this.EditTabPage.ResumeLayout(false);
            this.MainTabControl.ResumeLayout(false);
            this.ConfigTabPage.ResumeLayout(false);
            this.MusicTabPage.ResumeLayout(false);
            this.ScoreTabPage.ResumeLayout(false);
            this.MainStatusStrip.ResumeLayout(false);
            this.MainStatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.TabPage MidiMapSetTabPage;
        private System.Windows.Forms.TabPage EditTabPage;
        private System.Windows.Forms.TabControl MainTabControl;
        private System.Windows.Forms.TabPage MusicTabPage;
        private System.Windows.Forms.TabPage ScoreTabPage;
        private pPlay.PlayerControl PlayerControl;
        private pMidiMapSet.MidiMapSetControl MidiMapSetControl;
        private pEdit.EditerControl EditerControl;
        private pMusic.MusicControl MusicControl;
        private pScore.ScoreControl ScoreControl;
        private System.Windows.Forms.StatusStrip MainStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel MainToolStripStatusLabel;
        private System.Windows.Forms.ToolStripSplitButton LogToolStripSplitButton;
        private System.Windows.Forms.ImageList MainTabImageList;
        private System.Windows.Forms.TabPage ConfigTabPage;
        private System.Windows.Forms.TabPage tabPage1;
        private pConfig.ConfigControl configControl1;
    }
}