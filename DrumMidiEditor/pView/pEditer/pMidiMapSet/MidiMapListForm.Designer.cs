namespace DrumMidiEditor.pView.pEditer.pMidiMapSet
{
    partial class MidiMapListForm
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
            this.NoSelectButton = new System.Windows.Forms.Button();
            this.MidiMapListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // NoSelectButton
            // 
            this.NoSelectButton.BackColor = System.Drawing.Color.DimGray;
            this.NoSelectButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.NoSelectButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.NoSelectButton.FlatAppearance.BorderColor = System.Drawing.Color.LightSlateGray;
            this.NoSelectButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.NoSelectButton.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.NoSelectButton.ForeColor = System.Drawing.Color.White;
            this.NoSelectButton.Location = new System.Drawing.Point(0, 0);
            this.NoSelectButton.Margin = new System.Windows.Forms.Padding(0);
            this.NoSelectButton.Name = "NoSelectButton";
            this.NoSelectButton.Size = new System.Drawing.Size(352, 28);
            this.NoSelectButton.TabIndex = 34;
            this.NoSelectButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.NoSelectButton.UseVisualStyleBackColor = false;
            this.NoSelectButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.NoSelectButton_MouseClick);
            // 
            // MidiMapListBox
            // 
            this.MidiMapListBox.BackColor = System.Drawing.Color.Black;
            this.MidiMapListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.MidiMapListBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.MidiMapListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MidiMapListBox.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MidiMapListBox.ForeColor = System.Drawing.Color.Lime;
            this.MidiMapListBox.FormattingEnabled = true;
            this.MidiMapListBox.ItemHeight = 12;
            this.MidiMapListBox.Location = new System.Drawing.Point(0, 28);
            this.MidiMapListBox.Margin = new System.Windows.Forms.Padding(0);
            this.MidiMapListBox.Name = "MidiMapListBox";
            this.MidiMapListBox.Size = new System.Drawing.Size(352, 436);
            this.MidiMapListBox.TabIndex = 35;
            this.MidiMapListBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MidiMapListBox_MouseClick);
            this.MidiMapListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.MidiMapListBox_MouseDoubleClick);
            // 
            // MidiMapListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.ClientSize = new System.Drawing.Size(352, 464);
            this.ControlBox = false;
            this.Controls.Add(this.MidiMapListBox);
            this.Controls.Add(this.NoSelectButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MidiMapListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "MidiMap Key Change";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button NoSelectButton;
        private System.Windows.Forms.ListBox MidiMapListBox;

    }
}