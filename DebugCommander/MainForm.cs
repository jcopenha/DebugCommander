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
using Microsoft.Win32;

namespace DebugCommander
{
    public partial class MainForm : Form
    {
        string _processId = "";
        string _eventNumber = "";
        DebuggerCollection debuggers;
        bool debugged = false;

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

            // all the debuggers the user has added
            debuggers = DebuggerCollection.Load("main.xml");

            x = 0;
            foreach (Debugger debugger in debuggers)
            {
                AddDebuggerButton(debugger, x++);
            }

            // an escape hatch in case they don't want to debug this
            DoNothingDebugger dnd = new DoNothingDebugger("Do Not Debug", "", "%pid% %event%");
            AddDebuggerButton(dnd, x);

            // let's show some useful information about the process that crashed
            // this is a start but I think I might have to poke at a lower level
            // to get real information
            Process CrashedProcess = Process.GetProcessById(Int32.Parse(_processId));

            string info = String.Format("Process Name : {0}\r\n" +
                                        "Path : {1}\r\n" +
                                        "Command Line Options : {2}\r\n" +
                                        "Working Directory : {3}\r\n",
                                        CrashedProcess.ProcessName,
                                        CrashedProcess.MainModule.FileName,
                                        CrashedProcess.StartInfo.WorkingDirectory,
                                        CrashedProcess.StartInfo.Arguments);

            processInfoBox.Text = info;

        }

        private void ButtonClick(object sender, EventArgs e, Debugger debugger)
        {
            this.Hide();

            // this will return when the debugger exits
            debugger.StartDebugger(_processId, _eventNumber);
            debugged = true;

            Application.Exit();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            debuggers.Save("main.xml");
            if (!debugged)
            {
                DoNothingDebugger dnd = new DoNothingDebugger("Do Not Debug", "", "%pid% %event%");
                dnd.StartDebugger(_processId, _eventNumber);
            }
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

            btn.Location = new System.Drawing.Point(13, 35 * (index + 1) + 125);
            btn.Name = "btn" + debugger.DisplayName.Replace(" ", "_"); // really all whitespace
            btn.Size = new System.Drawing.Size(this.ClientRectangle.Width - 26, 23);
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

        private void setAsDebuggerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Assume if on 64-bit we are running as 64-bit
            RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\AeDebug");
            if (key == null)
                return; // oops

            key.SetValue("Debugger", Application.ExecutablePath + " -p %ld -e %ld");
            key.SetValue("Auto", "1");
            key.Close();

            // So we'll try and open the 32-bit view now
            key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows NT\CurrentVersion\AeDebug");
            if (key == null)
                return; // not a problem

            key.SetValue("Debugger", Application.ExecutablePath + " -p %ld -e %ld");
            key.SetValue("Auto", "1");
            key.Close();
        }
    }
}
