using System;
using System.ComponentModel.DataAnnotations;

using LedgerLib.Infrastructure;

namespace LedgerLib.Entities
{
    public class PoolEntity
    {
        [Required]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Description { get; set; }
        [Required, Positive]
        public decimal Amount { get; set; }
        [Required, NonNegative]
        public decimal Balance { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
