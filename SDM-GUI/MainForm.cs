using CS_DownloadManager;
using QUEUEDAT;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Web;
using System.Threading;
using System.Windows.Forms;
using System.Linq;
using System.Media;
using WMPLib;

namespace SDM_GUI
{
    public class MainForm : Form
    {
        #region Controls

        public TableLayoutPanel MainPanel;
        public Panel LeftPanel;
        public Panel RightPanel;
        public Panel UpperLeftPanel;
        public Panel UpperRightPanel;
        public Panel ItemsPanel;
        public Panel OptionsPanel;
        public Label TitleLabel;
        public Label SettingsLabel;
        public Label FolderLabel;
        public Label AddLinksLabel;
        public Button DownloadButton;
        public Button BrowseButton;
        public Button RemFinsihedButton;
        public Button RemFailedButton;
        public PictureBox MoreIcon;
        public PictureBox AddButtonIcon;
        public TextBox FolderTextBox;
        public TextBox LinksTextBox;
        public CheckBox MonitorClipboardCheck;
        public ProgressBar DownloadProgressBar;
        public StatusStrip BottomStrip;
        public System.Windows.Forms.Timer ClipboardMonitoringLoop;

        #endregion

        public string Path;
        public Settings Settings;
        public bool IsProcessing = false;
        public string ClipboardState = "";


        public MainForm()
        {
            Settings = Settings.Load();
            Path = Settings.DownloadsPath;
            Settings.Save();
            ClipboardState = Clipboard.GetText();

            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }

            Initialize();
            FolderTextBox.Text = Path;
        }

        public MainForm(string path)
        {
            Settings = Settings.Load();
            Path = Settings.DownloadsPath;
            Settings.Save();
            ClipboardState = Clipboard.GetText();

            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }

            Initialize();
            FolderTextBox.Text = Path;

            Log($"Trying to load $ql from $data << $path  [$path=\"{path}\"]");

            Queue queue = Queue.Load(path);
            LoadAsyncNoGui(queue);

