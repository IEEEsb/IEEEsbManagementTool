using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEEEsbUserManagement.Model
{
    public class User:INotifyPropertyChanged
    {
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                NotifyPropertyChanged("Name");
            }
        }

        private string username;
        public string UserName
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
                NotifyPropertyChanged("UserName");
            }
        }

        private string surnames;
        public string Surnames
        {
            get
            {
                return surnames;
            }
            set
            {
                surnames = value;
                NotifyPropertyChanged("Surnames");
            }
        }

        private string email;
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
                NotifyPropertyChanged("Email");
            }
        }

        private string phone;
        public string Phone
        {
            get
            {
                return phone;
            }
            set
            {
                phone = value;
                NotifyPropertyChanged("Phone");
            }
        }

        private string dni;
        public string DNI
        {
            get
            {
                return dni;
            }
            set
            {
                dni = value;
                NotifyPropertyChanged("DNI");
            }
        }

        private string ieeen;
        public string IEEEn
        {
            get
            {
                return ieeen;
            }
            set
            {
                ieeen = value;
                NotifyPropertyChanged("IEEEn");
            }
        }

        private string registrationID;
        public string RegistrationID
        {
            get
            {
                return registrationID;
            }
            set
            {
                registrationID = value;
                NotifyPropertyChanged("RegistrationID");
            }
        }

        public void Clear()
        {
            Name = string.Empty;
            UserName = string.Empty;
            Surnames = string.Empty;
            Email = string.Empty;
            Phone = string.Empty;
            DNI = string.Empty;
            IEEEn = string.Empty;
        }

        public override string ToString()
        {
            return "[DNI=" + DNI + ", Nombre=" + Name + "]";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(String property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
