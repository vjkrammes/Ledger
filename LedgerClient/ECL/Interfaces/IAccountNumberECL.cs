using System.Collections.Generic;

using LedgerClient.ECL.DTO;

using LedgerLib.Entities;

namespace LedgerClient.ECL.Interfaces
{
    public interface IAccountNumberECL : IECL<AccountNumberEntity, AccountNumber>
    {
        IEnumerable<AccountNumber> GetForAccount(int aid);
        AccountNumber Read(int id);
        AccountNumber Current(int aid);
        bool AccountHasAccountNumbers(int aid);
    }
}
