using System;
using System.ComponentModel.DataAnnotations;

namespace LedgerLib.HistoryEntities
{
    public class AccountNumberHistoryEntity
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int AccountId { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required, MaxLength(200)]
        public string Number { get; set; }
    }
}
