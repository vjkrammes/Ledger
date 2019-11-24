using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using LedgerClient.Infrastructure;

using LedgerLib;
using LedgerLib.Infrastructure;

using Microsoft.EntityFrameworkCore;

namespace LedgerClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            using var context = new LedgerContext();
            try
            {
                context.Database.Migrate();
                context.Seed();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Innermost(), "Database Migration Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(Constants.MigrationFailed);
            }

            // Make Textboxes and password boxes auto-select text when they get focus

            EventManager.RegisterClassHandler(typeof(TextBox), UIElement.PreviewMouseLeftButtonDownEvent,
                new MouseButtonEventHandler(MouseHandler<TextBox>));
            EventManager.RegisterClassHandler(typeof(TextBox), UIElement.GotKeyboardFocusEvent,
                new RoutedEventHandler(TextBoxSelectText));
            EventManager.RegisterClassHandler(typeof(TextBox), Control.MouseDoubleClickEvent,
                new RoutedEventHandler(TextBoxSelectText));
            EventManager.RegisterClassHandler(typeof(PasswordBox), UIElement.PreviewMouseLeftButtonDownEvent,
                new MouseButtonEventHandler(MouseHandler<PasswordBox>));
            EventManager.RegisterClassHandler(typeof(PasswordBox), UIElement.GotKeyboardFocusEvent,
                new RoutedEventHandler(PasswordBoxSelectText));
            EventManager.RegisterClassHandler(typeof(PasswordBox), Control.MouseDoubleClickEvent,
                new RoutedEventHandler(PasswordBoxSelectText));
        }

        private void MouseHandler<T>(object sender, MouseButtonEventArgs e) where T : UIElement
        {
            DependencyObject parent = e.OriginalSource as UIElement;
            while (parent != null && !(parent is T))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            if (parent != null)
            {
                if (parent is T control)
                {
                    if (!control.IsKeyboardFocusWithin)
                    {
                        control.Focus();
                        e.Handled = true;
                    }
                }
            }
        }

        private void TextBoxSelectText(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is TextBox tb)
            {
                tb.SelectAll();
            }
        }

        private void PasswordBoxSelectText(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is PasswordBox pb)
            {
                pb.SelectAll();
            }
        }
    }
}
