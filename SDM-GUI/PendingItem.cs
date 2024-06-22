using SDM_GUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace CS_DownloadManager
{
    public partial class PendingItem : UserControl
    {
        public event EventHandler<EventArgs> DownloadDone;

        public string FileName 
        { 
            get 
            { 
                return label1.Text; 
            } 
            set 
            { 
                label1.Text = value;
                string path = "assets\\extensions\\" + Path.GetExtension(value).TrimStart('.') + ".png";
                if (File.Exists(path))
                {
                    Size size = new Size(32, 32);
                    pictureBox1.Image = new Bitmap(new Bitmap(path), size);
                }
                else
                {
                    Size size = new Size(32, 32);
                    pictureBox1.Image = new Bitmap(new Bitmap("assets\\extensions\\.PNG"), size);
                }
                
            } 
        }
        public string Url 
        { 
            get 
            { 
                return label2.Text; 
            } 
            set 
            { 
                label2.Text = value;
                LoadSize(value);
            } 
        }

        public long FileSize
        {
            get;
            private set;
        }

        public void LoadSize(string url)
        {
            try
            {
                WebClient client = new WebClient();
                using (client.OpenRead(url))
                    FileSize = Convert.ToInt64(client.ResponseHeaders["Content-Length"]);
                label4.Text = "Total Filesize: " + FormatSize(FileSize);
            }
            catch (Exception ex)
            {
                MainForm.Log("Unable to determine file size. " + ex.ToString() + "\n");
                label4.Text = ex.Message;
            }
        }

        public string FormatSize(long size)
        {
            string[] units = { "B", "KB", "MB", "GB", "TB" };
            int uindex = 0;
            double nsize = (double)size;

            while (nsize >= 1024)
            {
                nsize = nsize / 1024;
                uindex++;

                if(uindex >= units.Length)
                {
                    uindex = 4;
                    break;
                }
            }

            return Math.Round(nsize, 2) + " " +  units[uindex];
        }

        int state = 0;

        public int State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;

                if(state == -1)
                {
                    pictureBox2.Image = new Bitmap("assets\\icons\\failed");
                }
                else if (state == 1)
                {
                    pictureBox2.Image = new Bitmap("assets\\icons\\success");
                }
                else if (state == 0) 
                {
                    pictureBox2.Image = new Bitmap("assets\\icons\\pending");
                }
            }
        }

        public string Checksum { get; set; }

        public PendingItem()
        {
            InitializeComponent();
            this.Dock = DockStyle.Top;
            pictureBox2.Image = new Bitmap("assets\\icons\\pending");
            pictureBox3.Image = new Bitmap("assets\\icons\\options");
            pictureBox5.Image = new Bitmap("assets\\icons\\dropdown");
            pictureBox4.BringToFront();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            InputBox inputBox = new InputBox();
            inputBox.ShowDialog();
            
            if(inputBox.Input != "")
            {
                string fn = inputBox.Input;
                FileName = fn;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Parent.Controls.Remove(this);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            InputBox inputBox = new InputBox();
            inputBox.ShowDialog();

            if (inputBox.Input != "")
            {
                string fn = inputBox.Input;
                Checksum = fn.ToUpper();
            }
        }

        private void OpenContextMenu(object sender, MouseEventArgs e)
        {
            ContextMenu newmenu = new ContextMenu();
            newmenu.MenuItems.Add("Edit Filename", button1_Click);
            newmenu.MenuItems.Add("Remove Item", button2_Click);
            newmenu.MenuItems.Add("Change (Add) Checksum", button3_Click);
            pictureBox3.ContextMenu = newmenu;
            newmenu.Show(pictureBox3, new Point(16, 16));
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            if(this.Height == 161)
            {
                this.Height = 61;
                pictureBox5.Image = new Bitmap("assets\\icons\\dropdown");
            }
            else if (this.Height == 61)
            {
                this.Height = 161;
                pictureBox5.Image = new Bitmap("assets\\icons\\dropdown_flip");
            }

            pictureBox4.Top = Height - 1;
        }

        public void Expand()
        {
            this.Height = 161;
            pictureBox4.Top = Height - 1;
            pictureBox5.Image = new Bitmap("assets\\icons\\dropdown_flip");
        }

        public void Shrink()
        {
            this.Height = 61;
            pictureBox4.Top = Height - 1;
            pictureBox5.Image = new Bitmap("assets\\icons\\dropdown");
        }

        public void Download(string filename)
        {
            WebClient client = new WebClient();
            client.DownloadFileAsync(new Uri(Url), filename);
            client.DownloadProgressChanged += DlProgressChanged;
            client.DownloadFileCompleted += (sender, e) => DownloadDone(null, null);
        }

        private void DlProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            label3.Text = "File Size: " + FormatSize(e.BytesReceived);
        }
    }
}
