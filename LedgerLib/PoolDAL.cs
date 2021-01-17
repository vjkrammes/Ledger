using LedgerLib.Entities;
using LedgerLib.Interfaces;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LedgerLib
{
    public class PoolDAL : DAL<PoolEntity, LedgerContext>, IPoolDAL
    {
        public PoolDAL(LedgerContext context) : base(context) { }

        public override IEnumerable<PoolEntity> Get(Expression<Func<PoolEntity, bool>> pred = null) => 
            pred switch
            {
                null => DbSet
                        .OrderBy(x => x.Name)
                        .AsNoTracking().ToList(),
                _ => DbSet
                        .Where(pred)
                        .OrderBy(x => x.Name)
                        .AsNoTracking().ToList()
            };

        public PoolEntity Read(int id) => Get(x => x.Id == id).SingleOrDefault();

        public PoolEntity Read(string name) => Get(x => x.Name == name).SingleOrDefault();
    }
}
