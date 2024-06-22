using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace SDM_GUI
{
    [Serializable]
    public class Queue
    {
        public Queue()
        {
            
        }

        public List<QueueItem> Items { get; set; }

        public void Save(string path)
        {
            try
            {
                if(File.Exists(path))
                {
                    File.Delete(path);
                }

                FileStream FS = new FileStream(path, FileMode.Create);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(FS, this);
                FS.Dispose();
                FS.Close();

                MainForm.Log($"$ql saved to $path satta [$path=\"{path}\"]");
            }
            catch (Exception ex)
            {
                MainForm.Log($"An error occured:\nMessage: {ex.Message}\n{new string('=', 32)}\n{ex}\n");
            }
        }

        public static Queue Load(string path)
        {
            MainForm.Log($"Trying to load $data << $content dä $file << $path satta [$path=\"{path}\"]");

            FileStream fs = new FileStream(path, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            object obj = bf.Deserialize(fs);
            fs.Close();
            fs.Dispose();
            MainForm.Log("$data << .$ANS loaded.");

            return (Queue)obj;
        }
    }

    [Serializable]
    public class QueueItem
    {
        public QueueItem()
        {
            
        }

        public string Url { get; set; }

        public string FileName { get; set; }

        public string Checksum { get; set; }

        public int State { get; set; }
    }
}
