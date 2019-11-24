using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using LedgerLib.Entities;
using LedgerLib.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace LedgerLib
{
    public class PoolDAL : DAL<PoolEntity, LedgerContext>, IPoolDAL
    {
        public PoolDAL(LedgerContext context) : base(context) { }

        public override IEnumerable<PoolEntity> Get(Expression<Func<PoolEntity, bool>> pred = null)
        {
            return pred switch
            {
                null => _dbset
                        .OrderBy(x => x.Name)
                        .Select(x => x).AsNoTracking().ToList(),
                _ => _dbset
                        .Where(pred)
                        .OrderBy(x => x.Name)
                        .Select(x => x).AsNoTracking().ToList()
            };
        }

        public PoolEntity Read(int id) => Get(x => x.Id == id).SingleOrDefault();

        public PoolEntity Read(string name) => Get(x => x.Name == name).SingleOrDefault();
    }
}
