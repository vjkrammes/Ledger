using System;
using System.ComponentModel.DataAnnotations;

using LedgerLib.Infrastructure;

namespace LedgerLib.Entities
{
    public class TransactionEntity
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int AccountId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required, Positive]
        public decimal Balance { get; set; }
        [Required, Positive]
        public decimal Payment { get; set; }
        [Required, MaxLength(100)]
        public string Reference { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
