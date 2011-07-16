﻿namespace DebugCommander
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setAsDebuggerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addDebuggerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.processInfoBox = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(472, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setAsDebuggerToolStripMenuItem,
            this.addDebuggerToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // setAsDebuggerToolStripMenuItem
            // 
            this.setAsDebuggerToolStripMenuItem.Name = "setAsDebuggerToolStripMenuItem";
            this.setAsDebuggerToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.setAsDebuggerToolStripMenuItem.Text = "Set As Debugger";
            this.setAsDebuggerToolStripMenuItem.Click += new System.EventHandler(this.setAsDebuggerToolStripMenuItem_Click);
            // 
            // addDebuggerToolStripMenuItem
            // 
            this.addDebuggerToolStripMenuItem.Name = "addDebuggerToolStripMenuItem";
            this.addDebuggerToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.addDebuggerToolStripMenuItem.Text = "Add Debugger";
            this.addDebuggerToolStripMenuItem.Click += new System.EventHandler(this.addDebuggerToolStripMenuItem_Click);
            // 
            // processInfoBox
            // 
            this.processInfoBox.Location = new System.Drawing.Point(13, 28);
            this.processInfoBox.Multiline = true;
            this.processInfoBox.Name = "processInfoBox";
            this.processInfoBox.ReadOnly = true;
            this.processInfoBox.Size = new System.Drawing.Size(447, 104);
            this.processInfoBox.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 355);
            this.Controls.Add(this.processInfoBox);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DebugCommander";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addDebuggerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setAsDebuggerToolStripMenuItem;
        private System.Windows.Forms.TextBox processInfoBox;


    }
}

