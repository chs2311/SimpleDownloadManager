using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SDM_GUI
{
    public partial class QueueVerfier : Form
    {
        Queue Queue;
        Queue CurrentQueue;
        public QueueVerfier(Queue ql)
        {
            InitializeComponent();
            Queue = ql;
            CurrentQueue = ql;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox2.Text.ToUpper() == GetMd5())
            {
                MessageBox.Show("Checksum verification succeded.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Checksum verification failed.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Text = GetMd5();
        }

        private string GetMd5()
        {
            if(radioButton2.Checked)
            {
                Queue = SDM_GUI.Queue.Load(textBox1.Text);
            }
            else
            {
                Queue = CurrentQueue;
            }

            string[] encpending = new string[Queue.Items.Count * 2 + 1];
            encpending[0] = "SimpleDownloadManager MD5-Verifier (by Christian Schlei)";

            int count = 1;

            foreach (QueueItem item in Queue.Items)
            {
                encpending[count] = item.FileName;
                encpending[count + 1] = item.Url;

                count += 2;
            }

            string builtstring = "";

            foreach(string str in encpending)
            {
                builtstring = Md5(builtstring + "#C#S#" +  str);
            }

            builtstring = Md5(builtstring);

            return builtstring.ToUpper();
        }

        private string Md5(string txt)
        {
            string hashText = string.Empty;
            var bytes = Encoding.Unicode.GetBytes(txt);
            var md5 = MD5.Create();
            var tmpHash = md5.ComputeHash(bytes);
            foreach (byte x in tmpHash)
            {
                hashText += String.Format("{0:x2}", x);
            }
            return hashText;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Load queue from file";
            ofd.Filter = "Simple Download Manager (SDM) File |*.sdm|Binary File |*.bin|Dat File |*.dat";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string path = ofd.FileName;
                textBox1.Text = path;
            }
        }
    }
}
