using System.Windows;

using LedgerClient.Infrastructure;

namespace LedgerClient.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = Tools.Locator.MainViewModel;
        }
    }
}
