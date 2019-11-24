using System;
using System.Windows;

using LedgerClient.ViewModels;

namespace LedgerClient.Views
{
    /// <summary>
    /// Interaction logic for PoolWindow.xaml
    /// </summary>
    public partial class PoolWindow : Window
    {
        public PoolWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ((PoolViewModel)DataContext).FocusRequested += Focus;
            ((PoolViewModel)DataContext).AmountFocusRequested += AmountFocus;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            ((PoolViewModel)DataContext).FocusRequested -= Focus;
            ((PoolViewModel)DataContext).AmountFocusRequested -= AmountFocus;
        }

        private void Focus(object sender, EventArgs e) => tbName.Focus();

        private void AmountFocus(object sender, EventArgs e) => tbAmount.Focus();
    }
}
