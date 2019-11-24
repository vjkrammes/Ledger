using LedgerLib.Entities;

namespace LedgerLib.Interfaces
{
    public interface IAccountTypeDAL : IDAL<AccountTypeEntity>
    {
        AccountTypeEntity Read(int id);
        AccountTypeEntity Read(string description);
    }
}
