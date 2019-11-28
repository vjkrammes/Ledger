using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

using LedgerClient.ECL.DTO;
using LedgerClient.ECL.Interfaces;
using LedgerClient.Infrastructure;
using LedgerClient.Views;

using LedgerLib.Infrastructure;

namespace LedgerClient.ViewModels
{
    public class AccountViewModel : ViewModelBase
    {
        #region Properties

        private Company _company;
        public Company Company
        {
            get => _company;
            set
            {
                SetProperty(ref _company, value);
                if (Company is null)
                {
                    IsPayable = true;
                }
                else
                {
                    IsPayable = Company.IsPayee;
                }
            }
        }

        private ObservableCollection<AccountType> _types;
        public ObservableCollection<AccountType> Types
        {
            get => _types;
            set => SetProperty(ref _types, value);
        }

        private AccountType _selectedType;
        public AccountType SelectedType
        {
            get => _selectedType;
            set => SetProperty(ref _selectedType, value);
        }

        private ObservableCollection<DueDateType> _dueDateTypes;
        public ObservableCollection<DueDateType> DueDateTypes
        {
            get => _dueDateTypes;
            set => SetProperty(ref _dueDateTypes, value);
        }

        private string _number;
        public string Number
        {
            get => _number;
            set => SetProperty(ref _number, value);
        }

        private DueDateType _selectedDueDateType;
        public DueDateType SelectedDueDateType
        {
            get => _selectedDueDateType;
            set => SetProperty(ref _selectedDueDateType, value);
        }

        private int _month;
        public int Month
        {
            get => _month;
            set => SetProperty(ref _month, value);
        }

        private int _day;
        public int Day
        {
            get => _day;
            set => SetProperty(ref _day, value);
        }

        private bool _isPayable;
        public bool IsPayable
        {
            get => _isPayable;
            set
            {
                SetProperty(ref _isPayable, value);
                if (IsPayable)
                {
                    SelectedDueDateType = Account?.DueDateType ?? DueDateType.Unspecified;
                    Month = Account?.Month ?? 0;
                    Day = Account?.Day ?? 0;
                }
                else
                {
                    SelectedDueDateType = DueDateType.NA;
                    Month = 0;
                    Day = 0;
                }
            }
        }

        private string _comments;
        public string Comments
        {
            get => _comments;
            set => SetProperty(ref _comments, value);
        }

        private Account _account;
        public Account Account
        {
            get => _account;
            set
            {
                SetProperty(ref _account, value);
                if (Account != null && Account.Id > 0)
                {
                    SelectedType = Account.AccountType;
                    SelectedDueDateType = Account.DueDateType;
                    Number = _locator.StringCypher.Decrypt(Account.AccountNumber.Number, _locator.PasswordManager.Get(),
                        Account.AccountNumber.Salt);
                    Month = Account.Month;
                    Day = Account.Day;
                    IsPayable = Account.IsPayable;
                    Comments = Account.Comments ?? string.Empty;
                    IsEditing = true;
                }
                else
                {
                    SelectedType = null;
                    SelectedDueDateType = DueDateType.Unspecified;
                    Number = string.Empty;
                    Month = 0;
                    Day = 0;
                    IsPayable = Company?.IsPayee ?? true;
                    Comments = string.Empty;
                    IsEditing = false;
                }
            }
        }

