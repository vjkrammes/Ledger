using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using LedgerClient.ECL.DTO;
using LedgerClient.ECL.Interfaces;
using LedgerClient.Infrastructure;
using LedgerClient.Views;

using LedgerLib.Infrastructure;

using Microsoft.EntityFrameworkCore;

namespace LedgerClient.ViewModels
{
    public class PoolViewModel : ViewModelBase
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
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

        private ObservableCollection<Pool> _pools;
        public ObservableCollection<Pool> Pools
        {
            get => _pools;
            set => SetProperty(ref _pools, value);
        }

        private Pool _selectedPool;
        public Pool SelectedPool
        {
            get => _selectedPool;
            set
            {
                SetProperty(ref _selectedPool, value);
                if (SelectedPool != null)
                {
                    _editing = true;
                    Name = SelectedPool.Name ?? string.Empty;
                    Date = SelectedPool.Date == default ? (DateTime?)null : SelectedPool.Date;
                    Description = SelectedPool.Description ?? string.Empty;
                    Amount = SelectedPool.Amount;
                    FocusRequested?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private bool _editing;
        private readonly IPoolECL _poolECL;
        private readonly IAllotmentECL _allotmentECL;

        #region Commands

        public RelayCommand _addCommand;
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

        private RelayCommand _recalculateCommand;
        public ICommand RecalculateCommand
        {
            get
            {
                if (_recalculateCommand is null)
                {
                    _recalculateCommand = new RelayCommand(parm => RecalculateClick(), parm => PoolsExist());
                }
                return _recalculateCommand;
            }
        }

        private RelayCommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand is null)
                {
                    _deleteCommand = new RelayCommand(parm => DeleteClick(), parm => DeleteCanClick());
                }
                return _deleteCommand;
            }
        }

        private RelayCommand _manageAllotmentsCommand;
        public ICommand ManageAllotmentsCommand
        {
            get
            {
                if (_manageAllotmentsCommand is null)
                {
                    _manageAllotmentsCommand = new RelayCommand(parm => ManageAllotmentsClick(), parm => PoolSelected());
                }
                return _manageAllotmentsCommand;
            }
        }

        #endregion

        #region Command Methods

        private bool AddCanClick() => !_editing && !string.IsNullOrEmpty(Name) && Amount > 0M;

