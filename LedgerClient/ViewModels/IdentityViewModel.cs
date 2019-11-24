using System;
using System.Collections.Generic;
using System.Text;
using LedgerClient.Infrastructure;
using LedgerClient.ECL.DTO;
using System.Windows;
using LedgerClient.ECL.Interfaces;
using System.Windows.Input;

namespace LedgerClient.ViewModels
{
    public class IdentityViewModel : ViewModelBase
    {
        #region Properties

        private string _savedUserId;
        private bool _editing;
        private readonly IIdentityECL _ecl;

        private Company _company;
        public Company Company
        {
            get => _company;
            set
            {
                SetProperty(ref _company, value);
                if (Company is null)
                {
                    Identity.CompanyId = 0;
                    Identity.Company = null;
                    URL = Identity?.URL ?? string.Empty;
                }
                else
                {
                    Identity.CompanyId = Company.Id;
                    Identity.Company = Company.Clone();
                    URL = Company.URL ?? string.Empty;
                }
            }
        }

        private string _url;
        public string URL
        {
            get => _url;
            set => SetProperty(ref _url, value);
        }

        private string _userId;
        public string UserId
        {
            get => _userId;
            set => SetProperty(ref _userId, value);
        }

        private string _password1;
        public string Password1
        {
            get => _password1;
            set
            {
                SetProperty(ref _password1, value);
                PasswordStrength = PasswordChecker.GetPasswordStrength(Password1);
            }
        }

        private string _password2;
        public string Password2
        {
            get => _password2;
            set => SetProperty(ref _password2, value);
        }

        private bool _showPassword;
        public bool ShowPassword
        {
            get => _showPassword;
            set
            {
                SetProperty(ref _showPassword, value);
                if (ShowPassword)
                {
                    Password2Visibility = Visibility.Hidden;
                }
                else
                {
                    Password2Visibility = Visibility.Visible;
                }
            }
        }

        private PasswordStrength _passwordStrength;
        public PasswordStrength PasswordStrength
        {
            get => _passwordStrength;
            set => SetProperty(ref _passwordStrength, value);
        }

        private Visibility _password2Visibility;
        public Visibility Password2Visibility
        {
            get => _password2Visibility;
            set => SetProperty(ref _password2Visibility, value);
        }

        public Identity _identity;
        public Identity Identity
        {
            get => _identity;
            set
            {
                SetProperty(ref _identity, value);
                Company = Identity.Company;
                URL = string.IsNullOrEmpty(Identity.URL) ? Company?.URL ?? string.Empty : Identity.URL;
                UserId = Tools.Locator.StringCypher.Decrypt(Identity.UserId, Tools.Locator.PasswordManager.Get(), Identity.UserSalt);
                _savedUserId = UserId;
                Password1 = Tools.Locator.StringCypher.Decrypt(Identity.Password, Tools.Locator.PasswordManager.Get(), Identity.PasswordSalt);
                Password2 = Password1;
                _editing = Identity.Id > 0;
            }
        }

        #endregion

        #region Commands

        private RelayCommand _togglePasswordCommand;
        public ICommand TogglePasswordCommand
        {
            get
            {
                if (_togglePasswordCommand is null)
                {
                    _togglePasswordCommand = new RelayCommand(parm => TogglePasswordClick(), parm => AlwaysCanExecute());
                }
                return _togglePasswordCommand;
            }
        }

        #endregion

        #region Command Methods

        public override bool OkCanExecute() => !string.IsNullOrEmpty(UserId) && (ShowPassword || Password1 == Password2);

        public override void OK()
        {
            if (_editing && UserId != _savedUserId && _ecl.Read(Company.Id, UserId) != null)
            {
                Duplicate();
                return;
            }
            else if (!_editing &&  _ecl.Read(Company.Id, UserId) != null)
            {
                Duplicate();
                return;
            }
            var cypher = Tools.Locator.StringCypher;
            Identity.URL = URL;
            Identity.UserId = cypher.Encrypt(UserId, Tools.Locator.PasswordManager.Get(), Identity.UserSalt);
            Identity.Password = cypher.Encrypt(Password1, Tools.Locator.PasswordManager.Get(), Identity.PasswordSalt);
            base.OK();
        }

        private void TogglePasswordClick() => ShowPassword = !ShowPassword;

        #endregion

        #region Utility Methods

        private void Duplicate()
        {
            PopupManager.Popup($"An Identity with the user name '{UserId}' already exists for this company", "Duplicate Identity",
                PopupButtons.Ok, PopupImage.Stop);
        }

        #endregion

        public IdentityViewModel(IIdentityECL ecl)
        {
            _ecl = ecl;
            Identity = new Identity();
            ShowPassword = false;
        }
    }
}
