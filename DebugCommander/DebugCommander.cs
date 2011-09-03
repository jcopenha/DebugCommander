using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Windows.Forms;
using System.Diagnostics;


namespace DebugCommander
{
    class DebugCommander
    {
        string _processId = "";
        string _eventNumber = "";
        DebuggerCollection debuggers;
        bool debugged = false;
        TaskDialog taskDialogMain = null;

        public DebugCommander()
        {
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

            taskDialogMain = new TaskDialog();
            taskDialogMain.Caption = "DebugCommander Debuggers";
            taskDialogMain.InstructionText = String.Format("The {0} process has crashed.  Choose a Debugger.", CrashedProcess.ProcessName);
            taskDialogMain.FooterText = "<a href=\"\">Add Debugger</a>";
            taskDialogMain.Closing += new EventHandler<TaskDialogClosingEventArgs>(taskDialogMain_Closing);
            taskDialogMain.FooterCheckBoxText = "Set DebugCommander as default debugger?";
            taskDialogMain.FooterCheckBoxChecked = false; // need method for checking if we are already set.
            taskDialogMain.HyperlinksEnabled = true;
            taskDialogMain.HyperlinkClick += new EventHandler<TaskDialogHyperlinkClickedEventArgs>(taskDialogMain_HyperlinkClick);
            taskDialogMain.Cancelable = true;

            taskDialogMain.DetailsExpanded = false;
            taskDialogMain.DetailsExpandedLabel = "Hide details";
            taskDialogMain.DetailsCollapsedLabel = "Show details";
            taskDialogMain.ExpansionMode = TaskDialogExpandedDetailsLocation.ExpandFooter;
            taskDialogMain.DetailsExpandedText = info;
            
            x = 0;
            foreach (Debugger debugger in debuggers)
            {
                taskDialogMain.Controls.Add(CreateDebuggerLink(debugger, x));
                x++;
            }

            // an escape hatch in case they don't want to debug this
            DoNothingDebugger dnd = new DoNothingDebugger("Do Not Debug", "", "%pid% %event%");
            taskDialogMain.Controls.Add(CreateDebuggerLink(dnd, 0));

            taskDialogMain.Show();
            Application.Exit();
        }

        void taskDialogMain_HyperlinkClick(object sender, TaskDialogHyperlinkClickedEventArgs e)
        {
            AddDebuggerForm form = new AddDebuggerForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                AddDebugger(form.DisplayName, form.Executable, form.CommandLineArguments);
            }
        }

        void taskDialogMain_Closing(object sender, TaskDialogClosingEventArgs e)
        {
            debuggers.Save("main.xml");
            if (!debugged)
            {
                DoNothingDebugger dnd = new DoNothingDebugger("Do Not Debug", "", "%pid% %event%");
                dnd.StartDebugger(_processId, _eventNumber);
            }
            if (taskDialogMain.FooterCheckBoxChecked.Value) 
            {
                setAsDefaultDebugger();
            }
        }

        private void ButtonClick(object sender, EventArgs e, Debugger debugger)
        {
            // This hides the TaskDialog even though we aren't done with it yet.
            taskDialogMain.Close();
            // this will return when the debugger exits
            debugger.StartDebugger(_processId, _eventNumber);
            debugged = true;
        }

        private TaskDialogCommandLink CreateDebuggerLink(Debugger debugger, int index)
        {
            TaskDialogCommandLink link = new TaskDialogCommandLink(index.ToString(), debugger.DisplayName);
            link.Click += new System.EventHandler(
                delegate(object sender, EventArgs e)
                {
                    this.ButtonClick(sender, e, debugger);
                });
            return link;
        }

        private void AddDebugger(string displayname, string executable, string commandlinearguments)
        {
            Debugger debugger = new Debugger(displayname, executable, commandlinearguments);
            // can't do this, but it'll be there on the next round
            //taskDialogMain.Controls.Add(CreateDebuggerLink(debugger, taskDialogMain.Controls.Count));
            debuggers.Add(debugger);
        }

        private void setAsDefaultDebugger()
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
