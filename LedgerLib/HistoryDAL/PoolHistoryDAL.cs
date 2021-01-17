using LedgerLib.HistoryEntities;
using LedgerLib.Interfaces;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LedgerLib.HistoryDAL
{
    public class PoolHistoryDAL : HistoryDAL<PoolHistoryEntity, HistoryContext>, IPoolHistoryDAL
    {
        public PoolHistoryDAL(HistoryContext context) : base(context) { }

        public override IEnumerable<PoolHistoryEntity> Get(Expression<Func<PoolHistoryEntity, bool>> pred = null) => 
            pred switch
            {
                null => DbSet.OrderBy(x => x.Name).AsNoTracking().ToList(),
                _ => DbSet.Where(pred).OrderBy(x => x.Name).AsNoTracking().ToList()
            };

        public PoolHistoryEntity Read(int id) => Get(x => x.Id == id).SingleOrDefault();

        public PoolHistoryEntity Read(string name) => Get(x => x.Name == name).SingleOrDefault();
    }
}
