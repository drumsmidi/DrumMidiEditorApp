
namespace DrumMidiEditor.pGeneralFunction.pLog
{
    partial class LogForm
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
            this.ConsoleRichTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // ConsoleRichTextBox
            // 
            this.ConsoleRichTextBox.AcceptsTab = true;
            this.ConsoleRichTextBox.BackColor = System.Drawing.Color.Black;
            this.ConsoleRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ConsoleRichTextBox.CausesValidation = false;
            this.ConsoleRichTextBox.DetectUrls = false;
            this.ConsoleRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConsoleRichTextBox.Font = new System.Drawing.Font("ＭＳ ゴシック", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ConsoleRichTextBox.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.ConsoleRichTextBox.Location = new System.Drawing.Point(0, 0);
            this.ConsoleRichTextBox.Margin = new System.Windows.Forms.Padding(0);
            this.ConsoleRichTextBox.Name = "ConsoleRichTextBox";
            this.ConsoleRichTextBox.ReadOnly = true;
            this.ConsoleRichTextBox.ShortcutsEnabled = false;
            this.ConsoleRichTextBox.Size = new System.Drawing.Size(765, 374);
            this.ConsoleRichTextBox.TabIndex = 0;
            this.ConsoleRichTextBox.TabStop = false;
            this.ConsoleRichTextBox.Text = "";
            this.ConsoleRichTextBox.WordWrap = false;
            // 
            // LogForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.ClientSize = new System.Drawing.Size(765, 374);
            this.Controls.Add(this.ConsoleRichTextBox);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.White;
            this.MinimizeBox = false;
            this.Name = "LogForm";
            this.Opacity = 0.9D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Log";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LogForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox ConsoleRichTextBox;
    }
}