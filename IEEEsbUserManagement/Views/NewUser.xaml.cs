using IEEEsbUserManagement.Model;
using IEEEsbUserManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IEEEsbUserManagement.Views
{
    /// <summary>
    /// Lógica de interacción para NewUser.xaml
    /// </summary>
    public partial class NewUser : Page
    {
        public NewUser()
        {
            InitializeComponent();
        }
        

        private bool checkIfFieldsAreBlank()
        {
            User user = NewUserViewModel.Current.CurrentUser;
            bool checkNull = user.Name != null && user.IEEEn != null && user.Email != null && user.Phone != null && user.Surnames != null && user.DNI != null && Password.Password != null;
            bool checkEmpty = user.Name != string.Empty && user.IEEEn != string.Empty && user.Email != string.Empty && user.Phone != string.Empty && user.Surnames != string.Empty
                && user.DNI != string.Empty && Password.Password != string.Empty;
            return checkEmpty && checkNull;
        }



        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (NewUserViewModel.Current.Monitor.Working) return;
            if(!checkIfFieldsAreBlank())
            {
                MessageBox.Show("All fields are compulsory", "Error");
                return;
            }
            int DNILetter = 0;
            if(Phone.Text.Length != 9)
            {
                MessageBox.Show("Incorrect phone number", "Error");
                return;
            }
            if(DNI.Text.Length != 9 || Int32.TryParse(DNI.Text.Substring(8), out DNILetter) || IEEEnumber.Text.Length != 8)
            {
                MessageBox.Show("Incorrect DNI or IEEE number", "Error");
                return;
            }
            if(!Password.Password.Equals(ConfirmPassword.Password) || Password.Password.Length < 6)
            {
                MessageBox.Show("Passwords don't match or are less than 6 characters long", "Error");
                Password.Password = string.Empty;
                ConfirmPassword.Password = string.Empty;
                return;
            }
            else
            {
                NewUserViewModel.Current.Monitor.Working = true;
                NewUserViewModel.Current.Register(Password.Password);
                Password.Password = string.Empty;
                ConfirmPassword.Password = string.Empty;
            }
        }
    }
}
