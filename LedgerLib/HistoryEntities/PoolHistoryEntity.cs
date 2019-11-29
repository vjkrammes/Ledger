using System;
using System.ComponentModel.DataAnnotations;

namespace LedgerLib.HistoryEntities
{
    public class PoolHistoryEntity
    {
        [Required]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required, MaxLength(200)]
        public string Description { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public decimal Balance { get; set; }
    }
}
