using System;

using LedgerClient.Infrastructure;
using LedgerLib.Infrastructure;

namespace LedgerClient.ECL.DTO
{
    public class Identity : NotifyBase, IEquatable<Identity>
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

        private string _url;
        public string URL
        {
            get => _url;
            set => SetProperty(ref _url, value);
        }

        private byte[] _userSalt;
        public byte[] UserSalt
        {
            get => _userSalt;
            set => SetProperty(ref _userSalt, value);
        }

        private string _userId;
        public string UserId
        {
            get => _userId;
            set => SetProperty(ref _userId, value);
        }

        private byte[] _passwordSalt;
        public byte[] PasswordSalt
        {
            get => _passwordSalt;
            set => SetProperty(ref _passwordSalt, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private byte[] _rowVersion;
        public byte[] RowVersion
        {
            get => _rowVersion;
            set => SetProperty(ref _rowVersion, value);
        }

        public Company _company;
        public Company Company
        {
            get => _company;
            set => SetProperty(ref _company, value);
        }

        public Identity()
        {
            Id = 0;
            CompanyId = 0;
            URL = string.Empty;
            UserSalt = Tools.GenerateSalt(Constants.SaltLength);
            UserId = string.Empty;
            PasswordSalt = Tools.GenerateSalt(Constants.SaltLength);
            Password = string.Empty;
            Company = null;
            RowVersion = null;
        }

        public Identity Clone() => new Identity
        {
            Id = Id,
            CompanyId = CompanyId,
            URL = URL ?? string.Empty,
            UserSalt = UserSalt?.ArrayCopy(),
            UserId = UserId ?? string.Empty,
            PasswordSalt = PasswordSalt?.ArrayCopy(),
            Password = Password ?? string.Empty,
            Company = Company?.Clone(),
            RowVersion = RowVersion?.ArrayCopy()
        };

        public override bool Equals(object obj)
        {
            if (!(obj is Identity i))
            {
                return false;
            }
            return i.Id == Id;
        }

        public bool Equals(Identity i) => i is null ? false : i.Id == Id;

        public override int GetHashCode() => Id;

        public static bool operator ==(Identity a, Identity b) => (a, b) switch
        {
            (null, null) => true,
            (null, _) => false,
            (_, null) => false,
            (_, _) => a.Id == b.Id
        };

        public static bool operator !=(Identity a, Identity b) => !(a == b);
    }
}
