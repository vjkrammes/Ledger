using System.Windows;

using LedgerClient.ViewModels;

namespace LedgerClient.Views
{
    /// <summary>
    /// Interaction logic for IdentityWindow.xaml
    /// </summary>
    public partial class IdentityWindow : Window
    {
        public IdentityWindow()
        {
            InitializeComponent();
        }

        private void pbPassword1_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ((IdentityViewModel)DataContext).Password1 = pbPassword1.Password;
        }

        private void pbPassword2_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ((IdentityViewModel)DataContext).Password2 = pbPassword2.Password;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                pbPassword1.Password = ((IdentityViewModel)DataContext).Password1;
                pbPassword2.Password = ((IdentityViewModel)DataContext).Password2;
            }
        }
    }
}
