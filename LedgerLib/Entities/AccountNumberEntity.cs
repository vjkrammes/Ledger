using System;
using System.ComponentModel.DataAnnotations;
using LedgerLib.Infrastructure;

namespace LedgerLib.Entities
{
    public class AccountNumberEntity
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int AccountId { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime StopDate { get; set; }
        [Required]
        public byte[] Salt { get; set; }
        [Required]
        public string Number { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public AccountNumberEntity()
        {
            Id = 0;
            AccountId = 0;
            StartDate = default;
            StopDate = DateTime.MaxValue;
            Salt = Salter.GeneratSalt(Constants.SaltLength);
            Number = string.Empty;
        }
    }
}
