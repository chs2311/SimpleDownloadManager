using CS_DownloadManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SDM_GUI
{
    public partial class ExportModuleSelector : Form
    {
        public string Path;
        public ExportModuleSelector()
        {
            InitializeComponent();
        }

        private void ExportModuleSelector_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Log("Generating $var");

            string output = Path + "\\SDM-EXPORT_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".EXE";
            string temp = Path + "\\Exporttemp";
            string sed = "";

            InputBox ib = new InputBox();
            ib.Input = output;
            ib.ShowDialog();
            output = ib.Input;

            if (radioButton1.Checked)
            {
                sed = string.Format(File.ReadAllText("assets\\export\\QuickSDM\\Template.sed"), output, temp);
                foreach (string file in Directory.GetFiles("assets\\export\\QuickSDM"))
                {
                    File.Copy(file, temp + "\\" + System.IO.Path.GetFileName(file));
                }
            }
            else if (radioButton2.Checked)
            {
                sed = string.Format(File.ReadAllText("assets\\export\\DatDownloader\\Template.sed"), output, temp);
                foreach (string file in Directory.GetFiles("assets\\export\\DatDownloader"))
                {
                    File.Copy(file, temp + "\\" + System.IO.Path.GetFileName(file));
                }
            }
            else
            {
                return;
            }

            File.WriteAllText(temp + "\\SDM-TMP.SED", sed);

            Process p = new Process();
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "C:\\Windows\\System32\\IEXPRESS.EXE";
            psi.Arguments = "/n SDM-TMP.SED";
            psi.UseShellExecute = false;
            psi.WorkingDirectory = temp;
            p.StartInfo = psi;

            Log("IEXPRESS /N \"" + temp + "\\SDM-TMP.SED\"");

            p.Start();
            p.WaitForExit();

            Log("DONE\n");

            DialogResult = DialogResult.OK;
        }

        public static void Log(string text)
        {
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}
