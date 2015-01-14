using IEEEsbUserManagement.Model;
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
    /// Lógica de interacción para LogViewer.xaml
    /// </summary>
    public partial class LogViewer : Page
    {
        public LogViewer()
        {
            InitializeComponent();
        }

        private void UpdateLog_Click(object sender, RoutedEventArgs e)
        {
            LogWindow.ScrollIntoView(LogViewModel.Current.Update());
        }
    }
}
