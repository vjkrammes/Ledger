using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LedgerLib.Infrastructure;

namespace LedgerLib.HistoryEntities
{
    public class AccountHistoryEntity
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int PayeeId { get; set; }
        [Required]
        public int AccountTypeId { get; set; }
        [Required]
        public DueDateType DueDateType { get; set; }
        [Required]
        public int Month { get; set; }
        [Required]
        public int Day { get; set; }
        
        [ForeignKey("AccountTypeId")]
        public AccountTypeHistoryEntity AccountType { get; set; }

        [ForeignKey("AccountId")]
        public IList<AccountNumberHistoryEntity> AccountNumbers { get; set; }
    }
}
