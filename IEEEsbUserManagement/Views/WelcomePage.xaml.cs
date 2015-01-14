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
    /// Lógica de interacción para WelcomePage.xaml
    /// </summary>
    public partial class WelcomePage : Page
    {
        public WelcomePage()
        {
            InitializeComponent();
        }

        private void NewUser_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new NewUser());
        }

        private void AccessManagement_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new AccessManagement());
        }

        private void LatchManagement_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new LatchManagement());
        }

        private void Log_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new LogViewer());
        }
    }
}
