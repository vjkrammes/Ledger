using System.Windows;
using System.Windows.Input;

using LedgerClient.ViewModels;

namespace LedgerClient.Views
{
    /// <summary>
    /// Interaction logic for AccountNumberWindow.xaml
    /// </summary>
    public partial class AccountNumberWindow : Window
    {
        public AccountNumberWindow()
        {
            InitializeComponent();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                e.Handled = true;
                ((AccountNumberViewModel)DataContext).Cancel();
            }
        }
    }
}
