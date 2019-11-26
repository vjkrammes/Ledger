using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Collections.ObjectModel;
using LedgerClient.Infrastructure;
using LedgerClient.ECL.DTO;
using LedgerClient.ECL.Interfaces;
using LedgerLib.Infrastructure;
using LedgerClient.Views;

namespace LedgerClient.ViewModels
{
    public class HistoryViewModel : ViewModelBase
    {
        #region Properties

        private Company _company;
        public Company Company
        {
            get => _company;
            set => SetProperty(ref _company, value);
        }

        private Account _account;
        public Account Account
        {
            get => _account;
            set
            {
                SetProperty(ref _account, value);
                if (Account is null)
                {
                    Numbers = null;
                }
                else
                {
                    try
                    {
                        Numbers = new ObservableCollection<AccountNumber>(_anECL.GetForAccount(Account.Id));
                    }
                    catch (Exception ex)
                    {
                        PopupManager.Popup("Failed to retrieve Account Numbers", Constants.DBE, ex.Innermost(), PopupButtons.Ok,
                            PopupImage.Error);
                        Cancel();
                    }
                }
            }
        }

        private ObservableCollection<AccountNumber> _numbers;
        public ObservableCollection<AccountNumber> Numbers
        {
            get => _numbers;
            set => SetProperty(ref _numbers, value);
        }

        private AccountNumber _selectedNumber;
        public AccountNumber SelectedNumber
        {
            get => _selectedNumber;
            set => SetProperty(ref _selectedNumber, value);
        }

        private readonly IAccountNumberECL _anECL;

        #endregion

        #region Commands

        private RelayCommand _viewNumberCommand;
        public ICommand ViewNumberCommand
        {
            get
            {
                if (_viewNumberCommand is null)
                {
                    _viewNumberCommand = new RelayCommand(parm => ViewNumberClick(), parm => NumberSelected());
                }
                return _viewNumberCommand;
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

        private bool NumberSelected() => SelectedNumber != null;

        private void ViewNumberClick()
        {
            if (SelectedNumber is null)
            {
                return;
            }
            var vm = Tools.Locator.AccountNumberViewModel;
            vm.AccountNumber = SelectedNumber;
            DialogSupport.ShowDialog<AccountNumberWindow>(vm);
            SelectedNumber = null;
        }

        private void WindowLoaded()
        {
            if (Company is null || Account is null)
            {
                PopupManager.Popup("Company and/or Account are missing", "Application Error", PopupButtons.Ok, PopupImage.Error);
                Cancel();
                return;
            }
        }

        #endregion

        public HistoryViewModel(IAccountNumberECL anecl)
        {
            _anECL = anecl;
        }
    }
}
