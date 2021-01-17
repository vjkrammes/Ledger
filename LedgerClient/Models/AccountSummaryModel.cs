using LedgerClient.Infrastructure;
using LedgerClient.Interfaces;

using LedgerLib.Entities;
using LedgerLib.Infrastructure;

using System;

namespace LedgerClient.Models
{
    public class AccountSummaryModel : NotifyBase
    {
        private string _company;
        public string Company
        {
            get => _company;
            set => SetProperty(ref _company, value);
        }

        private string _accountType;
        public string AccountType
        {
            get => _accountType;
            set => SetProperty(ref _accountType, value);
        }

        private string _accountNumber;
        public string AccountNumber
        {
            get => _accountNumber;
            set => SetProperty(ref _accountNumber, value);
        }

        private string _dueDate;
        public string DueDate
        {
            get => _dueDate;
            set => SetProperty(ref _dueDate, value);
        }

        private DateTime _lastTransaction;
        public DateTime LastTransaction
        {
            get => _lastTransaction;
            set => SetProperty(ref _lastTransaction, value);
        }

        private decimal _lastBalance;
        public decimal LastBalance
        {
            get => _lastBalance;
            set => SetProperty(ref _lastBalance, value);
        }

        private decimal _lastPayment;
        public decimal LastPayment
        {
            get => _lastPayment;
            set => SetProperty(ref _lastPayment, value);
        }

        private string _lastReference;
        public string LastReference
        {
            get => _lastReference;
            set => SetProperty(ref _lastReference, value);
        }

        public AccountSummaryModel()
        {
            Company = string.Empty;
            AccountType = string.Empty;
            AccountNumber = string.Empty;
            DueDate = string.Empty;
            LastTransaction = default;
            LastBalance = 0M;
            LastPayment = 0M;
            LastReference = string.Empty;
        }

        public AccountSummaryModel(CompanyEntity company, AccountEntity account, TransactionEntity lastTransaction, 
            IStringCypherService cypher, IPasswordManager manager)
        {
            Company = company.Name;
            AccountType = account.AccountType.Description;
            AccountNumber = cypher.Decrypt(account.AccountNumber.Number, manager.Get(Constants.LedgerPassword), account.AccountNumber.Salt);
            DueDate = account.DueDate();
            LastTransaction = lastTransaction.Date;
            LastBalance = lastTransaction.Balance;
            LastPayment = lastTransaction.Payment;
            LastReference = lastTransaction.Reference;
        }
    }
}