        private void AddClick()
        {
            if (string.IsNullOrEmpty(Name) || Amount == 0M)
            {
                return;
            }
            if (_poolECL.Read(Name) != null)
            {
                PopupManager.Popup($"A Pool with the name '{Name}' already exists", "Duplicate Pool", PopupButtons.Ok,
                    PopupImage.Stop);
                FocusRequested?.Invoke(this, EventArgs.Empty);
                return;
            }
            Pool p = new Pool
            {
                Name = Name.Caseify(),
                Date = Date ?? (default),
                Description = Description ?? string.Empty,
                Amount = Amount,
                Balance = Amount
            };
            try
            {
                _poolECL.Insert(p);
            }
            catch (DbUpdateConcurrencyException)
            {
                Tools.ConcurrencyError("Pool", "Insert");
                FocusRequested?.Invoke(this, EventArgs.Empty);
                return;
            }
            catch (Exception ex)
            {
                PopupManager.Popup("Failed to insert new Pool", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
                FocusRequested?.Invoke(this, EventArgs.Empty);
                return;
            }
            int ix = 0;
            while (ix < Pools.Count && Pools[ix] < p)
            {
                ix++;
            }
            Pools.Insert(ix, p);
            SelectedPool = p;
            SelectedPool = null;
            Clear();
        }

        private bool SaveChangesCanClick() => _editing && !string.IsNullOrEmpty(Name) && Amount > 0M;

        private void SaveChangesClick()
        {
            decimal diff = Amount - SelectedPool.Amount;
            if (SelectedPool.Balance + diff < 0M && SelectedPool.Balance >= 0M)
            {
                string msg = $"Adjusting the Amount to {Amount:c2} will create a negative Balance. Continue?";
                if (PopupManager.Popup("Balance will be negative", "Negative Balance", msg, PopupButtons.YesNo, PopupImage.Question) !=
                    PopupResult.Yes)
                {
                    AmountFocusRequested.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
            if (!Name.Equals(SelectedPool.Name, StringComparison.OrdinalIgnoreCase))
            {
                if (_poolECL.Read(Name) != null)
                {
                    PopupManager.Popup($"A Pool with the name '{Name}' already exists", "Duplicate Pool", PopupButtons.Ok,
                        PopupImage.Stop);
                    FocusRequested?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
            Pool p = SelectedPool.Clone();
            p.Name = Name.Caseify();
            p.Date = Date ?? (default);
            p.Amount = Amount;
            p.Balance += diff;
            p.Description = Description;
            try
            {
                _poolECL.Update(p);
            }
            catch (DbUpdateConcurrencyException)
            {
                Tools.ConcurrencyError("Pool", "Update");
                LoadPools(true);
                Clear();
                return;
            }
            Pools.Remove(SelectedPool);
            SelectedPool = null;
            int ix = 0;
            while (ix < Pools.Count && Pools[ix] < p)
            {
                ix++;
            }
            Pools.Insert(ix, p);
            SelectedPool = p;
            Clear();
        }

        private bool IsEditing() => _editing;

        private void CancelChangesClick()
        {
            Clear();
        }

        private bool PoolsExist() => Pools.Any();

        private void RecalculateClick()
        {
            try
            {
                Tools.Locator.PoolRecalculator.Recalculate();
            }
            catch (Exception ex)
            {
                PopupManager.Popup("Failed to recalculate Pools", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
            }
            LoadPools(true);
        }

        private bool DeleteCanClick() => SelectedPool != null && !_allotmentECL.PoolHasAllotments(SelectedPool.Id);

        private void DeleteClick()
        {
            if (SelectedPool is null || _allotmentECL.PoolHasAllotments(SelectedPool.Id))
            {
                return;
            }
            try
            {
                _poolECL.Delete(SelectedPool);
            }
            catch (DbUpdateConcurrencyException)
            {
                Tools.ConcurrencyError("Pool", "Delete");
                LoadPools(true);
                Clear();
                return;
            }
            catch (Exception ex)
            {
                PopupManager.Popup("Failed to delete Pool", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
                SelectedPool = null;
                return;
            }
            Pools.Remove(SelectedPool);
            SelectedPool = null;
            Clear();
        }

        private bool PoolSelected() => SelectedPool != null;

        private void ManageAllotmentsClick()
        {
            if (SelectedPool is null)
            {
                return;
            }
            var vm = Tools.Locator.AllotmentViewModel;
            vm.Pool = SelectedPool;
            DialogSupport.ShowDialog<AllotmentWindow>(vm);
            LoadPools(true);
            SelectedPool = null;
            Clear();
        }

        #endregion

        #region Events

        public event EventHandler FocusRequested;

        public event EventHandler AmountFocusRequested;

        #endregion

        #region Utility Methods

        private void LoadPools(bool reload = false)
        {
            try
            {
                Pools = new ObservableCollection<Pool>(_poolECL.Get());
            }
            catch (Exception ex)
            {
                string op = reload ? Constants.Reload : Constants.Load;
                PopupManager.Popup($"Failed to {op} Pools", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
                Cancel();
                return;
            }
        }

        private void Clear()
        {
            Name = string.Empty;
            Date = DateTime.Now;
            Description = string.Empty;
            Amount = 0M;
            FocusRequested?.Invoke(this, EventArgs.Empty);
            SelectedPool = null;
            _editing = false;
        }

        #endregion

        public PoolViewModel(IPoolECL pecl, IAllotmentECL aecl)
        {
            _poolECL = pecl;
            _allotmentECL = aecl;
            try
            {
                Tools.Locator.PoolRecalculator.Recalculate();
            }
            catch (Exception ex)
            {
                PopupManager.Popup("Failed to recalculate Pools", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
                Cancel();
                return;
            }
            LoadPools();
            Date = DateTime.Now;
        }

    }
}
