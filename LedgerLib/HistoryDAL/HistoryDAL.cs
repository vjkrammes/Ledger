using LedgerLib.Interfaces;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace LedgerLib.HistoryDAL
{
    public class HistoryDAL<TEntity, TContext> : IHistoryDAL<TEntity> where TEntity : class, new() where TContext : DbContext
    {
        private readonly TContext _context;
        private readonly DbSet<TEntity> _dbset;

        protected TContext Context => _context;
        protected DbSet<TEntity> DbSet => _dbset;

        public HistoryDAL(TContext context)
        {
            _context = context;
            _dbset = null;
            var dbtype = typeof(DbSet<>).MakeGenericType(typeof(TEntity));
            foreach (var prop in typeof(TContext).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (prop.PropertyType == dbtype)
                {
                    _dbset = prop.GetValue(_context) as DbSet<TEntity>;
                    break;
                }
            }
            if (_dbset is null)
            {
                throw new MissingMemberException("DbSet not found");
            }
        }

        public virtual int Count => _dbset.Count();

        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> pred = null) => pred switch
        {
            null => _dbset.AsNoTracking().ToList(),
            _ => _dbset.OrderBy(pred).AsNoTracking().ToList()
        };
    }
}
