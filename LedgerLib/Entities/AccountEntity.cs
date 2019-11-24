using System.ComponentModel.DataAnnotations;

using LedgerLib.Infrastructure;

namespace LedgerLib.Entities
{
    public class AccountEntity
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int CompanyId { get; set; }
        [Required]
        public int AccountTypeId { get; set; }
        [Required, NonNegative]
        public DueDateType DueDateType { get; set; }
        [Required, NonNegative]
        public int Month { get; set; }
        [Required, NonNegative]
        public int Day { get; set; }
        [Required]
        public bool IsPayable { get; set; }
        [Required]
        public string Comments { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public AccountTypeEntity AccountType { get; set; }

        public AccountNumberEntity AccountNumber { get; set; }
    }
}
