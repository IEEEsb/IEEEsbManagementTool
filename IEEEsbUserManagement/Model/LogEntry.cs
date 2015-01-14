using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEEEsbUserManagement.Model
{
    public class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public LogConnection.TYPE Type { get; set; }
        public LogConnection.SUBTYPE Subtype { get; set; }
        public string Message { get; set; }
    }
}
