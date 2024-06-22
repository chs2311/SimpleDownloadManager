using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CS_DownloadManager
{
    public partial class InputBox : Form
    {
        public string Input { get { return textBox1.Text; } set { textBox1.Text = value; } }
        public InputBox()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
