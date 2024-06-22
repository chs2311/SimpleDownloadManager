using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SDM_GUI
{
    public partial class QueueManagement : Form
    {
        public Queue CurrentQueue = new Queue();
        Queue BakQueue = new Queue();
        public QueueManagement(Queue queue)
        {
            InitializeComponent();
            CurrentQueue = queue;
            BakQueue = queue;
            LoadQueue();
        }

        private void LoadQueue()
        {
            QueueTable.Rows.Clear();
            foreach (QueueItem item in CurrentQueue.Items)
            {
                QueueTable.Rows.Add(item.FileName + "", item.Url + "", "", item.Checksum + "", item.State + "");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < QueueTable.RowCount - 1; i++)
            {
                DataGridViewRow row = QueueTable.Rows[i];
                string newname = row.Cells[0].Value.ToString();

                for (int j = 19; j <= 256; j++)
                {
                    newname = newname.Replace("%" + Convert.ToString(j, 16), (char)j + "");
                }

                row.Cells[2].Value = newname;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            CurrentQueue.Items.Clear();

            for (int i = 0; i < QueueTable.RowCount - 1; i++)
            {
                DataGridViewRow row = QueueTable.Rows[i];
                QueueItem qi = new QueueItem();


                qi.Url = row.Cells[1].Value.ToString();
                qi.Checksum = row.Cells[3].Value.ToString().ToUpper();

                if (!string.IsNullOrWhiteSpace(row.Cells[2].Value.ToString()))
                {
                    qi.FileName = row.Cells[2].Value.ToString();
                }
                else
                {
                    qi.FileName = row.Cells[0].Value.ToString();
                }

                int mightstate = Convert.ToInt32(row.Cells[4].Value.ToString());

                if(mightstate != -1 && mightstate != 0 && mightstate != 1)
                {
                    mightstate = 0;
                }

                qi.State = mightstate;

                CurrentQueue.Items.Add(qi);
            }

            LoadQueue();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            CurrentQueue = BakQueue;
            LoadQueue();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            QueueTable.Sort(FilenameCol, ListSortDirection.Ascending);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button5_Click(sender, e);

            for (int i = 0; i < QueueTable.RowCount - 1; i++)
            {
                DataGridViewRow row1 = QueueTable.Rows[i];
                row1.Cells[2].Value = row1.Cells[0].Value.ToString();
            }

            for (int i = 0; i < QueueTable.RowCount - 1; i++)
            {
                for (int j = 0; j < QueueTable.RowCount - 1; j++)
                {
                    DataGridViewRow row1 = QueueTable.Rows[i];
                    DataGridViewRow row2 = QueueTable.Rows[j];

                    if(i == j)
                    {
                        continue;
                    }

                    DataGridViewCell cell1 = row1.Cells[2];
                    DataGridViewCell cell2 = row2.Cells[2];

                    string fn1 = cell1.Value.ToString();
                    string fn2 = cell2.Value.ToString();

                    if (fn1 == fn2)
                    {
                        row2.Cells[2].Value = "[DUPL]_" + fn2;
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult cc =
                MessageBox.Show("Your userdefined Filenames will be lost, continue anyways?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if(cc == DialogResult.No)
            {
                return;
            }

            for(int a = 0; a < 10; a++)
            {
                for (int i = 0; i < QueueTable.RowCount - 1; i++)
                {
                    for (int j = 0; j < QueueTable.RowCount - 1; j++)
                    {
                        DataGridViewRow row1 = QueueTable.Rows[i];
                        DataGridViewRow row2 = QueueTable.Rows[j];

                        if (i == j)
                        {
                            continue;
                        }

                        DataGridViewCell cell1 = row1.Cells[1];
                        DataGridViewCell cell2 = row2.Cells[1];

                        string fn1 = cell1.Value.ToString();
                        string fn2 = cell2.Value.ToString();

                        if (fn1 == fn2)
                        {
                            QueueTable.Rows.Remove(row2);
                        }

                        button5_Click(sender, e);
                    }
                }

                for (int i = 0; i < QueueTable.RowCount - 1; i++)
                {
                    string url = QueueTable.Rows[i].Cells[1].Value.ToString();

                    string[] uriparts = url.Split('/');
                    string filename = uriparts[uriparts.Length - 1];

                    QueueTable.Rows[i].Cells[2].Value = filename;
                }

                button5_Click(sender, e);
                button4_Click(sender, e);
                button5_Click(sender, e);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < QueueTable.RowCount - 1; i++)
            {
                string url = QueueTable.Rows[i].Cells[1].Value.ToString();

                string[] uriparts = url.Split('/');
                string filename = uriparts[uriparts.Length - 1];

                QueueTable.Rows[i].Cells[0].Value = filename;
                QueueTable.Rows[i].Cells[2].Value = "";
                QueueTable.Rows[i].Cells[3].Value = "";
                QueueTable.Rows[i].Cells[4].Value = "0";

                button5_Click(sender, e);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < QueueTable.RowCount - 1; i++)
            {
                QueueTable.Rows[i].Cells[4].Value = "0";
            }
        }

        private void RebuildQueue(object sender, FormClosingEventArgs e)
        {
            Queue queue = new Queue();
            queue.Items = new List<QueueItem>();

            for (int i = 0; i < QueueTable.RowCount - 1; i++)
            {
                DataGridViewRow row = QueueTable.Rows[i];

                QueueItem qi = new QueueItem();
                qi.FileName = row.Cells[0].Value.ToString();
                qi.Url = row.Cells[1].Value.ToString();
                qi.Checksum = row.Cells[3].Value.ToString();
                qi.State = Convert.ToInt32(row.Cells[4].Value.ToString());

                row.Cells[0].Value = "-";
                row.Cells[1].Value = "-";
                row.Cells[2].Value = "-";
                row.Cells[3].Value = "-";
                row.Cells[4].Value = "-";

                queue.Items.Add(qi);
            }
            CurrentQueue = queue;
        }
    }
}
