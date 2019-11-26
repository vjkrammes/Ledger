using System.Collections.Generic;

using LedgerLib.Entities;

namespace LedgerLib.Interfaces
{
    public interface IAccountNumberDAL : IDAL<AccountNumberEntity>
    {
        void DeleteForAccount(int aid);
        IEnumerable<AccountNumberEntity> GetForAccount(int aid);
        AccountNumberEntity Read(int id);
        AccountNumberEntity Current(int aid);
        bool AccountHasAccountNumbers(int aid);
    }
}
