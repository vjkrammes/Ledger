using LedgerLib.HistoryEntities;
using LedgerLib.Interfaces;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LedgerLib.HistoryDAL
{
    public class PayeeHistoryDAL : HistoryDAL<PayeeHistoryEntity, HistoryContext>, IPayeeHistoryDAL
    {
        public PayeeHistoryDAL(HistoryContext context) : base(context) { }

        public override IEnumerable<PayeeHistoryEntity> Get(Expression<Func<PayeeHistoryEntity, bool>> pred = null) => 
            pred switch
            {
                null => DbSet.OrderBy(x => x.Name).AsNoTracking().ToList(),
                _ => DbSet.Where(pred).OrderBy(x => x.Name).AsNoTracking().ToList()
            };

        public PayeeHistoryEntity Read(int id) => Get(x => x.Id == id).SingleOrDefault();

        public PayeeHistoryEntity Read(string name) => Get(x => x.Name == name).SingleOrDefault();
    }
}
