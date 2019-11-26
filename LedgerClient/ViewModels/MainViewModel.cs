using System;
using System.Windows;

using LedgerClient.Infrastructure;

using LedgerLib.Infrastructure;

namespace LedgerClient.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            Pallette.SetSystemColors();
            Application.Current.Resources[Constants.IconHeight] = Tools.Locator.Settings.IconHeight;
            WindowTitle = string.Empty; // null title will throw an exception
            PopupManager.SetMaxWidth(400);
            PopupManager.SetWindowIcon(new Uri("/resources/book-32.png", UriKind.Relative));
        }
    }
}