        private bool _isEditing;
        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                SetProperty(ref _isEditing, value);
                if (IsEditing)
                {
                    NANButtonVisibility = Visibility.Visible;
                }
                else
                {
                    NANButtonVisibility = Visibility.Collapsed;
                }
            }
        }

        private Visibility _nanButtonVisibility;
        public Visibility NANButtonVisibility
        {
            get => _nanButtonVisibility;
            set => SetProperty(ref _nanButtonVisibility, value);
        }

        private readonly IAccountECL _aECL;
        private readonly IAccountTypeECL _atECL;
        private readonly Locator _locator;

        #endregion

        #region Commands

        private RelayCommand _changeNumberCommand;
        public ICommand ChangeNumberCommand
        {
            get
            {
                if (_changeNumberCommand is null)
                {
                    _changeNumberCommand = new RelayCommand(parm => ChangeNumberClick(), parm => AlwaysCanExecute());
                }
                return _changeNumberCommand;
            }
        }

        private RelayCommand _manageTypesCommand;
        public ICommand ManageTypesCommand
        {
            get
            {
                if (_manageTypesCommand is null)
                {
                    _manageTypesCommand = new RelayCommand(parm => ManageTypesClick(), parm => AlwaysCanExecute());
                }
                return _manageTypesCommand;
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

        public override bool OkCanExecute() =>
            SelectedType != null && SelectedDueDateType != DueDateType.Unspecified && !string.IsNullOrEmpty(Number);

        public override void OK()
        {
            switch (SelectedDueDateType)
            {
                case DueDateType.NA:
                case DueDateType.ServiceRelated:
                    break;
                case DueDateType.Unspecified:
                    return;
                case DueDateType.Monthly:
                    if (Day == 0)
                    {
                        PopupManager.Popup("Day is required for Monthly Accounts", "Missing Day", PopupButtons.Ok, PopupImage.Stop);
                        return;
                    }
                    break;
                case DueDateType.Quarterly:
                    if (Month == 0 || Day == 0)
                    {
                        PopupManager.Popup("Month and Day are required for Quarterly Accounts", "Missing Date", PopupButtons.Ok,
                            PopupImage.Stop);
                        return;
                    }
                    break;
                case DueDateType.Annually:
                    if (Month == 0 || Day == 0)
                    {
                        PopupManager.Popup("Month and Day are required for Annual Accounts", "Missing Date", PopupButtons.Ok,
                            PopupImage.Stop);
                        return;
                    }
                    break;
            }
            base.OK();
        }

        private void ChangeNumberClick()
        {
            var vm = Tools.Locator.QAViewModel;
            vm.Question = "New Account Number:";
            vm.Answer = string.Empty;
            vm.AnswerRequired = true;
            vm.BorderBrush = Application.Current.Resources[Constants.Border] as SolidColorBrush;
            if (DialogSupport.ShowDialog<QAWindow>(vm) != true)
            {
                return;
            }
            Debug.WriteLine($"Pre Create, Account.AccountNumber.Id = {Account.AccountNumber.Id}");
            try
            {
                var newacct = _aECL.Create(Account, vm.Answer);
                Account = null;     // workaround for number not updating
                Account = newacct;
            }
            catch (Exception ex)
            {
                PopupManager.Popup("Failed to create new Account Number", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
                return;
            }
        }

        private void ManageTypesClick()
        {
            var vm = Tools.Locator.AccountTypeViewModel;
            DialogSupport.ShowDialog<AccountTypeWindow>(vm);
            LoadAccountTypes();
        }

        private void WindowLoaded()
        {
            if (Company is null)
            {
                PopupManager.Popup("Company was not selected", "Application Error", PopupButtons.Ok, PopupImage.Error);
                Cancel();
                return;
            }
        }

        #endregion

        #region Utility Methods

        private void LoadAccountTypes(bool reload = false)
        {
            try
            {
                Types = new ObservableCollection<AccountType>(_atECL.Get());
            }
            catch (Exception ex)
            {
                var op = reload ? Constants.Reload : Constants.Load;
                PopupManager.Popup($"Failed to {op} Account Types", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
                Cancel();
                return;
            }
        }

        #endregion

        public AccountViewModel(IAccountECL aecl, IAccountTypeECL atecl)
        {
            _atECL = atecl;
            _aECL = aecl;
            DueDateTypes = new ObservableCollection<DueDateType>(Tools.GetValues<DueDateType>());
            LoadAccountTypes(false);
            Account = new Account();
            _locator = Tools.Locator;
        }
    }
}
