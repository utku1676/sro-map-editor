using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SilkroadMapEditor
{
    public partial class Window : Form
    {
        public Window()
        {
            InitializeComponent();
        }

        private void Window_Load(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.game.Exit();
            this.Close();
        }

        private void Window_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.game.Exit();
        }
    }
}
