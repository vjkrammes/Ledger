using System;

using LedgerClient.Infrastructure;

namespace LedgerClient.ECL.DTO
{
    public class AccountType : NotifyBase, IEquatable<AccountType>, IComparable<AccountType>
    {
        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private byte[] _rowVersion;
        public byte[] RowVersion
        {
            get => _rowVersion;
            set => SetProperty(ref _rowVersion, value);
        }

        public AccountType()
        {
            Id = 0;
            Description = string.Empty;
            RowVersion = null;
        }

        public AccountType Clone() => new AccountType
        {
            Id = Id,
            Description = Description ?? string.Empty,
            RowVersion = RowVersion?.ArrayCopy()
        };

        public override string ToString() => Description;

        public override bool Equals(object obj)
        {
            if (!(obj is AccountType a))
            {
                return false;
            }
            return a.Id == Id;
        }

        public bool Equals(AccountType a) => a is null ? false : a.Id == Id;

        public override int GetHashCode() => Id;

        public static bool operator ==(AccountType a, AccountType b) => (a, b) switch
        {
            (null, null) => true,
            (null, _) => false,
            (_, null) => false,
            (_, _) => a.Id == b.Id
        };

        public static bool operator !=(AccountType a, AccountType b) => !(a == b);

        public int CompareTo(AccountType other) => Description.CompareTo(other.Description);

        public static bool operator >(AccountType a, AccountType b) => a.CompareTo(b) > 0;

        public static bool operator <(AccountType a, AccountType b) => a.CompareTo(b) < 0;

        public static bool operator >=(AccountType a, AccountType b) => a.CompareTo(b) >= 0;

        public static bool operator <=(AccountType a, AccountType b) => a.CompareTo(b) <= 0;
    }
}
