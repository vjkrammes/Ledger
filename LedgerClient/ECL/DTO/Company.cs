using System;

using LedgerClient.Infrastructure;

namespace LedgerClient.ECL.DTO
{
    public class Company : NotifyBase, IEquatable<Company>, IComparable<Company>
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

        private string _address1;
        public string Address1
        {
            get => _address1;
            set => SetProperty(ref _address1, value);
        }

        private string _address2;
        public string Address2
        {
            get => _address2;
            set => SetProperty(ref _address2, value);
        }

        private string _city;
        public string City
        {
            get => _city;
            set => SetProperty(ref _city, value);
        }

        private string _state;
        public string State
        {
            get => _state;
            set => SetProperty(ref _state, value);
        }

        private string _postalCode;
        public string PostalCode
        {
            get => _postalCode;
            set => SetProperty(ref _postalCode, value);
        }

        private string _phone;
        public string Phone
        {
            get => _phone;
            set => SetProperty(ref _phone, value);
        }

        private string _url;
        public string URL
        {
            get => _url;
            set => SetProperty(ref _url, value);
        }

        private bool _isPayee;
        public bool IsPayee
        {
            get => _isPayee;
            set => SetProperty(ref _isPayee, value);
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

        public Company()
        {
            Id = 0;
            Name = string.Empty;
            Address1 = string.Empty;
            Address2 = string.Empty;
            City = string.Empty;
            State = string.Empty;
            PostalCode = string.Empty;
            Phone = string.Empty;
            URL = string.Empty;
            IsPayee = false;
            Comments = string.Empty;
            RowVersion = null;
        }

        public Company Clone() => new Company
        {
            Id = Id,
            Name = Name ?? string.Empty,
            Address1 = Address1 ?? string.Empty,
            Address2 = Address2 ?? string.Empty,
            City = City ?? string.Empty,
            State = State ?? string.Empty,
            PostalCode = PostalCode ?? string.Empty,
            Phone = Phone ?? string.Empty,
            URL = URL ?? string.Empty,
            IsPayee = IsPayee,
            Comments = Comments ?? string.Empty,
            RowVersion = RowVersion?.ArrayCopy()
        };

        public override string ToString() => Name;

        public override bool Equals(object obj)
        {
            if (!(obj is Company c))
            {
                return false;
            }
            return c.Id == Id;
        }

        public bool Equals(Company c) => c is null ? false : c.Id == Id;

        public override int GetHashCode() => Id;

        public static bool operator ==(Company a, Company b) => (a, b) switch
        {
            (null, null) => true,
            (null, _) => false,
            (_, null) => false,
            (_, _) => a.Id == b.Id
        };

        public static bool operator !=(Company a, Company b) => !(a == b);

        public int CompareTo(Company other) => Name.CompareTo(other.Name);

        public static bool operator >(Company a, Company b) => a.CompareTo(b) > 0;

        public static bool operator <(Company a, Company b) => a.CompareTo(b) < 0;

        public static bool operator >=(Company a, Company b) => a.CompareTo(b) >= 0;

        public static bool operator <=(Company a, Company b) => a.CompareTo(b) <= 0;
    }
}
