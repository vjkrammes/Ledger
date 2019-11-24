using System.Windows;

using LedgerClient.ViewModels;

namespace LedgerClient.Views
{
    /// <summary>
    /// Interaction logic for PasswordWindow.xaml
    /// </summary>
    public partial class PasswordWindow : Window
    {
        public PasswordWindow()
        {
            InitializeComponent();
        }

        private void pbPassword1_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                ((PasswordViewModel)DataContext).Password1 = pbPassword1.Password;
            }
        }

        private void pbPassword2_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                ((PasswordViewModel)DataContext).Password2 = pbPassword2.Password;
            }
        }
    }
}
