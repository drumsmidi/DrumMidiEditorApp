namespace DrumMidiEditor.pView.pEditer.pMidiMapSet
{
	partial class ImportMidiMapSetForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.CxlButton = new System.Windows.Forms.Button();
            this.ImportButton = new System.Windows.Forms.Button();
            this.ConvertDataGridView = new System.Windows.Forms.DataGridView();
            this.MidiMapUsedColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MidiMapAssignColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ImportMidiMapSetGridView = new System.Windows.Forms.DataGridView();
            this.ImportMidiMapSetColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ConvertDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImportMidiMapSetGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.panel1.Controls.Add(this.CxlButton);
            this.panel1.Controls.Add(this.ImportButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1036, 40);
            this.panel1.TabIndex = 39;
            // 
            // CxlButton
            // 
            this.CxlButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CxlButton.BackColor = System.Drawing.Color.DodgerBlue;
            this.CxlButton.BackgroundImage = global::DrumMidiEditor.Properties.Resources.ic_cancel_black_48dp;
            this.CxlButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.CxlButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CxlButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CxlButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CxlButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.CxlButton.Location = new System.Drawing.Point(958, 4);
            this.CxlButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CxlButton.Name = "CxlButton";
            this.CxlButton.Size = new System.Drawing.Size(68, 32);
            this.CxlButton.TabIndex = 15;
            this.CxlButton.TabStop = false;
            this.CxlButton.UseVisualStyleBackColor = false;
            this.CxlButton.Click += new System.EventHandler(this.CxlButton_Click);
            // 
            // ImportButton
            // 
            this.ImportButton.BackColor = System.Drawing.Color.DodgerBlue;
            this.ImportButton.BackgroundImage = global::DrumMidiEditor.Properties.Resources.ic_check_circle_black_48dp;
            this.ImportButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ImportButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ImportButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ImportButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.ImportButton.Location = new System.Drawing.Point(7, 4);
            this.ImportButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ImportButton.Name = "ImportButton";
            this.ImportButton.Size = new System.Drawing.Size(68, 32);
            this.ImportButton.TabIndex = 14;
            this.ImportButton.TabStop = false;
            this.ImportButton.UseVisualStyleBackColor = false;
            this.ImportButton.Click += new System.EventHandler(this.ImportButton_Click);
            // 
            // ConvertDataGridView
            // 
            this.ConvertDataGridView.AllowUserToAddRows = false;
            this.ConvertDataGridView.AllowUserToDeleteRows = false;
            this.ConvertDataGridView.AllowUserToResizeColumns = false;
            this.ConvertDataGridView.AllowUserToResizeRows = false;
            this.ConvertDataGridView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.ConvertDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ConvertDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MidiMapUsedColumn,
            this.MidiMapAssignColumn});
            this.ConvertDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConvertDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.ConvertDataGridView.EnableHeadersVisualStyles = false;
            this.ConvertDataGridView.GridColor = System.Drawing.Color.White;
            this.ConvertDataGridView.Location = new System.Drawing.Point(355, 40);
            this.ConvertDataGridView.Margin = new System.Windows.Forms.Padding(0);
            this.ConvertDataGridView.MultiSelect = false;
            this.ConvertDataGridView.Name = "ConvertDataGridView";
            this.ConvertDataGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ConvertDataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.ConvertDataGridView.RowHeadersVisible = false;
            this.ConvertDataGridView.RowHeadersWidth = 300;
            this.ConvertDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.ConvertDataGridView.RowTemplate.Height = 21;
            this.ConvertDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ConvertDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.ConvertDataGridView.ShowCellErrors = false;
            this.ConvertDataGridView.ShowCellToolTips = false;
            this.ConvertDataGridView.ShowEditingIcon = false;
            this.ConvertDataGridView.ShowRowErrors = false;
            this.ConvertDataGridView.Size = new System.Drawing.Size(681, 576);
            this.ConvertDataGridView.TabIndex = 46;
            this.ConvertDataGridView.TabStop = false;
            this.ConvertDataGridView.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.ConvertDataGridView_CellEnter);
            // 
            // MidiMapUsedColumn
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Gold;
            this.MidiMapUsedColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.MidiMapUsedColumn.FillWeight = 50F;
            this.MidiMapUsedColumn.HeaderText = "MidiMaps used";
            this.MidiMapUsedColumn.MaxInputLength = 10;
            this.MidiMapUsedColumn.MinimumWidth = 9;
            this.MidiMapUsedColumn.Name = "MidiMapUsedColumn";
            this.MidiMapUsedColumn.ReadOnly = true;
            this.MidiMapUsedColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.MidiMapUsedColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.MidiMapUsedColumn.Width = 330;
            // 
            // MidiMapAssignColumn
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Lime;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Gold;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.MidiMapAssignColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.MidiMapAssignColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.MidiMapAssignColumn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MidiMapAssignColumn.HeaderText = "Assign to MidiMaps to be imported";
            this.MidiMapAssignColumn.MinimumWidth = 9;
            this.MidiMapAssignColumn.Name = "MidiMapAssignColumn";
            this.MidiMapAssignColumn.Width = 330;
            // 
            // ImportMidiMapSetGridView
            // 
            this.ImportMidiMapSetGridView.AllowUserToAddRows = false;
            this.ImportMidiMapSetGridView.AllowUserToDeleteRows = false;
            this.ImportMidiMapSetGridView.AllowUserToResizeColumns = false;
            this.ImportMidiMapSetGridView.AllowUserToResizeRows = false;
            this.ImportMidiMapSetGridView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.ImportMidiMapSetGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ImportMidiMapSetGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ImportMidiMapSetColumn});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Lime;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ImportMidiMapSetGridView.DefaultCellStyle = dataGridViewCellStyle5;
            this.ImportMidiMapSetGridView.Dock = System.Windows.Forms.DockStyle.Left;
            this.ImportMidiMapSetGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.ImportMidiMapSetGridView.EnableHeadersVisualStyles = false;
            this.ImportMidiMapSetGridView.GridColor = System.Drawing.Color.White;
            this.ImportMidiMapSetGridView.Location = new System.Drawing.Point(0, 40);
            this.ImportMidiMapSetGridView.Margin = new System.Windows.Forms.Padding(0);
            this.ImportMidiMapSetGridView.MultiSelect = false;
            this.ImportMidiMapSetGridView.Name = "ImportMidiMapSetGridView";
            this.ImportMidiMapSetGridView.ReadOnly = true;
            this.ImportMidiMapSetGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ImportMidiMapSetGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.ImportMidiMapSetGridView.RowHeadersVisible = false;
            this.ImportMidiMapSetGridView.RowHeadersWidth = 300;
            this.ImportMidiMapSetGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.ImportMidiMapSetGridView.RowTemplate.Height = 21;
            this.ImportMidiMapSetGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ImportMidiMapSetGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.ImportMidiMapSetGridView.ShowCellErrors = false;
            this.ImportMidiMapSetGridView.ShowCellToolTips = false;
            this.ImportMidiMapSetGridView.ShowEditingIcon = false;
            this.ImportMidiMapSetGridView.ShowRowErrors = false;
            this.ImportMidiMapSetGridView.Size = new System.Drawing.Size(355, 576);
            this.ImportMidiMapSetGridView.TabIndex = 47;
            this.ImportMidiMapSetGridView.TabStop = false;
            // 
            // ImportMidiMapSetColumn
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Gold;
            this.ImportMidiMapSetColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.ImportMidiMapSetColumn.FillWeight = 50F;
            this.ImportMidiMapSetColumn.HeaderText = "Import MidiMaps";
            this.ImportMidiMapSetColumn.MaxInputLength = 10;
            this.ImportMidiMapSetColumn.MinimumWidth = 9;
            this.ImportMidiMapSetColumn.Name = "ImportMidiMapSetColumn";
            this.ImportMidiMapSetColumn.ReadOnly = true;
            this.ImportMidiMapSetColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ImportMidiMapSetColumn.Width = 352;
            // 
            // ToolTip
            // 
            this.ToolTip.AutomaticDelay = 2000;
            // 
            // ImportMidiMapSetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 26F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.ClientSize = new System.Drawing.Size(1036, 616);
            this.ControlBox = false;
            this.Controls.Add(this.ConvertDataGridView);
            this.Controls.Add(this.ImportMidiMapSetGridView);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1060, 680);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1060, 680);
            this.Name = "ImportMidiMapSetForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Import MidiMapSet";
            this.TopMost = true;
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ConvertDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImportMidiMapSetGridView)).EndInit();
            this.ResumeLayout(false);

		}

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button ImportButton;
        private System.Windows.Forms.Button CxlButton;
        private System.Windows.Forms.DataGridView ConvertDataGridView;
        private System.Windows.Forms.DataGridView ImportMidiMapSetGridView;
        private System.Windows.Forms.ToolTip ToolTip;
        private System.Windows.Forms.DataGridViewTextBoxColumn MidiMapUsedColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn MidiMapAssignColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ImportMidiMapSetColumn;
    }
}