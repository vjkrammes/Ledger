using System.ComponentModel.DataAnnotations;

namespace LedgerLib.HistoryEntities
{
    public class PayeeHistoryEntity
    {
        [Required]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [Required, MaxLength(200)]
        public string Address { get; set; }
        [Required, MaxLength(50)]
        public string City { get; set; }
        [Required, MaxLength(50)]
        public string State { get; set; }
        [Required, MaxLength(50)]
        public string ZIP { get; set; }
        [Required, MaxLength(50)]
        public string Phone { get; set; }
        [Required, MaxLength(200)]
        public string URL { get; set; }
    }
}
