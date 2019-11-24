using System;
using System.ComponentModel.DataAnnotations;

namespace LedgerLib.Entities
{
    public class AllotmentEntity
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int PoolId { get; set; }
        [Required]
        public int CompanyId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
