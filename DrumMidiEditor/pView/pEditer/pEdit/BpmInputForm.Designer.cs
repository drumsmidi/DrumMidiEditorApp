namespace DrumMidiEditor.pView.pEditer.pEdit
{
    partial class BpmInputForm
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
            this.BpmNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.OkButton = new System.Windows.Forms.Button();
            this.CxlButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.BpmNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // BpmNumericUpDown
            // 
            this.BpmNumericUpDown.BackColor = System.Drawing.Color.Black;
            this.BpmNumericUpDown.DecimalPlaces = 2;
            this.BpmNumericUpDown.ForeColor = System.Drawing.Color.Lime;
            this.BpmNumericUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.BpmNumericUpDown.Location = new System.Drawing.Point(3, 5);
            this.BpmNumericUpDown.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BpmNumericUpDown.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.BpmNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.BpmNumericUpDown.Name = "BpmNumericUpDown";
            this.BpmNumericUpDown.Size = new System.Drawing.Size(114, 23);
            this.BpmNumericUpDown.TabIndex = 0;
            this.BpmNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.BpmNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // OkButton
            // 
            this.OkButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.OkButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
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
            this.CxlButton.Location = new System.Drawing.Point(60, 34);
            this.CxlButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CxlButton.Name = "CxlButton";
            this.CxlButton.Size = new System.Drawing.Size(57, 28);
            this.CxlButton.TabIndex = 1;
            this.CxlButton.Text = "Cancel";
            this.CxlButton.UseVisualStyleBackColor = true;
            this.CxlButton.Click += new System.EventHandler(this.CxlButton_Click);
            // 
            // BpmInputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.CancelButton = this.OkButton;
            this.ClientSize = new System.Drawing.Size(121, 65);
            this.ControlBox = false;
            this.Controls.Add(this.CxlButton);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.BpmNumericUpDown);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BpmInputForm";
            this.Opacity = 0.6D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Input Bpm";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.BpmNumericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown BpmNumericUpDown;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button CxlButton;
    }
}