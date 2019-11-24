using System.ComponentModel.DataAnnotations;

namespace LedgerLib.Entities
{
    public class AccountTypeEntity
    {
        [Required]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Description { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public override string ToString() => Description;
    }
}
