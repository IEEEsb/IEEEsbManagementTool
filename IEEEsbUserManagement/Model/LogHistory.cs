using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEEEsbUserManagement.Model
{
    public class LogHistory
    {
        public ObservableCollection<LogEntry> FullLog {get; set;}

        public LogHistory()
        {
            FullLog = new ObservableCollection<LogEntry>();
        }
    }
}
