using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IMDB.Data.Objects
{
    public class Logger
    {
        public string LogLocation { get; set; }
        public Logger(string logLocation)
        {
            LogLocation = logLocation;
        }

        public void WriteToFile(string message)
        {
            try
            {
                using (FileStream s = File.Open(LogLocation, FileMode.Append, FileAccess.Write, FileShare.Write))
                {
                    string dataTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
                    byte[] info = new UTF8Encoding(true).GetBytes($"{Environment.NewLine} {dataTime} {message}");
                    s.Write(info, 0, info.Length);
                }
            }
            catch (Exception fileException)
            {

            }
        }

        public void WriteToFile(string message, object objectToPrint)
        {
            this.WriteToFile(message + JsonConvert.SerializeObject(objectToPrint));
        }
    }
}
