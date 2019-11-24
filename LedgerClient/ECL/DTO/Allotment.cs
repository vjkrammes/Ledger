using System;

using LedgerClient.Infrastructure;

namespace LedgerClient.ECL.DTO
{
    public class Allotment : NotifyBase, IEquatable<Allotment>, IComparable<Allotment>
    {
        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private int _poolId;
        public int PoolId
        {
            get => _poolId;
            set => SetProperty(ref _poolId, value);
        }

        private int _companyId;
        public int CompanyId
        {
            get => _companyId;
            set => SetProperty(ref _companyId, value);
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

        private byte[] _rowVersion;
        public byte[] RowVersion
        {
            get => _rowVersion;
            set => SetProperty(ref _rowVersion, value);
        }

        public Allotment()
        {
            Id = 0;
            PoolId = 0;
            CompanyId = 0;
            Date = default;
            Description = string.Empty;
            Amount = 0M;
            RowVersion = null;
        }

        public Allotment Clone() => new Allotment
        {
            Id = Id,
            PoolId = PoolId,
            CompanyId = CompanyId,
            Date = Date,
            Description = Description ?? string.Empty,
            Amount = Amount,
            RowVersion = RowVersion?.ArrayCopy()
        };

        public override bool Equals(object obj)
        {
            if (!(obj is Allotment a))
            {
                return false;
            }
            return a.Id == Id;
        }

        public bool Equals(Allotment a) => a is null ? false : a.Id == Id;

        public override int GetHashCode() => Id;

        public static bool operator ==(Allotment a, Allotment b) => (a, b) switch
        {
            (null, null) => true,
            (null, _) => false,
            (_, null) => false,
            (_, _) => a.Id == b.Id
        };

        public static bool operator !=(Allotment a, Allotment b) => !(a == b);

        public int CompareTo(Allotment other) => Date.CompareTo(other.Date);

        public static bool operator >(Allotment a, Allotment b) => a.CompareTo(b) > 0;

        public static bool operator <(Allotment a, Allotment b) => a.CompareTo(b) < 0;

        public static bool operator >=(Allotment a, Allotment b) => a.CompareTo(b) >= 0;

        public static bool operator <=(Allotment a, Allotment b) => a.CompareTo(b) <= 0;
    }
}
