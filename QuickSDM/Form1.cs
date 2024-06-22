using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using QUEUEDAT;
using ExportModule_Extensions;

namespace QuickSDM
{
    public partial class Form1 : Form
    {
        string Path = "";
        bool DlStarted = false;

        public Form1()
        {
            InitializeComponent();

            Path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\__QuickSDM_DL";

            if(!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }

            if(File.Exists("QUEUE.DAT"))
            {
                QueueList ql = QueueList.Load(Directory.GetCurrentDirectory());

                foreach(DownloadLink dl in ql.Items)
                {
                    PendingItem pi = new PendingItem();
                    pi.Url = dl.Url;
                    pi.FileName = dl.FileName;
                    ItemsPanel.Controls.Add(pi);
                }
            }
        }

        private void RequestDownload(object sender, EventArgs e)
        {
            
            if (!DlStarted)
            {
                DlStarted = true;
                Thread thread = new Thread(new ThreadStart(() => Download()));
                thread.Start();
            }
            else
            {
                MessageBox.Show("Any process is already running.", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void Download()
        {
            foreach (Control control in ItemsPanel.Controls)
            {
                PendingItem item = control as PendingItem;

                if (item.State != 0)
                {
                    continue;
                }

                using (WebClient client = new WebClient())
                {
                    try
                    {
                        client.DownloadFile(item.Url, Path + "\\" + item.FileName);
                        item.State = 1;
                    }
                    catch
                    { 
                        item.State = -1;
                    }
                }
            }

            DlStarted = false;
            System.Diagnostics.Process.Start(Path);
        }
    }
}
