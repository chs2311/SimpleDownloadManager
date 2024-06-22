using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SDM_GUI
{
    [Serializable]
    public class Settings
    {
        public string DownloadsPath { get; set; }
        public bool OpenConsole { get; set; }
        public string FinSndPath { get; set; }
        public bool ResetStatus { get; set; }

        public void Save()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\SDM-CFG.CFG";

            try
            {
                FileStream fs = new FileStream(path, FileMode.Create);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, this);
                fs.Close();
                fs.Dispose();
            }
            catch (Exception ex)
            {
                Log($"An error occured:\nMessage: {ex.Message}\n{new string('=', 32)}\n{ex}\n");
            }
        }

        public static Settings Load()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\SDM-CFG.CFG";

            try
            {
                if (!File.Exists(path))
                {
                    return new Settings()
                    {
                        DownloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\SDM-FILES",
                        FinSndPath = "assets\\audio\\Fin\\fin-1.wav",
                        OpenConsole = true,
                        ResetStatus = true
                    };
                }

                FileStream fs = new FileStream(path, FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                object obj = bf.Deserialize(fs);
                fs.Close();
                fs.Dispose();
                return (Settings)obj;
            }
            catch (Exception ex)
            {
                Log($"An error occured:\nMessage: {ex.Message}\n{new string('=', 32)}\n{ex}\n");

                return new Settings()
                {
                    DownloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\SDM-FILES",
                    OpenConsole = true
                };
            }
        }

        public static void Log(string text)
        {
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}
