using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using LedgerLib.HistoryEntities;
using LedgerLib.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace LedgerLib.HistoryDAL
{
    public class AllotmentHistoryDAL : HistoryDAL<AllotmentHistoryEntity, HistoryContext>, IAllotmentHistoryDAL
    {
        public AllotmentHistoryDAL(HistoryContext context) : base(context) { }

        public override IEnumerable<AllotmentHistoryEntity> Get(Expression<Func<AllotmentHistoryEntity, bool>> pred = null)
        {
            return pred switch
            {
                null => _dbset.OrderByDescending(x => x.Date).AsNoTracking().ToList(),
                _ => _dbset.Where(pred).OrderByDescending(x => x.Date).AsNoTracking().ToList()
            };
        }

        public IEnumerable<AllotmentHistoryEntity> GetForPool(int pid) => Get(x => x.PoolId == pid);

        public IEnumerable<AllotmentHistoryEntity> GetForPayee(int pid) => Get(x => x.PayeeId == pid);

        public AllotmentHistoryEntity Read(int id) => Get(x => x.Id == id).SingleOrDefault();
    }
}
