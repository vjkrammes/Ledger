using System;

using LedgerClient.Infrastructure;

namespace LedgerClient.ECL.DTO
{
    public class Pool : NotifyBase, IEquatable<Pool>, IComparable<Pool>
    {
        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private DateTime _date;
        public DateTime Date
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

        private decimal _balance;
        public decimal Balance
        {
            get => _balance;
            set => SetProperty(ref _balance, value);
        }

        private byte[] _rowVersion;
        public byte[] RowVersion
        {
            get => _rowVersion;
            set => SetProperty(ref _rowVersion, value);
        }

        public Pool()
        {
            Id = 0;
            Name = string.Empty;
            Date = default;
            Description = string.Empty;
            Amount = 0M;
            Balance = 0M;
            RowVersion = null;
        }

        public Pool Clone() => new Pool
        {
            Id = Id,
            Name = Name ?? string.Empty,
            Date = Date,
            Description = Description ?? string.Empty,
            Amount = Amount,
            Balance = Balance,
            RowVersion = RowVersion?.ArrayCopy()
        };

        public override string ToString() => Name;

        public override bool Equals(object obj)
        {
            if (!(obj is Pool p))
            {
                return false;
            }
            return p.Id == Id;
        }

        public bool Equals(Pool p) => p is null ? false : p.Id == Id;

        public override int GetHashCode() => Id;

        public static bool operator ==(Pool a, Pool b) => (a, b) switch
        {
            (null, null) => true,
            (null, _) => false,
            (_, null) => false,
            (_, _) => a.Id == b.Id
        };

        public static bool operator !=(Pool a, Pool b) => !(a == b);

        public int CompareTo(Pool other) => Name.CompareTo(other.Name);

        public static bool operator >(Pool a, Pool b) => a.CompareTo(b) > 0;

        public static bool operator <(Pool a, Pool b) => a.CompareTo(b) < 0;

        public static bool operator >=(Pool a, Pool b) => a.CompareTo(b) >= 0;

        public static bool operator <=(Pool a, Pool b) => a.CompareTo(b) <= 0;
    }
}
