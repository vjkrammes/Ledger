using LedgerClient.Infrastructure;

namespace LedgerClient.HistoryViewModels
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

        #region Command Methods

        public override bool OkCanExecute() => !string.IsNullOrEmpty(Password);
        
        #endregion
    }
}
