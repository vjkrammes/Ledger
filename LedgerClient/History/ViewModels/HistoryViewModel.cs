using LedgerClient.History.Views;
using LedgerClient.Infrastructure;

using LedgerLib.HistoryEntities;
using LedgerLib.Infrastructure;
using LedgerLib.Interfaces;

using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LedgerClient.History.ViewModels
{
    public class HistoryViewModel : ViewModelBase
    {
        #region Properties

        private ObservableCollection<PayeeHistoryEntity> _payees;
        public ObservableCollection<PayeeHistoryEntity> Payees
        {
            get => _payees;
            set => SetProperty(ref _payees, value);
        }

        private PayeeHistoryEntity _selectedPayee;
        public PayeeHistoryEntity SelectedPayee
        {
            get => _selectedPayee;
            set
            {
                SetProperty(ref _selectedPayee, value);
                LoadAccounts();
            }
        }

        private ObservableCollection<AccountHistoryEntity> _accounts;
        public ObservableCollection<AccountHistoryEntity> Accounts
        {
            get => _accounts;
            set => SetProperty(ref _accounts, value);
        }

        private AccountHistoryEntity _selectedAccount;
        public AccountHistoryEntity SelectedAccount
        {
            get => _selectedAccount;
            set
            {
                SetProperty(ref _selectedAccount, value);
                LoadTransactions();
            }
        }

        private ObservableCollection<TransactionHistoryEntity> _transactions;
        public ObservableCollection<TransactionHistoryEntity> Transactions
        {
            get => _transactions;
            set => SetProperty(ref _transactions, value);
        }

        private TransactionHistoryEntity _selectedTransaction;
        public TransactionHistoryEntity SelectedTransaction
        {
            get => _selectedTransaction;
            set => SetProperty(ref _selectedTransaction, value);
        }

        private ObservableCollection<PoolHistoryEntity> _pools;
        public ObservableCollection<PoolHistoryEntity> Pools
        {
            get => _pools;
            set => SetProperty(ref _pools, value);
        }

        private PoolHistoryEntity _selectedPool;
        public PoolHistoryEntity SelectedPool
        {
            get => _selectedPool;
            set
            {
                SetProperty(ref _selectedPool, value);
                LoadAllotments();
            }
        }

        private ObservableCollection<AllotmentHistoryEntity> _allotments;
        public ObservableCollection<AllotmentHistoryEntity> Allotments
        {
            get => _allotments;
            set => SetProperty(ref _allotments, value);
        }

        private AllotmentHistoryEntity _selectedAllotment;
        public AllotmentHistoryEntity SelectedAllotment
        {
            get => _selectedAllotment;
            set => SetProperty(ref _selectedAllotment, value);
        }

        private readonly IAccountHistoryDAL _accountDAL;
        private readonly IAllotmentHistoryDAL _allotmentDAL;
        private readonly IPayeeHistoryDAL _payeeDAL;
        private readonly IPoolHistoryDAL _poolDAL;
        private readonly ITransactionHistoryDAL _transactionDAL;

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

        private static void WindowLoaded()
        {
        }

        #endregion

        #region Utility Methods

        private void LoadAccounts()
        {
            Transactions = null;
            Accounts = null;
            if (SelectedPayee != null)
            {
                try
                {
                    Accounts = new ObservableCollection<AccountHistoryEntity>(_accountDAL.GetForPayee(SelectedPayee.Id));
                }
                catch (Exception ex)
                {
                    PopupManager.Popup("Failed to load Accounts", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
                }
            }
        }

        private void LoadTransactions()
        {
            Transactions = null;
            if (SelectedAccount != null)
            {
                try
                {
                    Transactions =  new ObservableCollection<TransactionHistoryEntity>(_transactionDAL.GetForAccount(SelectedAccount.Id));
                }
                catch (Exception ex)
                {
                    PopupManager.Popup("Failed to load Transactions", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
                }
            }
        }

        private void LoadAllotments()
        {
            Allotments = null;
            if (SelectedPool != null)
            {
                try
                {
                    Allotments = new ObservableCollection<AllotmentHistoryEntity>(_allotmentDAL.GetForPool(SelectedPool.Id));
                }
                catch (Exception ex)
                {
                    PopupManager.Popup("Failed to load Allotments", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
                }
            }
        }

        #endregion

        public HistoryViewModel()
        {
            _accountDAL = Tools.Locator.AccountHistoryDAL;
            _allotmentDAL = Tools.Locator.AllotmentHistoryDAL;
            _payeeDAL = Tools.Locator.PayeeHistoryDAL;
            _poolDAL = Tools.Locator.PoolHistoryDAL;
            _transactionDAL = Tools.Locator.TransactionHistoryDAL;

            var vm = Tools.Locator.Ledger5PasswordViewModel;
            if (DialogSupport.ShowDialog<Ledger5PasswordWindow>(vm) != true)
            {
                Cancel();
                return;
            }
            Tools.Locator.PasswordManager.Set(vm.Password, Constants.Ledger5Password);
            try
            {
                Payees = new ObservableCollection<PayeeHistoryEntity>(_payeeDAL.Get());
            }
            catch (Exception ex)
            {
                PopupManager.Popup("Failed to load Payees", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
                Cancel();
                return;
            }

            try
            {
                Pools = new ObservableCollection<PoolHistoryEntity>(_poolDAL.Get());
            }
            catch (Exception ex)
            {
                PopupManager.Popup("Failed to load Pools", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
                Cancel();
                return;
            }
        }
    }
}
