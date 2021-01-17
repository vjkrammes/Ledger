using LedgerLib.HistoryEntities;
using LedgerLib.Interfaces;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LedgerLib.HistoryDAL
{
    public class AllotmentHistoryDAL : HistoryDAL<AllotmentHistoryEntity, HistoryContext>, IAllotmentHistoryDAL
    {
        public AllotmentHistoryDAL(HistoryContext context) : base(context) { }

        public override IEnumerable<AllotmentHistoryEntity> Get(Expression<Func<AllotmentHistoryEntity, bool>> pred = null) => 
            pred switch
            {
                null => DbSet
                    .Include(x => x.Payee)
                    .OrderByDescending(x => x.Date).AsNoTracking().ToList(),
                _ => DbSet
                    .Include(x => x.Payee)
                    .Where(pred)
                    .OrderByDescending(x => x.Date).AsNoTracking().ToList()
            };

        public IEnumerable<AllotmentHistoryEntity> GetForPool(int pid) => Get(x => x.PoolId == pid);

        public IEnumerable<AllotmentHistoryEntity> GetForPayee(int pid) => Get(x => x.PayeeId == pid);

        public AllotmentHistoryEntity Read(int id) => Get(x => x.Id == id).SingleOrDefault();
    }
}
