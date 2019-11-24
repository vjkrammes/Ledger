using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using LedgerClient.ECL.DTO;
using LedgerClient.ECL.Interfaces;
using LedgerClient.Infrastructure;
using LedgerClient.Interfaces;
using LedgerClient.Views;
using LedgerLib.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LedgerClient.ViewModels
{
    public class AllotmentViewModel : ViewModelBase
    {
        #region Properties

        private Pool _pool;
        public Pool Pool
        {
            get => _pool;
            set => SetProperty(ref _pool, value);
        }

        private ObservableCollection<Company> _companies;
        public ObservableCollection<Company> Companies
        {
            get => _companies;
            set => SetProperty(ref _companies, value);
        }

        private Company _selectedCompany;
        public Company SelectedCompany
        {
            get => _selectedCompany;
            set => SetProperty(ref _selectedCompany, value);
        }

        private DateTime? _date;
        public DateTime? Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private decimal _amount;
        public decimal Amount
        {
            get => _amount;
            set => SetProperty(ref _amount, value);
        }

        private ObservableCollection<Allotment> _allotments;
        public ObservableCollection<Allotment> Allotments
        {
            get => _allotments;
            set => SetProperty(ref _allotments, value);
        }

        private Allotment _selectedAllotment;
        public Allotment SelectedAllotment
        {
            get => _selectedAllotment;
            set
            {
                SetProperty(ref _selectedAllotment, value);
                if (SelectedAllotment != null)
                {
                    SelectedCompany = (from c in Companies where c.Id == SelectedAllotment.CompanyId select c).SingleOrDefault();
                    Date = SelectedAllotment.Date;
                    Description = SelectedAllotment.Description ?? string.Empty;
                    Amount = SelectedAllotment.Amount;
                    _editing = true;
                }
                else
                {
                    Clear();
                }
            }
        }

        private bool _editing;
        private readonly ICompanyECL _companyECL;
        private readonly IAllotmentECL _allotmentECL;
        private readonly IPoolECL _poolECL;

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

        private RelayCommand _saveChangesCommand;
        public ICommand SaveChangesCommand
        {
            get
            {
                if (_saveChangesCommand is null)
                {
                    _saveChangesCommand = new RelayCommand(parm => SaveChangesClick(), parm => SaveChangesCanClick());
                }
                return _saveChangesCommand;
            }
        }

        private RelayCommand _cancelChangesCommand;
        public ICommand CancelChangesCommand
        {
            get
            {
                if (_cancelChangesCommand is null)
                {
                    _cancelChangesCommand = new RelayCommand(parm => CancelChangesClick(), parm => IsEditing());
                }
                return _cancelChangesCommand;
            }
        }

        private RelayCommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand is null)
                {
                    _deleteCommand = new RelayCommand(parm => DeleteClick(), parm => AllotmentSelected());
                }
                return _deleteCommand;
            }
        }

        private RelayCommand _deleteAllCommand;
        public ICommand DeleteAllCommand
        {
            get
            {
                if (_deleteAllCommand is null)
                {
                    _deleteAllCommand = new RelayCommand(parm => DeleteAllClick(), parm => AllotmentsExist());
                }
                return _deleteAllCommand;
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

        private bool AddCanClick() => SelectedCompany != null && Amount > 0M;

        private void AddClick()
        {
            if (SelectedCompany is null || Amount <= 0M)
            {
                return;
            }
            if (Pool.Balance - Amount < 0M)
            {
                string msg = "Adding this allotment will reduce the Balance of this Pool to less than zero. Continue?";
                if (PopupManager.Popup("Continue with negative balance?","Negative Balance",msg,PopupButtons.YesNo, PopupImage.Question)
                    != PopupResult.Yes)
                {
                    return;
                }
            }
            Allotment a = new Allotment
            {
                PoolId = Pool.Id,
                CompanyId = SelectedCompany.Id,
                Company = SelectedCompany,
                Date = Date ?? (default),
                Amount = Amount,
                Description = Description ?? string.Empty
            };
            try
            {
                _allotmentECL.Insert(a);
            }
            catch (DbUpdateConcurrencyException)
            {
                Tools.ConcurrencyError("Allotment", "Insert");
                return;
            }
            catch (Exception ex)
            {
                PopupManager.Popup("Failed to add new Allotment", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
                return;
            }
            int ix = 0;
            while (ix < Allotments.Count && Allotments[ix] > a)
            {
                ix++;
            }
            Allotments.Insert(ix, a);
            SelectedAllotment = a;
            SelectedAllotment = null;
            Pool.Balance -= a.Amount;
            try
            {
                _poolECL.Update(Pool);
            }
            catch (DbUpdateConcurrencyException)
            {
                Tools.ConcurrencyError("Pool", "Update");
            }
            catch (Exception ex)
            {
                PopupManager.Popup("Failed to update Pool", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
            }
        }

        private bool SaveChangesCanClick() => _editing && SelectedCompany != null && Amount > 0M;

        private void SaveChangesClick()
        {

        }

        private bool IsEditing() => _editing;

        private void CancelChangesClick()
        {
            Clear();
            SelectedAllotment = null;
        }

        private bool AllotmentSelected() => SelectedAllotment != null;

        private void DeleteClick()
        {

        }

        private bool AllotmentsExist() => Allotments != null && Allotments.Any();

        private void DeleteAllClick()
        {

        }

        private void WindowLoaded()
        {
            if (Pool is null)
            {
                PopupManager.Popup($"No Pool was selected", "Application Error", PopupButtons.Ok, PopupImage.Stop);
                Cancel();
                return;
            }
            LoadCompanies();
            LoadAllotments(false);
        }

        #endregion

        #region Utility Methods

        private void LoadCompanies()
        {
            try
            {
                Companies = new ObservableCollection<Company>(_companyECL.Get());
            }
            catch (Exception ex)
            {
                PopupManager.Popup("Failed to load Companies", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
                Cancel();
                return;
            }
        }

        private void LoadAllotments(bool reload = false)
        {
            try
            {
                Allotments = new ObservableCollection<Allotment>(_allotmentECL.GetForPool(Pool.Id));
            }
            catch (Exception ex)
            {
                string op = reload ? Constants.Reload : Constants.Load;
                PopupManager.Popup($"Failed to {op} Allotments", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
                Cancel();
                return;
            }
        }

        private void Clear()
        {
            SelectedCompany = null;
            Date = DateTime.Now;
            Description = string.Empty;
            Amount = 0M;
            _editing = false;
        }

        #endregion

        public AllotmentViewModel(IAllotmentECL aecl, ICompanyECL cecl, IPoolECL pecl)
        {
            _allotmentECL = aecl;
            _companyECL = cecl;
            _poolECL = pecl;
            Date = DateTime.Now;
        }
    }
}
