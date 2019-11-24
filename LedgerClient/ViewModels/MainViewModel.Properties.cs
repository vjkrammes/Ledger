using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using LedgerClient.ECL.DTO;

namespace LedgerClient.ViewModels
{
    public partial class MainViewModel
    {
        #region Window-related Properties

        private string _windowTitle;
        public string WindowTitle
        {
            get => _windowTitle;
            set => SetProperty(ref _windowTitle, value);
        }

        private string _banner;
        public string Banner
        {
            get => _banner;
            set => SetProperty(ref _banner, value);
        }

        private Visibility _statusbarVisibility;
        public Visibility StatusbarVisibility
        {
            get => _statusbarVisibility;
            set => SetProperty(ref _statusbarVisibility, value);
        }

        #endregion

        private string _shortTitle;
        public string ShortTitle
        {
            get => _shortTitle;
            set => SetProperty(ref _shortTitle, value);
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
            set
            {
                SetProperty(ref _selectedCompany, value);
                LoadIdentities(false);
                LoadAccounts(false);
            }
        }

        private ObservableCollection<Identity> _identities;
        public ObservableCollection<Identity> Identities
        {
            get => _identities;
            set => SetProperty(ref _identities, value);
        }

        private Identity _selectedIdentity;
        public Identity SelectedIdentity
        {
            get => _selectedIdentity;
            set => SetProperty(ref _selectedIdentity, value);
        }

        private ObservableCollection<Account> _accounts;
        public ObservableCollection<Account> Accounts
        {
            get => _accounts;
            set => SetProperty(ref _accounts, value);
        }

        private Account _selectedAccount;
        public Account SelectedAccount
        {
            get => _selectedAccount;
            set
            {
                SetProperty(ref _selectedAccount, value);
                LoadTransactions(false);
            }
        }

        private ObservableCollection<Transaction> _transactions;
        public ObservableCollection<Transaction> Transactions
        {
            get => _transactions;
            set => SetProperty(ref _transactions, value);
        }

        private Transaction _selectedTransaction;
        public Transaction SelectedTransaction
        {
            get => _selectedTransaction;
            set => SetProperty(ref _selectedTransaction, value);
        }
    }
}