            Log($"Loaded $ql from $data << $path [$path=\"{path}\"]");
        }

        public static void Log(string text)
        {
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        private void Initialize()
        {
            #region Create instanzes

            Log("Creating instances for $controls");

            MainPanel = new TableLayoutPanel();
            LeftPanel = new Panel();
            RightPanel = new Panel();
            UpperLeftPanel = new Panel();
            UpperRightPanel = new Panel();
            ItemsPanel = new Panel();
            OptionsPanel = new Panel();
            TitleLabel = new Label();
            SettingsLabel = new Label();
            FolderLabel = new Label();
            AddLinksLabel = new Label();
            DownloadButton = new Button();
            BrowseButton = new Button();
            RemFinsihedButton = new Button();
            RemFailedButton = new Button();
            MoreIcon = new PictureBox();
            AddButtonIcon = new PictureBox();
            FolderTextBox = new TextBox();
            LinksTextBox = new TextBox();
            MonitorClipboardCheck = new CheckBox();
            DownloadProgressBar = new ProgressBar();
            BottomStrip = new StatusStrip();
            ClipboardMonitoringLoop = new System.Windows.Forms.Timer();

            Log("DONE\n");

            #endregion

            #region Form

            Log("Creating $newform");

            Icon = new Icon("assets\\favicon\\favicon.ico");
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            MinimumSize = new System.Drawing.Size(700, 600);
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "SimpleDownloadManager";
            FormClosing += BeforeExit;
            FormClosed += Exiting;

            Log("DONE\n");

            #endregion

            #region Applying propeties

            Log("Creating $propeties for $controls");

            MainPanel.Dock = DockStyle.Fill;
            MainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            MainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            MainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250F));
            MainPanel.ColumnCount = 2;
            MainPanel.RowCount = 1;
            LeftPanel.Dock = DockStyle.Fill;
            LeftPanel.Margin = new Padding(0);
            RightPanel.BackColor = Color.White;
            RightPanel.Dock = DockStyle.Fill;
            RightPanel.Location = new Point(434, 0);
            RightPanel.Margin = new Padding(0);
            RightPanel.Size = new Size(250, 561);
            UpperLeftPanel.BackColor = System.Drawing.Color.FromArgb(35, 35, 35);
            UpperLeftPanel.Dock = DockStyle.Top;
            UpperLeftPanel.Size = new System.Drawing.Size(434, 50);
            UpperRightPanel.BackColor = Color.FromArgb(192, 0, 192);
            UpperRightPanel.Dock = DockStyle.Top;
            UpperRightPanel.Location = new Point(0, 0);
            UpperRightPanel.Size = new Size(250, 50);
            ItemsPanel.AutoScroll = true;
            ItemsPanel.BorderStyle = BorderStyle.FixedSingle;
            ItemsPanel.Dock = DockStyle.Fill;
            ItemsPanel.Location = new Point(0, 50);
            OptionsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            OptionsPanel.Dock = DockStyle.Fill;
            OptionsPanel.Font = new Font("Microsoft YaHei", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            OptionsPanel.Location = new Point(0, 50);
            OptionsPanel.Size = new Size(250, 511);
            TitleLabel.BackColor = Color.Transparent;
            TitleLabel.Dock = DockStyle.Left;
            TitleLabel.Font = new Font("Century Gothic", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            TitleLabel.ForeColor = Color.White;
            TitleLabel.Location = new Point(0, 0);
            TitleLabel.Size = new Size(350, 50);
            TitleLabel.Text = "Simple Download Manager";
            TitleLabel.TextAlign = ContentAlignment.MiddleCenter;
            TitleLabel.Click += Credits;
            SettingsLabel.BackColor = Color.Transparent;
            SettingsLabel.Dock = DockStyle.Fill;
            SettingsLabel.Font = new Font("Century Gothic", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            SettingsLabel.ForeColor = Color.White;
            SettingsLabel.Location = new Point(0, 0);
            SettingsLabel.Text = "          Settings";
            SettingsLabel.TextAlign = ContentAlignment.MiddleLeft;
            FolderLabel.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            FolderLabel.Location = new Point(3, 15);
            FolderLabel.Size = new Size(221, 20);
            FolderLabel.Text = "Downloads Folder:";
            AddLinksLabel.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            AddLinksLabel.Location = new Point(5, 90);
            AddLinksLabel.Size = new Size(221, 20);
            AddLinksLabel.Text = "Links to add:";
            DownloadButton.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            DownloadButton.BackColor = Color.Gainsboro;
            DownloadButton.FlatStyle = FlatStyle.Flat;
            DownloadButton.Font = new Font("Century Gothic", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            DownloadButton.Location = new Point(5, 448);
            DownloadButton.Size = new System.Drawing.Size(163, 50);
            DownloadButton.Text = "DOWNLOAD";
            DownloadButton.UseVisualStyleBackColor = false;
            DownloadButton.Click += DownloadRequest;
            BrowseButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BrowseButton.FlatStyle = FlatStyle.Flat;
            BrowseButton.Location = new Point(199, 38);
            BrowseButton.Size = new Size(40, 25);
            BrowseButton.Text = ". . .";
            BrowseButton.UseVisualStyleBackColor = true;
            BrowseButton.Click += Browse;
            RemFinsihedButton.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            RemFinsihedButton.BackColor = Color.Transparent;
            RemFinsihedButton.FlatStyle = FlatStyle.Flat;
            RemFinsihedButton.Font = new Font("Century Gothic", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            RemFinsihedButton.Location = new Point(5, 417);
            RemFinsihedButton.Size = new Size(232, 25);
            RemFinsihedButton.Text = "Remove finished ones.";
            RemFinsihedButton.UseVisualStyleBackColor = false;
            RemFinsihedButton.Click += RemoveFinishedOnes;
            RemFailedButton.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            RemFailedButton.BackColor = Color.Transparent;
            RemFailedButton.FlatStyle = FlatStyle.Flat;
            RemFailedButton.Font = new Font("Century Gothic", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            RemFailedButton.Location = new Point(5, 386);
            RemFailedButton.Size = new Size(232, 25);
            RemFailedButton.Text = "Remove failed ones.";
            RemFailedButton.UseVisualStyleBackColor = false;
            RemFailedButton.Click += RemoveFailedOnes;
            MoreIcon.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            MoreIcon.Location = new Point(393, 9);
            MoreIcon.Image = new Bitmap("assets\\icons\\menu");
            MoreIcon.Size = new Size(32, 32);
            MoreIcon.MouseClick += MenuContextMenu;
            AddButtonIcon.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            AddButtonIcon.Image = new Bitmap("assets\\icons\\add");
            AddButtonIcon.Location = new Point(187, 448);
            AddButtonIcon.Size = new Size(50, 50);
            AddButtonIcon.SizeMode = PictureBoxSizeMode.StretchImage;
            AddButtonIcon.Click += AddLinksToQueue;
            FolderTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            FolderTextBox.BackColor = Color.White;
            FolderTextBox.BorderStyle = BorderStyle.FixedSingle;
            FolderTextBox.Font = new Font("Microsoft YaHei", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FolderTextBox.Location = new Point(5, 38);
            FolderTextBox.ReadOnly = true;
            FolderTextBox.Size = new Size(186, 25);
            FolderTextBox.Text = "$desktop\\SDM_Downloads";
            LinksTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            LinksTextBox.BackColor = Color.White;
            LinksTextBox.BorderStyle = BorderStyle.FixedSingle;
            LinksTextBox.Font = new Font("Microsoft YaHei", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            LinksTextBox.Location = new Point(5, 113);
            LinksTextBox.MaxLength = 2_097_152;
            LinksTextBox.Multiline = true;
            LinksTextBox.ScrollBars = ScrollBars.Both;
            LinksTextBox.Size = new Size(232, 204);
            MonitorClipboardCheck.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            MonitorClipboardCheck.AutoSize = true;
            MonitorClipboardCheck.Checked = true;
            MonitorClipboardCheck.FlatStyle = FlatStyle.Flat;
            MonitorClipboardCheck.Location = new Point(5, 323);
            MonitorClipboardCheck.Size = new Size(200, 23);
            MonitorClipboardCheck.Text = "Automatic insert";
            MonitorClipboardCheck.UseVisualStyleBackColor = true;
            DownloadProgressBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            DownloadProgressBar.Location = new Point(5, 357);
            DownloadProgressBar.Size = new Size(232, 23);
            ClipboardMonitoringLoop.Enabled = true;
            ClipboardMonitoringLoop.Interval = 50;
            ClipboardMonitoringLoop.Tick += MonitorClipboard;

            Log("DONE\n");

            #endregion

            #region Combining Controls

            Log("Combining and showing $controls");

            Controls.Add(MainPanel);
            MainPanel.Controls.Add(this.LeftPanel, 0, 0);
            MainPanel.Controls.Add(this.RightPanel, 1, 0);
            LeftPanel.Controls.Add(this.ItemsPanel);
            LeftPanel.Controls.Add(this.UpperLeftPanel);
            RightPanel.Controls.Add(this.OptionsPanel);
            RightPanel.Controls.Add(this.UpperRightPanel);
            UpperLeftPanel.Controls.Add(this.MoreIcon);
            UpperLeftPanel.Controls.Add(this.TitleLabel);
            UpperRightPanel.Controls.Add(this.SettingsLabel);
            OptionsPanel.Controls.Add(this.DownloadProgressBar);
            OptionsPanel.Controls.Add(this.MonitorClipboardCheck);
            OptionsPanel.Controls.Add(this.DownloadButton);
            OptionsPanel.Controls.Add(this.BrowseButton);
            OptionsPanel.Controls.Add(this.RemFinsihedButton);
            OptionsPanel.Controls.Add(this.RemFailedButton);
            OptionsPanel.Controls.Add(this.LinksTextBox);
            OptionsPanel.Controls.Add(this.FolderTextBox);
            OptionsPanel.Controls.Add(this.AddButtonIcon);
            OptionsPanel.Controls.Add(this.FolderLabel);
            OptionsPanel.Controls.Add(this.AddLinksLabel);

            Log("DONE\n");

            #endregion

            Log("Created $form\n");
        }

        #region Events

        public void Exiting(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void AddLinksToQueue(object sender, EventArgs e)
        {
            if (!IsProcessing)
            {
                IsProcessing = true;
                DownloadProgressBar.Value = 0;
                DownloadProgressBar.Maximum = LinksTextBox.Lines.Length;
                Thread thread = new Thread(new ThreadStart(() => AddLinksAsync()));
                thread.Start();
            }
            else
            {
                MessageBox.Show("Any process is already running.", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        public void DownloadRequest(object sender, EventArgs e)
        {
            if (!IsProcessing)
            {
                Log("Download got requested.\n");
                IsProcessing = true;
                DownloadProgressBar.Value = 0;
                DownloadProgressBar.Maximum = ItemsPanel.Controls.Count;
                Log("Creating second thread.\n");
                Thread thread = new Thread(new ThreadStart(() => DownloadAll()));
                thread.Start();
            }
            else
            {
                MessageBox.Show("Any process is already running.", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        public void Browse(object sender, EventArgs e)
        {
            Log("Creating $FolderBrowserDialog\n");
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                FolderTextBox.Text = Path = fbd.SelectedPath;
                Log("Changed $Settings.$Path\n");
            }
        }

        public void RemoveFinishedOnes(object sender, EventArgs e)
        {
            if (!IsProcessing)
            {
                IsProcessing = true;
                DownloadProgressBar.Value = 0;
                DownloadProgressBar.Maximum = ItemsPanel.Controls.Count;
                Thread thread = new Thread(new ThreadStart(() => RemoveAsync(1)));
                thread.Start();
            }
            else
            {
                MessageBox.Show("Any process is already running.", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        public void RemoveFailedOnes(object sender, EventArgs e)
        {
            if (!IsProcessing)
            {
                IsProcessing = true;
                DownloadProgressBar.Value = 0;
                DownloadProgressBar.Maximum = ItemsPanel.Controls.Count;
                Thread thread = new Thread(new ThreadStart(() => RemoveAsync(-1)));
                thread.Start();
            }
            else
            {
                MessageBox.Show("Any process is already running.", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        public void MonitorClipboard(object sender, EventArgs e)
        {
            if (MonitorClipboardCheck.Checked)
            {
                if (ClipboardState != Clipboard.GetText())
                {
                    LinksTextBox.Text += "\r\n" + Clipboard.GetText();
                    Log("Clipboard changed. --> Adding new link.\n");
                    ClipboardState = Clipboard.GetText();
                }
            }
        }

        public void Credits(object sender, EventArgs e)
        {
            MessageBox.Show("SIMPLE DOWNLOAD MANAGER (SDM)\n" +
                            "========================================================\n" +
                            "Programmed by Christian Schlei\n" +
                            "in 2024\n" +
                            "\n" +
                            "Icons: Flaticon.com\n" +
                            "https://www.flaticon.com/free-icons/delete\n" +
                            "https://www.flaticon.com/free-icons/tick\n" +
                            "https://www.flaticon.com/free-icons/pendin\n" +
                            "https://www.flaticon.com/free-icons/more\n" +
                            "https://www.flaticon.com/free-icons/ui\n" +
                            "https://www.flaticon.com/free-icons/folder\n" +
                            "https://www.flaticon.com/free-icons/more\n" +
                            "https://www.flaticon.com/free-icons/down-arrow</a>",
                            "CREDITS OF SDM", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void BeforeExit(object sender, FormClosingEventArgs e)
        {
            e.Cancel = IsProcessing;
            Settings.DownloadsPath = Path;
            Settings.Save();
        }

        public void MenuContextMenu(object sender, MouseEventArgs e)
        {
            ContextMenu newmenu = new ContextMenu();
            newmenu.MenuItems.Add("Remove failed ones", RemoveFailedOnes);
            newmenu.MenuItems.Add("Remove finished ones", RemoveFinishedOnes);
            newmenu.MenuItems.Add("Expand all", ExpandAll);
            newmenu.MenuItems.Add("Shrink all", ShrinkAll);
            newmenu.MenuItems.Add("Save current queue as", SaveQueue);
            newmenu.MenuItems.Add("Load items from file", LoadQueue);
            newmenu.MenuItems.Add("Export List to EXE", ExportRequest);
            newmenu.MenuItems.Add("Clear log console", (sen, ev) => { Console.Clear(); });
            newmenu.MenuItems.Add("Clear download queue", ClearQueue);
            newmenu.MenuItems.Add("Checksum verifier", OpenVerifier);
            newmenu.MenuItems.Add("Open Queue Manager", OpenManager);
            MoreIcon.ContextMenu = newmenu;
            newmenu.Show(MoreIcon, new Point(16, 16));
        }

        public void ExpandAll(object sender, EventArgs e)
        {
            foreach(Control c in ItemsPanel.Controls)
            {
                PendingItem item = c as PendingItem;
                item.Expand();
            }
        }

        public void ShrinkAll(object sender, EventArgs e)
        {
            foreach (Control c in ItemsPanel.Controls)
            {
                PendingItem item = c as PendingItem;
                item.Shrink();
            }
        }

        public void OpenManager(object sender, EventArgs e)
        {
            if (!IsProcessing)
            {
                MessageBox.Show("This process might take some time during longer queues.\n" +
                    "The application cannot respond properly during these action.\n" +
                    "Don't close the application it will respond after some moments.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                IsProcessing = true;
                DownloadProgressBar.Value = 0;

                Queue queue = new Queue();
                queue.Items = new List<QueueItem>();

                foreach (Control c in ItemsPanel.Controls)
                {
                    PendingItem item = c as PendingItem;
                    QueueItem qi = new QueueItem();
                    qi.FileName = item.FileName;
                    qi.Url = item.Url;
                    qi.State = 0;
                    qi.Checksum = item.Checksum;
                    queue.Items.Add(qi);
                }

                ItemsPanel.Controls.Clear();

                QueueManagement qm = new QueueManagement(queue);
                qm.ShowDialog();

                Queue newqueue = new Queue();
                newqueue.Items = new List<QueueItem>();
                for(int i = qm.CurrentQueue.Items.Count - 1; i >= 0; i--)
                {
                    newqueue.Items.Add(qm.CurrentQueue.Items[i]);
                }

                DownloadProgressBar.Maximum = qm.CurrentQueue.Items.Count;

                Thread thread = new Thread(new ThreadStart(() => LoadAsync(newqueue)));
                thread.Start();
            }
            else
            {
                MessageBox.Show("Any process is already running.", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        public void OpenVerifier(object sender, EventArgs e) 
        {
            MessageBox.Show("This process might take some time during longer queues.\n" +
                "The application cannot respond properly during these action.\n" +
                "Don't close the application it will respond after some moments.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Queue queue = new Queue();
            queue.Items = new List<QueueItem>();

            foreach (Control c in ItemsPanel.Controls)
            {
                PendingItem item = c as PendingItem;
                QueueItem qi = new QueueItem();
                qi.FileName = item.FileName;
                qi.Url = item.Url;
                qi.State = 0;
                queue.Items.Add(qi);
            }

            QueueVerfier qv = new QueueVerfier(queue);
            qv.ShowDialog();
        }

        public void ExportRequest(object sender, EventArgs e)
        {
            if (!IsProcessing)
            {
                IsProcessing = true;
                DownloadProgressBar.Value = 0;
                DownloadProgressBar.Maximum = ItemsPanel.Controls.Count + ((1 / 10) * ItemsPanel.Controls.Count);
                Thread thread = new Thread(new ThreadStart(() => ExportAsync()));
                thread.Start();
            }
            else
            {
                MessageBox.Show("Any process is already running.", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        public void SaveQueue(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Save Queue to file";
            sfd.Filter = "Simple Download Manager (SDM) File |*.sdm|Binary File |*.bin|Dat File |*.dat";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (!IsProcessing)
                {
                    string path = sfd.FileName;

                    IsProcessing = true;
                    DownloadProgressBar.Value = 0;
                    DownloadProgressBar.Maximum = ItemsPanel.Controls.Count;
                    Thread thread = new Thread(new ThreadStart(() => SaveAsync(path)));
                    thread.Start();
                }
                else
                {
                    MessageBox.Show("Any process is already running.", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }

        public void LoadQueue(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Load queue from file";
            ofd.Filter = "Simple Download Manager (SDM) File |*.sdm|Binary File |*.bin|Dat File |*.dat";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (!IsProcessing)
                {
                    try
                    {
                        string path = ofd.FileName;

                        IsProcessing = true;
                        DownloadProgressBar.Value = 0;


                        Queue nql = Queue.Load(ofd.FileName);

                        DownloadProgressBar.Maximum = nql.Items.Count + 25;

                        DownloadProgressBar.Value = 25;

                        Thread t = new Thread(new ThreadStart(() => LoadAsync(nql)));
                        t.Start();
                    }
                    catch (Exception ex)
                    {
                        Log($"An error occured:\nMessage: {ex.Message}\n{new string('=', 32)}\n{ex}\n");
                    }

                }
                else
                {
                    MessageBox.Show("Any process is already running.", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }

        public void ClearQueue(object sender, EventArgs e)
        {
            if (!IsProcessing)
            {
                IsProcessing = true;
                DownloadProgressBar.Value = 0;
                DownloadProgressBar.Maximum = ItemsPanel.Controls.Count;

                Thread thread = new Thread(new ThreadStart(() => ClearAsync()));
                thread.Start();
            }
            else
            {
                MessageBox.Show("Any process is already running.", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        #endregion

        #region ForAsync

        public void DownloadAll()
        {
            if(Settings.ResetStatus)
            {
                foreach (Control control in ItemsPanel.Controls)
                {
                    PendingItem item = control as PendingItem;
                    item.State = 0;
                }
            }

            foreach (Control control in ItemsPanel.Controls)
            {
                PendingItem item = control as PendingItem;

                if (item.State != 0)
                {
                    continue;
                }

                using (WebClient client = new WebClient())
                {
                    Log($"\"SDM-GUI.dll, $MainForn.$DownloadAll\" \"{item.Url}\" \"{Path + "\\" + item.FileName}\"\n");

                    try
                    {
                        client.DownloadFile(item.Url, Path + "\\" + item.FileName);
                        item.State = 1;
                    }
                    catch (Exception ex)
                    {
                        Log($"An error occured:\nMessage: {ex.Message}\n{new string('=', 32)}\n{ex}\n");
                        item.State = -1;
                    }

                    DownloadProgressBar.Invoke((MethodInvoker)(() => DownloadProgressMade()));
                }
            }

            DownloadProgressBar.Invoke((MethodInvoker)(() => { DownloadProgressBar.Value = DownloadProgressBar.Maximum; }));
            IsProcessing = false;

            WindowsMediaPlayer wmp = new WindowsMediaPlayer();
            wmp.URL = "assets\\audio\\buz\\buz-01.wav";
            wmp.controls.play();
        }

        public void DownloadProgressMade()
        {
            DownloadProgressBar.Value++;
        }

        public void AddLinksAsync()
        {
            foreach (string i in LinksTextBox.Lines)
            {
                string[] uriparts = i.Split('/');
                string filename = uriparts[uriparts.Length - 1];

                if (i == "") continue;

                Log($"Adding to queue: {i} as {filename}\n");

                ItemsPanel.Invoke((MethodInvoker)(() => LinkAdding(filename, i)));
                DownloadProgressBar.Invoke((MethodInvoker)(() => DownloadProgressMade()));
            }

            LinksTextBox.Invoke(new MethodInvoker(() => { LinksTextBox.Text = ""; }));
            DownloadProgressBar.Invoke((MethodInvoker)(() => { DownloadProgressBar.Value = DownloadProgressBar.Maximum; }));
            IsProcessing = false;
        }

        public void LinkAdding(string filename, string i)
        {
            ItemsPanel.Controls.Add(new PendingItem() { FileName = filename, Url = i });
        }

        public void RemoveAsync(int state)
        {
            for (int a = 1; a <= 10000; a++)
            {
                foreach (Control c in ItemsPanel.Controls)
                {
                    PendingItem item = c as PendingItem;

                    if (item.State == state)
                    {
                        ItemsPanel.Invoke((MethodInvoker)(() => ItemRemoving(item)));
                        DownloadProgressBar.Invoke((MethodInvoker)(() => DownloadProgressMade()));
                        Log("Removed item: " + item.FileName + "\n");
                    }
                }
            }

            DownloadProgressBar.Invoke((MethodInvoker)(() => { DownloadProgressBar.Value = DownloadProgressBar.Maximum; }));
            IsProcessing = false;
        }

        public void ItemRemoving(PendingItem item)
        {
            ItemsPanel.Controls.Remove(item);
        }

        public void ExportAsync()
        {
            string temp = Path + "\\Exporttemp";

            if (!Directory.Exists(temp))
            {
                Directory.CreateDirectory(temp);
            }
            else
            {
                foreach (string f in Directory.GetFiles(temp))
                {
                    File.Delete(f);
                }
            }

            QueueList ql = new QueueList();
            ql.Items = new List<DownloadLink>();

            foreach (Control i in ItemsPanel.Controls)
            {
                PendingItem item = i as PendingItem;

                DownloadLink dl = new DownloadLink();
                dl.Url = item.Url;
                dl.FileName = item.FileName;

                ql.Items.Add(dl);

                DownloadProgressBar.Invoke((MethodInvoker)(() => DownloadProgressMade()));
            }

            ql.Save(Path + "\\Exporttemp");

            ExportModuleSelector ems = new ExportModuleSelector();
            ems.Path = Path;
            if (ems.ShowDialog() == DialogResult.OK)
            {
                DownloadProgressBar.Invoke((MethodInvoker)(() => { DownloadProgressBar.Value = DownloadProgressBar.Maximum; }));
                IsProcessing = false;
            }
        }

        public void SaveAsync(string path)
        {
            Queue queue = new Queue();
            queue.Items = new List<QueueItem>();

            foreach (Control c in ItemsPanel.Controls)
            {
                PendingItem item = c as PendingItem;

                QueueItem qi = new QueueItem();
                qi.FileName = item.FileName;
                qi.Url = item.Url;
                qi.State = item.State;
                qi.Checksum = item.Checksum;

                DownloadProgressBar.Invoke((MethodInvoker)(() => DownloadProgressMade()));

                queue.Items.Add(qi);
            }

            queue.Save(path);

            DownloadProgressBar.Invoke((MethodInvoker)(() => { DownloadProgressBar.Value = DownloadProgressBar.Maximum; }));
            IsProcessing = false;
        }

        public void LoadAsync(Queue nql)
        {
            foreach (QueueItem qi in nql.Items)
            {
                PendingItem pi = new PendingItem();
                pi.FileName = qi.FileName;
                pi.Url = qi.Url;
                pi.State = qi.State;
                pi.Checksum = qi.Checksum;

                ItemsPanel.Invoke(new MethodInvoker(() => { ItemsPanel.Controls.Add(pi); }));
                DownloadProgressBar.Invoke((MethodInvoker)(() => { DownloadProgressBar.Value++; }));
            }

            DownloadProgressBar.Invoke((MethodInvoker)(() => { DownloadProgressBar.Value = DownloadProgressBar.Maximum; }));
            IsProcessing = false;
        }

        public void LoadAsyncNoGui(Queue nql)
        {
            foreach (QueueItem qi in nql.Items)
            {
                PendingItem pi = new PendingItem();
                pi.FileName = qi.FileName;
                pi.Url = qi.Url;
                pi.State = qi.State;

                ItemsPanel.Controls.Add(pi);
            }
        }

        public void ClearAsync()
        {
            for (int a = 1; a < 10; a++)
            {
                foreach (Control c in ItemsPanel.Controls)
                {
                    PendingItem item = c as PendingItem;

                    ItemsPanel.Invoke((MethodInvoker)(() => ItemRemoving(item)));
                    DownloadProgressBar.Invoke((MethodInvoker)(() => DownloadProgressMade()));
                    Log("Removed item: " + item.FileName + "\n");
                }
            }

            DownloadProgressBar.Invoke((MethodInvoker)(() => { DownloadProgressBar.Value = DownloadProgressBar.Maximum; }));
            IsProcessing = false;
        }

        #endregion
    }
}