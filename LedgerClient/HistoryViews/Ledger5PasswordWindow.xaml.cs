using System.Windows;

using LedgerClient.HistoryViewModels;

namespace LedgerClient.HistoryViews
{
    /// <summary>
    /// Interaction logic for Ledger5PasswordWindow.xaml
    /// </summary>
    public partial class Ledger5PasswordWindow : Window
    {
        public Ledger5PasswordWindow()
        {
            InitializeComponent();
        }

        private void pbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                ((Ledger5PasswordViewModel)DataContext).Password = pbPassword.Password;
            }
        }
    }
}
