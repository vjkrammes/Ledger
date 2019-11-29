using System.Collections.Generic;
using System.Linq;

using LedgerLib.HistoryEntities;
using LedgerLib.Interfaces;

namespace LedgerLib.HistoryDAL
{
    public class AccountHistoryDAL : HistoryDAL<AccountHistoryEntity, HistoryContext>, IAccountHistoryDAL
    {

        public AccountHistoryDAL(HistoryContext context) : base(context) { }

        public IEnumerable<AccountHistoryEntity> GetForPayee(int pid) => Get(x => x.PayeeId == pid);

        public IEnumerable<AccountHistoryEntity> GetForAccountType(int atid) => Get(x => x.AccountTypeId == atid);

        public AccountHistoryEntity Read(int id) => Get(x => x.Id == id).SingleOrDefault();
    }
}
