using LedgerLib.Interfaces;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace LedgerLib
{
    public class DAL<TEntity, TContext> : IDAL<TEntity> where TEntity : class, new() where TContext : DbContext
    {
        private readonly TContext _context;
        private readonly DbSet<TEntity> _dbSet;

        protected TContext Context => _context;
        protected DbSet<TEntity> DbSet => _dbSet;

        public DAL(TContext context)
        {
            _context = context;
            _dbSet = null;
            var dbtype = typeof(DbSet<>).MakeGenericType(typeof(TEntity));
            foreach (var prop in typeof(TContext).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (prop.PropertyType == dbtype)
                {
                    _dbSet = prop.GetValue(Context) as DbSet<TEntity>;
                    break;
                }
            }
            if (DbSet is null)
            {
                throw new MissingMemberException("DbSet not found");
            }
        }

        public virtual int Count => DbSet.Count();

        public virtual void Insert(TEntity entity)
        {
            DbSet.Add(entity);
            Context.SaveChanges();
        }

        public virtual void Update(TEntity entity)
        {
            Context.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public virtual void Delete(TEntity entity)
        {
            Context.Attach(entity);
            DbSet.Remove(entity);
            Context.SaveChanges();
        }

        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> pred = null) => pred switch
        {
            null => DbSet.AsNoTracking().ToList(),
            _ => DbSet.Where(pred).AsNoTracking().ToList()
        };
    }
}
