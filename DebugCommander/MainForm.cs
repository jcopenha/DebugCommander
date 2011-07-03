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

namespace DebugCommander
{
    public partial class MainForm : Form
    {
        string _processId = "";
        string _eventNumber = "";

        public MainForm()
        {
            InitializeComponent();
            string[] args = Environment.GetCommandLineArgs();

            int processIdIndex = 0;
            int eventNumberIndex = 0;
            for (int x = 0; x < args.Length; x++)
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
        }

        private void btnWinDbgx86_Click(object sender, EventArgs e)
        {
            Debugger windbg = new Debugger("WinDbg x86",
                                           @"C:\Program Files (x86)\Debugging Tools For Windows (x86)\WinDbg.exe",
                                           "-p %pid% -e %event%");

            this.Hide();

            // this will return when the debugger exits
            windbg.StartDebugger(_processId, _eventNumber);

            Application.Exit();
        }
    }
}
