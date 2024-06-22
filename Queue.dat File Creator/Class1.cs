using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

namespace QUEUEDAT
{
    [Serializable]
    public class QueueList
    {
        public List<DownloadLink> Items;

        public QueueList()
        {
            Items = new List<DownloadLink>();
        }

        public void Save(string path)
        {
            FileStream fs = new FileStream(path + "\\QUEUE.DAT", FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, this);
            fs.Close();
            fs.Dispose();
        }

        public static QueueList Load(string path)
        {
            FileStream fs = new FileStream(path + "\\QUEUE.DAT", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            object obj = bf.Deserialize(fs);
            fs.Close();
            fs.Dispose();
            return (QueueList)obj;
        }
    }

    [Serializable]
    public class DownloadLink
    {
        public string Url;
        public string FileName;
    }

}
