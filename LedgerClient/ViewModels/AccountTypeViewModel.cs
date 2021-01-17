using LedgerClient.ECL.DTO;
using LedgerClient.ECL.Interfaces;
using LedgerClient.Infrastructure;
using LedgerClient.Views;

using LedgerLib.Infrastructure;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace LedgerClient.ViewModels
{
    public class AccountTypeViewModel : ViewModelBase
    {
        #region Properties

        private readonly IAccountTypeECL _ecl;

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
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

        #endregion

        #region Commands

        private RelayCommand _addCommand;
        public ICommand AddCommand
        {
            get
            {
                if (_addCommand is null)
                {
                    _addCommand = new RelayCommand(parm => AddClick(), parm => AddCanClick());
                }
                return _addCommand;
            }
        }

        private RelayCommand _renameCommand;
        public ICommand RenameCommand
        {
            get
            {
                if (_renameCommand is null)
                {
                    _renameCommand = new RelayCommand(parm => RenameClick(), parm => TypeSelected());
                }
                return _renameCommand;
            }
        }

        private RelayCommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand is null)
                {
                    _deleteCommand = new RelayCommand(parm => DeleteClick(), parm => TypeSelected());
                }
                return _deleteCommand;
            }
        }

        #endregion

        #region Command Methods

        private bool AddCanClick() => !string.IsNullOrEmpty(Description);

        public void AddClick()
        {
            if (string.IsNullOrEmpty(Description))
            {
                return;
            }
            if (_ecl.Read(Description) != null)
            {
                PopupManager.Popup($"An Account Type with the description '{Description}' already exists", "Duplicate Account Type",
                    PopupButtons.Ok, PopupImage.Stop);
                FocusRequested?.Invoke(this, EventArgs.Empty);
                return;
            }
            var a = new AccountType
            {
                Description = Description.Caseify()
            };
            try
            {
                _ecl.Insert(a);
            }
            catch (DbUpdateConcurrencyException)
            {
                Tools.ConcurrencyError("Account Type", "Insert");
                return;
            }
            catch (Exception ex)
            {
                PopupManager.Popup("Failed to add new Account Type", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
                FocusRequested?.Invoke(this, EventArgs.Empty);
                return;
            }
            var ix = 0;
            while (ix < Types.Count && Types[ix] < a)
            {
                ix++;
            }

            Types.Insert(ix, a);
            SelectedType = a;
            SelectedType = null;
            Description = string.Empty;
            FocusRequested?.Invoke(this, EventArgs.Empty);
        }

        private bool TypeSelected() => SelectedType != null;

        private void RenameClick()
        {
            if (SelectedType is null)
            {
                return;
            }
            var save = SelectedType.Description;
            var vm = Tools.Locator.QAViewModel;
            vm.Question = "Description:";
            vm.Answer = SelectedType.Description;
            vm.AnswerRequired = true;
            vm.BorderBrush = Application.Current.Resources[Constants.Border] as SolidColorBrush;
            if (DialogSupport.ShowDialog<QAWindow>(vm) != true)
            {
                SelectedType = null;
                FocusRequested?.Invoke(this, EventArgs.Empty);
                return;
            }
            if (!vm.Answer.Equals(save, StringComparison.OrdinalIgnoreCase) && _ecl.Read(vm.Answer) != null)
            {
                PopupManager.Popup($"An Account Type with the description '{vm.Answer}' already exists", "Duplicate Account Type",
                    PopupButtons.Ok, PopupImage.Stop);
                SelectedType = null;
                FocusRequested?.Invoke(this, EventArgs.Empty);
                return;
            }
            SelectedType.Description = vm.Answer.Caseify();
            try
            {
                _ecl.Update(SelectedType);
            }
            catch (DbUpdateConcurrencyException)
            {
                Tools.ConcurrencyError("Account Type", "Update");
                LoadAccountTypes(true);
                return;
            }
            catch (Exception ex)
            {
                SelectedType.Description = save;
                PopupManager.Popup("Failed to update Account Type", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
                SelectedType = null;
                FocusRequested?.Invoke(this, EventArgs.Empty);
                return;
            }
            var a = SelectedType;
            Types.Remove(SelectedType);
            SelectedType = null;
            var ix = 0;
            while (ix < Types.Count && Types[ix] < a)
            {
                ix++;
            }

            Types.Insert(ix, a);
            SelectedType = a;
            SelectedType = null;
            FocusRequested?.Invoke(this, EventArgs.Empty);
        }

        private void DeleteClick()
        {
            if (SelectedType is null)
            {
                return;
            }
            if (Tools.Locator.AccountECL.AccountTypeHasAccounts(SelectedType.Id))
            {
                PopupManager.Popup("Can't Delete the Account Type", "Can't Delete", 
                    "The selected Account Type has associated Accounts and cannot be deleted", PopupButtons.Ok, PopupImage.Stop);
                SelectedType = null;
                FocusRequested?.Invoke(this, EventArgs.Empty);
                return;
            }
            try
            {
                _ecl.Delete(SelectedType);
            }
            catch (DbUpdateConcurrencyException)
            {
                Tools.ConcurrencyError("Account Type", "Delete");
                LoadAccountTypes(true);
                return;
            }
            catch (Exception ex)
            {
                PopupManager.Popup("Failed to delete Account Type", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
                SelectedType = null;
                FocusRequested?.Invoke(this, EventArgs.Empty);
                return;
            }
            Types.Remove(SelectedType);
            SelectedType = null;
            FocusRequested?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Utility methods

        private void LoadAccountTypes(bool reload = false)
        {
            try
            {
                Types = new ObservableCollection<AccountType>(_ecl.Get());
            }
            catch (Exception ex)
            {
                var op = reload ? "reload" : "load";
                PopupManager.Popup($"Failed to {op} Account Types", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
                Cancel();
                return;
            }
        }

        #endregion

        #region Events

        public event EventHandler FocusRequested;

        #endregion

        public AccountTypeViewModel(IAccountTypeECL ecl)
        {
            _ecl = ecl;
            LoadAccountTypes(false);
        }
    }
}
