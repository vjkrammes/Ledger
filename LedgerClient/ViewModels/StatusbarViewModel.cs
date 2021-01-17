using LedgerClient.ECL.DTO;
using LedgerClient.Infrastructure;

using LedgerLib;

using System;
using System.Windows;

namespace LedgerClient.ViewModels
{
    public class StatusbarViewModel : ViewModelBase
    {
        private readonly LedgerContext _context;

        private Visibility _statusbarVisibility;
        public Visibility StatusbarVisibility
        {
            get => _statusbarVisibility;
            set => SetProperty(ref _statusbarVisibility, value);
        }

        private int _accountCount;
        public int AccountCount
        {
            get => _accountCount;
            set => SetProperty(ref _accountCount, value);
        }

        private int _accountNumberCount;
        public int AccountNumberCount
        {
            get => _accountNumberCount;
            set => SetProperty(ref _accountNumberCount, value);
        }

        private int _accountTypeCount;
        public int AccountTypeCount
        {
            get => _accountTypeCount;
            set => SetProperty(ref _accountTypeCount, value);
        }

        private int _allotmentCount;
        public int AllotmentCount
        {
            get => _allotmentCount;
            set => SetProperty(ref _allotmentCount, value);
        }

        private int _companyCount;
        public int CompanyCount
        {
            get => _companyCount;
            set => SetProperty(ref _companyCount, value);
        }

        private int _identityCount;
        public int IdentityCount
        {
            get => _identityCount;
            set => SetProperty(ref _identityCount, value);
        }

        private int _poolCount;
        public int PoolCount
        {
            get => _poolCount;
            set => SetProperty(ref _poolCount, value);
        }

        private int _transactionCount;
        public int TransactionCount
        {
            get => _transactionCount;
            set => SetProperty(ref _transactionCount, value);
        }

        private double _databaseSize;
        public double DatabaseSize
        {
            get => _databaseSize;
            set => SetProperty(ref _databaseSize, value);
        }

        private double _quota;
        public double Quota
        {
            get => _quota;
            set => SetProperty(ref _quota, value);
        }

        private double _howFull;
        public double HowFull
        {
            get => _howFull;
            set => SetProperty(ref _howFull, value);
        }

        private decimal _accountTotal;
        public decimal AccountTotal
        {
            get => _accountTotal;
            set => SetProperty(ref _accountTotal, value);
        }

        private decimal _grandTotal;
        public decimal GrandTotal
        {
            get => _grandTotal;
            set => SetProperty(ref _grandTotal, value);
        }

        private DateTime _lastCopyDate;
        public DateTime LastCopyDate
        {
            get => _lastCopyDate;
            set => SetProperty(ref _lastCopyDate, value);
        }

        public void Update(Account selectedAccount = null)
        {
            var trans = Tools.Locator.TransactionECL;
            AccountCount = Tools.Locator.AccountECL.Count;
            AccountNumberCount = Tools.Locator.AccountNumberECL.Count;
            AccountTypeCount = Tools.Locator.AccountTypeECL.Count;
            AllotmentCount = Tools.Locator.AllotmentECL.Count;
            CompanyCount = Tools.Locator.CompanyECL.Count;
            IdentityCount = Tools.Locator.IdentityECL.Count;
            PoolCount = Tools.Locator.PoolECL.Count;
            TransactionCount = trans.Count;
            AccountTotal = selectedAccount is null ? 0M : trans.TotalForAccount(selectedAccount.Id);
            GrandTotal = trans.Total();
            DatabaseSize = _context.DatabaseInfo().Size;
            Quota = 10_000_000_000.0;
            HowFull = DatabaseSize / Quota;
        }

        public StatusbarViewModel(LedgerContext context) => _context = context;
    }
}
