using System.ComponentModel.DataAnnotations;
using LedgerLib.Infrastructure;

namespace LedgerLib.Entities
{
    public class IdentityEntity
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int CompanyId { get; set; }
        [Required, MaxLength(1024)]
        public string URL { get; set; }
        [Required]
        public byte[] UserSalt { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        [Required]
        public string Password { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public CompanyEntity Company { get; set; }

        public IdentityEntity()
        {
            Id = 0;
            CompanyId = 0;
            URL = string.Empty;
            UserSalt = Salter.GeneratSalt(Constants.SaltLength);
            UserId = string.Empty;
            PasswordSalt = Salter.GeneratSalt(Constants.SaltLength);
            Password = string.Empty;
            Company = null;
        }
    }
}
