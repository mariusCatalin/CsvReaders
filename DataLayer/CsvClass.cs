using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{

    [Serializable]
    public class CsvClass
    {
        public int Id { get; set; }
        public DateTime? LogTime { get; set; }
        public string Action { get; set; }
        public string FolderPath { get; set; }
        public string Filename { get; set; }
        public string Username { get; set; }
        public string IpAdress { get; set; }
        public int? XferSize { get; set; }
        public double? Duration { get; set; }
        public string AgentBrand { get; set; }
        public string AgentVersion { get; set; }
        public int? Error { get; set; }

        public CsvClass()
        {

        }

        public CsvClass(DateTime? logTime, string action, string folderPath, string fileName, string username, string ipAdress, int? xferSize, double? duration, string agentBrand, string agentVersion, int? error)
        {
            LogTime = logTime;
            Action = action;
            FolderPath = folderPath;
            Filename = fileName;
            Username = username;
            IpAdress = ipAdress;
            XferSize = xferSize;
            Duration = duration;
            AgentBrand = agentBrand;
            AgentVersion = agentVersion;
            Error = error;
        }

    }
}
