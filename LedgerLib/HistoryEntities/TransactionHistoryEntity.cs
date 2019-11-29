using System;
using System.ComponentModel.DataAnnotations;

namespace LedgerLib.HistoryEntities
{
    public class TransactionHistoryEntity
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int AccountId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public decimal Balance { get; set; }
        [Required]
        public decimal Payment { get; set; }
        [Required, MaxLength(100)]
        public string Reference { get; set; }
    }
}
