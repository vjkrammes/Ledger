using LedgerLib.Infrastructure;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LedgerLib.Entities
{
    public class AccountEntity
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int CompanyId { get; set; }
        [Required]
        public int AccountTypeId { get; set; }
        [Required, NonNegative]
        public DueDateType DueDateType { get; set; }
        [Required, NonNegative]
        public int Month { get; set; }
        [Required, NonNegative]
        public int Day { get; set; }
        [Required]
        public bool IsPayable { get; set; }
        [Required]
        public string Comments { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public AccountTypeEntity AccountType { get; set; }

        public AccountNumberEntity AccountNumber { get; set; }


        public string DueDate()
        {
            var months = new List<string>
            {
                "", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"
            };
            var sb = new StringBuilder();
            sb.Append(DueDateType.GetDescriptionFromEnumValue());
            switch (DueDateType)
            {
                case DueDateType.Annually:
                    sb.Append(" on the ");
                    sb.Append(Day.Ordinalize());
                    sb.Append(" of ");
                    sb.Append(months[Month]);
                    break;
                case DueDateType.Quarterly:
                    sb.Append(" on the ");
                    sb.Append(Day.Ordinalize());
                    sb.Append(" of ");
                    for (var i = 0; i < 12; i += 3)
                    {
                        sb.Append(months[(Month + i) % 12]);
                        if (i == 6)
                        {
                            sb.Append(" and ");
                        }
                        else if (i != 9)
                        {
                            sb.Append(", ");
                        }
                    }
                    break;
                case DueDateType.Monthly:
                    sb.Append(" on the ");
                    sb.Append(Day.Ordinalize());
                    break;
            }
            return sb.ToString();
        }

    }
}
