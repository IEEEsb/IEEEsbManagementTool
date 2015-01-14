using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEEEsbUserManagement.Model
{
    public class WorkerMonitor:INotifyPropertyChanged
    {
        private bool working;
        public bool Working
        {
            get
            {
                return working;
            }
            set
            {
                working = value;
                NotifyPropertyChanged("Working");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(String property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
