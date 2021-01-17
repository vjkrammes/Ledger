using LedgerClient.ECL.DTO;
using LedgerClient.Infrastructure;
using LedgerClient.Interfaces;

using LedgerLib.Infrastructure;

using System.Windows;
using System.Windows.Input;

namespace LedgerClient.ViewModels
{
    public class AccountNumberViewModel : ViewModelBase
    {
        #region Properties

        private readonly IStringCypherService _crypto;

        private AccountNumber _accountNumber;
        public AccountNumber AccountNumber
        {
            get => _accountNumber;
            set
            {
                SetProperty(ref _accountNumber, value);
                if (AccountNumber is null)
                {
                    Number = string.Empty;
                }
                else
                {
                    Number = _crypto.Decrypt(AccountNumber.Number, Tools.Locator.PasswordManager.Get(Constants.LedgerPassword), 
                        AccountNumber.Salt);
                }
            }
        }

        private string _number;
        public string Number
        {
            get => _number;
            set => SetProperty(ref _number, value);
        }

        #endregion

        #region Commands

        private RelayCommand _copyCommand;
        public ICommand CopyCommand
        {
            get
            {
                if (_copyCommand is null)
                {
                    _copyCommand = new RelayCommand(parm => CopyClick(), parm => AlwaysCanExecute());
                }
                return _copyCommand;
            }
        }

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

        private void CopyClick() => Clipboard.SetText(Number);

        private void WindowLoaded()
        {
            if (AccountNumber is null)
            {
                PopupManager.Popup("No Account Number was selected", "No Account Number", PopupButtons.Ok, PopupImage.Stop);
                Cancel();
                return;
            }
        }

        #endregion

        public AccountNumberViewModel(IStringCypherService crypto) => _crypto = crypto;
    }
}
