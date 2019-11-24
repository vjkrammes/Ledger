using LedgerClient.ECL.DTO;

using LedgerLib.Entities;

namespace LedgerClient.ECL.Interfaces
{
    public interface IAccountTypeECL : IECL<AccountTypeEntity, AccountType>
    {
        AccountType Read(int id);
        AccountType Read(string description);
    }
}
