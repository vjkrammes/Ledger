using System.Collections.Generic;

using LedgerClient.ECL.DTO;

using LedgerLib.Entities;

namespace LedgerClient.ECL.Interfaces
{
    public interface IAccountECL : IECL<AccountEntity, Account>
    {
        IEnumerable<Account> GetForCompany(int cid);
        IEnumerable<Account> GetForAccountType(int atid);
        Account Read(int id);
        bool CompanyHasAccounts(int cid);
        bool AccountTypeHasAccounts(int atid);
    }
}
