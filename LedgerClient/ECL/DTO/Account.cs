using System;

using LedgerClient.Infrastructure;

using LedgerLib.Infrastructure;

namespace LedgerClient.ECL.DTO
{
    public class Account : NotifyBase, IEquatable<Account>
    {
        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private int _companyId;
        public int CompanyId
        {
            get => _companyId;
            set => SetProperty(ref _companyId, value);
        }

        private int _accountTypeId;
        public int AccountTypeId
        {
            get => _accountTypeId;
            set => SetProperty(ref _accountTypeId, value);
        }

        private DueDateType _dueDateType;
        public DueDateType DueDateType
        {
            get => _dueDateType;
            set => SetProperty(ref _dueDateType, value);
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
            set => SetProperty(ref _isPayable, value);
        }

        private string _comments;
        public string Comments
        {
            get => _comments;
            set => SetProperty(ref _comments, value);
        }

        private byte[] _rowVersion;
        public byte[] RowVersion
        {
            get => _rowVersion;
            set => SetProperty(ref _rowVersion, value);
        }

        private AccountType _accountType;
        public AccountType AccountType
        {
            get => _accountType;
            set => SetProperty(ref _accountType, value);
        }

        private AccountNumber _accountNumber;
        public AccountNumber AccountNumber
        {
            get => _accountNumber;
            set => SetProperty(ref _accountNumber, value);
        }

        public Account()
        {
            Id = 0;
            CompanyId = 0;
            AccountTypeId = 0;
            DueDateType = DueDateType.Unspecified;
            Month = 0;
            Day = 0;
            IsPayable = false;
            Comments = string.Empty;
            AccountType = null;
            AccountNumber = null;
            RowVersion = null;
        }

        public Account Clone() => new Account
        {
            Id = Id,
            CompanyId = CompanyId,
            AccountTypeId = AccountTypeId,
            DueDateType = DueDateType,
            Month = Month,
            Day = Day,
            IsPayable = IsPayable,
            Comments = Comments ?? string.Empty,
            AccountType = AccountType?.Clone(),
            AccountNumber = AccountNumber?.Clone(),
            RowVersion = RowVersion?.ArrayCopy()
        };

        public override string ToString() => Tools.FormatAccountNumber(this, AccountNumber);

        public override bool Equals(object obj)
        {
            if (!(obj is Account a))
            {
                return false;
            }
            return a.Id == Id;
        }

        public bool Equals(Account a) => a is null ? false : a.Id == Id;

        public override int GetHashCode() => Id;

        public static bool operator ==(Account a, Account b) => (a, b) switch
        {
            (null, null) => true,
            (null, _) => false,
            (_, null) => false,
            (_, _) => a.Id == b.Id
        };

        public static bool operator !=(Account a, Account b) => !(a == b);

    }
}
