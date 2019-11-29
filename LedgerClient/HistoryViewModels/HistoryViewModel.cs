using System;
using System.Collections.Generic;
using System.Text;
using LedgerClient.Infrastructure;
using System.Windows;
using System.Windows.Input;

namespace LedgerClient.HistoryViewModels
{
    public class HistoryViewModel : ViewModelBase
    {
        #region Properties



        #endregion

        #region Commands

        private RelayCommand _windowLoadedCommand;
        public ICommand WindowLoadedCommand
        {
            get
            {
                if (_windowLoadedCommand is null)
                {
                    _windowLoadedCommand = new RelayCommand(parm => WindowLoaded(), parm => AlwaysCanExecute());
                }
                return _windowLoadedCommand;
            }
        }

        #endregion

        #region Command Methods

        private void WindowLoaded()
        {

        }

        #endregion
    }
}
