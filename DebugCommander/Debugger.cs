using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Runtime.InteropServices;

namespace DebugCommander
{
    [Serializable]
    public class Debugger
    {
        private string _DebuggerExecutable;
        private string _CommandLineArguments;
        private string _DisplayName;

        public string DebuggerExecutable { 
            get { return _DebuggerExecutable; }
            set { _DebuggerExecutable = value; }
        }
        public string CommandLineArguments { 
            get { return _CommandLineArguments; }
            set { _CommandLineArguments = value; }
        }
        public string DisplayName { 
            get { return _DisplayName; }
            set { _DisplayName = value; }
        }

        public Debugger()
        {
        }

        public Debugger(string DisplayName, string Debugger, string Arguments)
        {
            _DisplayName = DisplayName;
            _DebuggerExecutable = Debugger;
            _CommandLineArguments = Arguments;

            if (!_CommandLineArguments.Contains("%pid%"))
                throw new ArgumentException("The command line arguments must contain a %pid% value.");
            if (!_CommandLineArguments.Contains("%event%"))
                throw new ArgumentException("The command line arguments must contain a %event% value.");
        }

        virtual public bool StartDebugger(string pid, string eventnumber)
        {
            string commandLine = _CommandLineArguments;
            commandLine = commandLine.Replace("%pid%", pid);
            commandLine = commandLine.Replace("%event%", eventnumber);

            ProcessStartInfo startInfo = new ProcessStartInfo(_DebuggerExecutable);
            startInfo.UseShellExecute = true;
            startInfo.Arguments = commandLine;
            Process p = Process.Start(startInfo);
            
            if (p == null)
                return false;

            // First trick.  WerFault.exe waits on the event AND the process
            // it launches.  If the process it launches exits before the event
            // is set then WerFault.exe thinks the crash wasn't handled and
            // relaunchs the fault handling dialog.  So don't return from here
            // until the debugger exits.
            p.WaitForExit();
            
            return true;
        }
    }
    
    
    [Serializable]
    public class DoNothingDebugger : Debugger
    {
        [DllImport("kernel32.dll")]
        static extern bool SetEvent(IntPtr hEvent);

        public DoNothingDebugger(string DisplayName, string Debugger, string Arguments) :
            base(DisplayName, Debugger, Arguments)
        {
            
        }

        public override bool  StartDebugger(string pid, string eventnumber)
        {
            // tell WERFFault that we handled the debugging
            SetEvent((IntPtr)Int32.Parse(eventnumber));
            return true;
        }
    }
}
