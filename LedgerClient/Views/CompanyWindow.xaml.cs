using System;
using System.Windows;

using LedgerClient.ViewModels;

namespace LedgerClient.Views
{
    /// <summary>
    /// Interaction logic for CompanyWindow.xaml
    /// </summary>
    public partial class CompanyWindow : Window
    {
        public CompanyWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ((CompanyViewModel)DataContext).FocusRequested += Focus;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            ((CompanyViewModel)DataContext).FocusRequested -= Focus;
        }

        private void Focus(object sender, EventArgs e) => tbName.Focus();
    }
}
