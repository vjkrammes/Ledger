using System;

using LedgerClient.Infrastructure;
using LedgerLib.Infrastructure;

namespace LedgerClient.ECL.DTO
{
    public class AccountNumber : NotifyBase, IEquatable<AccountNumber>
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

        private DateTime _startDate;
        public DateTime StartDate
        {
            get => _startDate;
            set => SetProperty(ref _startDate, value);
        }

        private DateTime _stopDate;
        public DateTime StopDate
        {
            get => _stopDate;
            set => SetProperty(ref _stopDate, value);
        }

        private byte[] _salt;
        public byte[] Salt
        {
            get => _salt;
            set => SetProperty(ref _salt, value);
        }

        private string _number;
        public string Number
        {
            get => _number;
            set => SetProperty(ref _number, value);
        }

        private byte[] _rowVersion;
        public byte[] RowVersion
        {
            get => _rowVersion;
            set => SetProperty(ref _rowVersion, value);
        }

        public AccountNumber()
        {
            Id = 0;
            AccountId = 0;
            StartDate = default;
            StopDate = DateTime.MaxValue;
            Salt = Tools.GenerateSalt(Constants.SaltLength);
            Number = string.Empty;
            RowVersion = null;
        }

        public AccountNumber Clone() => new AccountNumber
        {
            Id = Id,
            AccountId = AccountId,
            StartDate = StartDate,
            StopDate = StopDate,
            Salt = Salt.ArrayCopy(),
            Number = Number ?? string.Empty,
            RowVersion = RowVersion?.ArrayCopy()
        };

        public override bool Equals(object obj)
        {
            if (!(obj is AccountNumber a))
            {
                return false;
            }
            return a.Id == Id;
        }

        public bool Equals(AccountNumber a) => a is null ? false : a.Id == Id;

        public override int GetHashCode() => Id;

        public static bool operator ==(AccountNumber a, AccountNumber b) => (a, b) switch
        {
            (null, null) => true,
            (null, _) => false,
            (_, null) => false,
            (_, _) => a.Id == b.Id
        };

        public static bool operator !=(AccountNumber a, AccountNumber b) => !(a == b);

    }
}
