namespace DrumMidiEditor.pView.pEditer.pEdit
{
    partial class ImportMidiForm
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
            this.ZoomNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.OkButton = new System.Windows.Forms.Button();
            this.CxlButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ZoomNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // ZoomNumericUpDown
            // 
            this.ZoomNumericUpDown.BackColor = System.Drawing.Color.Black;
            this.ZoomNumericUpDown.ForeColor = System.Drawing.Color.Lime;
            this.ZoomNumericUpDown.Location = new System.Drawing.Point(60, 6);
            this.ZoomNumericUpDown.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ZoomNumericUpDown.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.ZoomNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ZoomNumericUpDown.Name = "BpmNumericUpDown";
            this.ZoomNumericUpDown.Size = new System.Drawing.Size(57, 23);
            this.ZoomNumericUpDown.TabIndex = 0;
            this.ZoomNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ZoomNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // OkButton
            // 
            this.OkButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.OkButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.OkButton.ForeColor = System.Drawing.Color.Black;
            this.OkButton.Location = new System.Drawing.Point(2, 34);
            this.OkButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(57, 28);
            this.OkButton.TabIndex = 1;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // CxlButton
            // 
            this.CxlButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CxlButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CxlButton.ForeColor = System.Drawing.Color.Black;
            this.CxlButton.Location = new System.Drawing.Point(60, 34);
            this.CxlButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CxlButton.Name = "CxlButton";
            this.CxlButton.Size = new System.Drawing.Size(57, 28);
            this.CxlButton.TabIndex = 1;
            this.CxlButton.Text = "Cancel";
            this.CxlButton.UseVisualStyleBackColor = true;
            this.CxlButton.Click += new System.EventHandler(this.CxlButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(7, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Bpm ×";
            // 
            // ImportMidiForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.CancelButton = this.OkButton;
            this.ClientSize = new System.Drawing.Size(121, 65);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CxlButton);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.ZoomNumericUpDown);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImportMidiForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Import Midi";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.ZoomNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown ZoomNumericUpDown;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button CxlButton;
        private System.Windows.Forms.Label label1;
    }
}