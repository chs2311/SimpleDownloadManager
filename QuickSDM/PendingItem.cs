using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ExportModule_Extensions;

namespace QuickSDM
{
    public partial class PendingItem : UserControl
    {
        public string FileName
        {
            get
            {
                return label1.Text;
            }
            set
            {
                label1.Text = value;
                string ext = Path.GetExtension(value).TrimStart('.').ToUpper();
                try
                {
                    Bitmap bmp = (Bitmap)Icons.ResourceManager.GetObject("IR_" + ext, Icons.Culture);
                    pictureBox1.Image = new Bitmap(bmp, new Size(32, 32));
                }
                catch
                {
                    pictureBox1.Image = Icons.IR_;
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
            }
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

                if (state == -1)
                {
                    pictureBox3.Image = InternalResx.FAILED;
                }
                else if (state == 1)
                {
                    pictureBox3.Image = InternalResx.SUCCESS;
                }
                else if (state == 0)
                {
                    pictureBox3.Image = InternalResx.PENDING;
                }
            }
        }

        public PendingItem()
        {
            InitializeComponent();
            this.Dock = DockStyle.Top;
            State = 0;
        }
    }
}
