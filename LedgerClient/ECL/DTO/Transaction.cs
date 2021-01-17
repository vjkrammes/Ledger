using LedgerClient.Infrastructure;

using System;

namespace LedgerClient.ECL.DTO
{
    public class Transaction : NotifyBase, IEquatable<Transaction>, IComparable<Transaction>
    {
        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private int _accountId;
        public int AccountId
        {
            get => _accountId;
            set => SetProperty(ref _accountId, value);
        }

        private DateTime _date;
        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

        private decimal _balance;
        public decimal Balance
        {
            get => _balance;
            set => SetProperty(ref _balance, value);
        }

        private decimal _payment;
        public decimal Payment
        {
            get => _payment;
            set => SetProperty(ref _payment, value);
        }

        private string _reference;
        public string Reference
        {
            get => _reference;
            set => SetProperty(ref _reference, value);
        }

        private byte[] _rowVersion;
        public byte[] RowVersion
        {
            get => _rowVersion;
            set => SetProperty(ref _rowVersion, value);
        }

        public Transaction()
        {
            Id = 0;
            AccountId = 0;
            Date = default;
            Balance = 0M;
            Payment = 0M;
            Reference = string.Empty;
            RowVersion = null;
        }

        public Transaction Clone() => new Transaction
        {
            Id = Id,
            AccountId = AccountId,
            Date = Date,
            Balance = Balance,
            Payment = Payment,
            Reference = Reference ?? string.Empty,
            RowVersion = RowVersion?.ArrayCopy()
        };

        public override bool Equals(object obj)
        {
            if (obj is not Transaction t)
            {
                return false;
            }
            return t.Id == Id;
        }

        public bool Equals(Transaction t) => t is not null && t.Id == Id;

        public override int GetHashCode() => Id;

        public static bool operator ==(Transaction a, Transaction b) => (a, b) switch
        {
            (null, null) => true,
            (null, _) => false,
            (_, null) => false,
            (_, _) => a.Id == b.Id
        };

        public static bool operator !=(Transaction a, Transaction b) => !(a == b);

        public int CompareTo(Transaction other) => Date.CompareTo(other.Date);

        public static bool operator >(Transaction a, Transaction b) => a.CompareTo(b) > 0;

        public static bool operator <(Transaction a, Transaction b) => a.CompareTo(b) < 0;

        public static bool operator >=(Transaction a, Transaction b) => a.CompareTo(b) >= 0;

        public static bool operator <=(Transaction a, Transaction b) => a.CompareTo(b) <= 0;
    }
}
