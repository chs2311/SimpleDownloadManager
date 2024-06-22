using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using QUEUEDAT;

namespace DatDownloader
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if(!File.Exists("QUEUE.DAT"))
            {
                Print("'QUEUE.DAT' File was not found. Please use SimpleDownloadManager to create this file.", ConsoleColor.Red);
            }
            else
            {
                if(!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\DatDownloader-DL"))
                {
                    Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\DatDownloader-DL");
                }

                try
                {
                    QueueList ql = QueueList.Load(Directory.GetCurrentDirectory());

                    foreach (DownloadLink dl in ql.Items)
                    {
                        Print($"Trying to download: {dl.Url}", ConsoleColor.Yellow);

                        try
                        {
                            using (WebClient wc = new WebClient())
                            {
                                wc.DownloadFile(dl.Url, Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\DatDownloader-DL\\" + dl.FileName);
                            }

                            Print($"SUCCESS", ConsoleColor.Green);
                        }
                        catch (Exception ex)
                        {
                            Print("ERROR: " + ex.Message, ConsoleColor.Red);
                        }
                    }

                    Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\DatDownloader-DL");
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        static void Print(string text, ConsoleColor color)
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = color;
            Console.WriteLine(text + "\n");
            Console.ResetColor();
        }
    }
}
