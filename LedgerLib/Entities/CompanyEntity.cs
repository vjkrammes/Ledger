using System.ComponentModel.DataAnnotations;

namespace LedgerLib.Entities
{
    public class CompanyEntity
    {
        [Required]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [Required, MaxLength(50)]
        public string Address1 { get; set; }
        [Required, MaxLength(50)]
        public string Address2 { get; set; }
        [Required, MaxLength(50)]
        public string City { get; set; }
        [Required, MaxLength(50)]
        public string State { get; set; }
        [Required, MaxLength(50)]
        public string PostalCode { get; set; }
        [Required, MaxLength(50)]
        public string Phone { get; set; }
        [Required, MaxLength(1024)]
        public string URL { get; set; }
        [Required]
        public bool IsPayee { get; set; }
        [Required]
        public string Comments { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
