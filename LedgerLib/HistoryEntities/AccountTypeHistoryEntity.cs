using System.ComponentModel.DataAnnotations;

namespace LedgerLib.HistoryEntities
{
    public class AccountTypeHistoryEntity
    {
        [Required]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Description { get; set; }
        [Required, MaxLength(250)]
        public string ImageUri { get; set; }
    }
}
