using SDM_GUI;
using System;
using System.Windows.Forms;

namespace SimpleDownloadManager
{
    public static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Log("Enabled Visual Styles\n");
            Log("Gettings access to .\\libraries\\SDM-GUI.dll\n");
            
            
            try
            {
                if(args.Length != 0)
                {
                    Log("$args found.\n");
                    Log("Run SDM-GUI.dll, $MainForm..$ctor\n");
                    Application.Run(new MainForm(args[0]));
                }
                else
                {
                    Log("Run SDM-GUI.dll, $MainForm..$ctor\n");
                    Application.Run(new MainForm());
                }
                
            }
            catch(Exception ex)
            {
                Log($"An error occured:\nMessage: {ex.Message}\n{new string('=', 32)}\n{ex}\n");
            }

            Log("\n\n Press enter to exit console . . . ");
            Console.ReadLine();
        }

        public static void Log(string text)
        {
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}
