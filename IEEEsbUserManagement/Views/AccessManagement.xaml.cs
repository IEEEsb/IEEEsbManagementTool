using IEEEsbUserManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IEEEsbUserManagement.Views
{
    /// <summary>
    /// Lógica de interacción para AccessManagement.xaml
    /// </summary>
    public partial class AccessManagement : Page
    {
        public AccessManagement()
        {
            InitializeComponent();
        }

        private bool checkIfFieldsAreBlank()
        {
            return DNI.Text != null && Email.Text != null && DNI.Text != string.Empty && Email.Text != string.Empty;
        }

        private void GenerateNewRegisterID_Click(object sender, RoutedEventArgs e)
        {
            if (checkIfFieldsAreBlank())
            {
                AccessManagementViewModel.Current.Monitor.Working = true;
                AccessManagementViewModel.Current.GenerateNewRegisterID(DNI.Text, Email.Text);
            }
            else MessageBox.Show("All fields are compulsory", "Access management");
        }

        private void RevokeAccess_Click(object sender, RoutedEventArgs e)
        {
            if (checkIfFieldsAreBlank())
            {
                AccessManagementViewModel.Current.Monitor.Working = true;
                AccessManagementViewModel.Current.RevokeAccessToken(DNI.Text);
            }
            else MessageBox.Show("All fields are compulsory", "Access management");
        }
    }
}
