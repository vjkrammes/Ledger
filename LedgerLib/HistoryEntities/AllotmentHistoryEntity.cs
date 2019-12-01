using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LedgerLib.HistoryEntities
{
    public class AllotmentHistoryEntity
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int PoolId { get; set; }
        [Required, ForeignKey("Payee")]
        public int PayeeId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required, MaxLength(200)]
        public string Description { get; set; }
        [Required]
        public decimal Amount { get; set; }

        public PayeeHistoryEntity Payee { get; set; }
    }
}
