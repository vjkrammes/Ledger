using System;
using System.Windows;

using LedgerClient.ViewModels;

namespace LedgerClient.Views
{
    /// <summary>
    /// Interaction logic for AccountTypeWindow.xaml
    /// </summary>
    public partial class AccountTypeWindow : Window
    {
        public AccountTypeWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ((AccountTypeViewModel)DataContext).FocusRequested += Focus;
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            ((AccountTypeViewModel)DataContext).FocusRequested -= Focus;
        }

        private void Focus(object sender, EventArgs e)
        {
            tbDescription.Focus();
        }
    }
}
