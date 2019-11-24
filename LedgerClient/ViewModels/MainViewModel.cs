using System;
using System.Collections.Generic;
using System.Text;
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
        }
    }
}
