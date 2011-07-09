using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;

namespace DebugCommander
{
    public partial class MainForm : Form
    {
        string _processId = "";
        string _eventNumber = "";
        DebuggerCollection debuggers;

        public MainForm()
        {
            InitializeComponent();
            string[] args = Environment.GetCommandLineArgs();

            int processIdIndex = 0;
            int eventNumberIndex = 0;
            int x = 0;
            for (x = 0; x < args.Length; x++)
            {
                if (args[x].Equals("-p", StringComparison.InvariantCultureIgnoreCase))
                {
                    processIdIndex = x+1;
                }
                if (args[x].Equals("-e", StringComparison.InvariantCultureIgnoreCase))
                {
                    eventNumberIndex = x+1;
                }
            }

            if (processIdIndex < args.Length)
                _processId = args[processIdIndex];
            if (eventNumberIndex < args.Length)
                _eventNumber = args[eventNumberIndex];

            debuggers = DebuggerCollection.Load("main.xml");

            x = 0;
            foreach (Debugger debugger in debuggers)
            {
                AddDebuggerButton(debugger, x++);
            }

        }

        private void ButtonClick(object sender, EventArgs e, Debugger debugger)
        {
            this.Hide();

            // this will return when the debugger exits
            debugger.StartDebugger(_processId, _eventNumber);

            Application.Exit();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            debuggers.Save("main.xml");
        }

        private void addDebuggerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddDebuggerForm form = new AddDebuggerForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                AddDebugger(form.DisplayName, form.Executable, form.CommandLineArguments);
            }
        }

        private void AddDebuggerButton(Debugger debugger, int index)
        {
            Button btn = new Button();

            btn.Location = new System.Drawing.Point(13, 23 * (index + 1) + 5);
            btn.Name = "btn" + debugger.DisplayName.Replace(" ", "_"); // really all whitespace
            btn.Size = new System.Drawing.Size(75, 23);
            btn.TabIndex = index;
            btn.Text = debugger.DisplayName;
            btn.UseVisualStyleBackColor = true;
            btn.Click += new System.EventHandler(
                delegate(object sender, EventArgs e)
                {
                    this.ButtonClick(sender, e, debugger);
                });
            index++;
            this.Controls.Add(btn);
        }

        private void AddDebugger(string displayname, string executable, string commandlinearguments)
        {
            Debugger debugger = new Debugger(displayname, executable, commandlinearguments);
            AddDebuggerButton(debugger, debuggers.Count);
            debuggers.Add(debugger);
        }
    }
}
