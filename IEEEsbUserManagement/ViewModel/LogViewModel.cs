using IEEEsbUserManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEEEsbUserManagement.ViewModel
{
    public class LogViewModel
    {
        public static LogViewModel Current { get; set; }
        public PropertiesFileHandler Properties { get; set; }
        public LogHistory Log {get; set;}

        public LogViewModel()
        {
            Current = this;
            Log = new LogHistory();
            Properties = new PropertiesFileHandler();
            LogConnection.Initialize(Properties);
        }

        public LogEntry Update() 
        {
            LogEntry LastEntry = null;
            foreach(LogEntry entry in LogConnection.RetrieveFullLog())
            {
                if(!Log.FullLog.Contains(entry))
                {
                    Log.FullLog.Add(entry);
                }
                LastEntry = entry;
            }
            return LastEntry;
        }
    }
}
