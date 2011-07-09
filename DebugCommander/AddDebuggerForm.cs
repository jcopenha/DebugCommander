using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DebugCommander
{
    public partial class AddDebuggerForm : Form
    {
        public string DisplayName { get { return DisplayNameTextBox.Text; } }
        public string CommandLineArguments { get { return CommandLineArgumentsTextBox.Text; } }
        public string Executable { get { return ExecutableTextBox.Text; } }

        public AddDebuggerForm()
        {
            InitializeComponent();
            DisplayNameTextBox.Text = "New Debugger";
            CommandLineArgumentsTextBox.Text = "-p %pid% -e %event%";

        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Executables files|*.exe";
            if (ofd.ShowDialog() == DialogResult.OK)
                ExecutableTextBox.Text = ofd.FileName;
        }
    }
}
