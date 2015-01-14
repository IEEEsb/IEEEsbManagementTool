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
    /// Lógica de interacción para LatchManagement.xaml
    /// </summary>
    public partial class LatchManagement : Page
    {
        public LatchManagement()
        {
            InitializeComponent();
        }

        private void Pair_Click(object sender, RoutedEventArgs e)
        {
            LatchManagementViewModel.Current.Monitor.Working = true;
            LatchManagementViewModel.Current.Pair(DNI.Text, PairingToken.Text);
        }

        private void Unpair_Click(object sender, RoutedEventArgs e)
        {
            LatchManagementViewModel.Current.Monitor.Working = true;
            LatchManagementViewModel.Current.Unpair(DNI.Text);
        }
    }
}
