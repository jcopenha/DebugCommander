namespace DebugCommander
{
    partial class MainForm
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
            this.btnWinDbgx86 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnWinDbgx86
            // 
            this.btnWinDbgx86.Location = new System.Drawing.Point(13, 13);
            this.btnWinDbgx86.Name = "btnWinDbgx86";
            this.btnWinDbgx86.Size = new System.Drawing.Size(75, 23);
            this.btnWinDbgx86.TabIndex = 0;
            this.btnWinDbgx86.Text = "WinDbg x86";
            this.btnWinDbgx86.UseVisualStyleBackColor = true;
            this.btnWinDbgx86.Click += new System.EventHandler(this.btnWinDbgx86_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 264);
            this.Controls.Add(this.btnWinDbgx86);
            this.Name = "MainForm";
            this.Text = "DebugCommander";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnWinDbgx86;
    }
}

