using LedgerClient.Infrastructure;

using LedgerLib.Infrastructure;

using System.Windows.Input;

namespace LedgerClient.History.ViewModels
{
    public class Ledger5PasswordViewModel : ViewModelBase
    {
        #region Properties

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        #endregion

        #region Commands

        private RelayCommand _sameCommand;
        public ICommand SameCommand
        {
            get
            {
                if (_sameCommand is null)
                {
                    _sameCommand = new RelayCommand(parm => SameClick(), parm => AlwaysCanExecute());
                }
                return _sameCommand;
            }
        }

        #endregion

        #region Command Methods

        public override bool OkCanExecute() => !string.IsNullOrEmpty(Password);
        
        private void SameClick()
        {
            Password = Tools.Locator.PasswordManager.Get(Constants.LedgerPassword);
            OK();
        }
        
        #endregion
    }
}
