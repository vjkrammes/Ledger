using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using LedgerLib.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace LedgerLib
{
    public class DAL<TEntity, TContext> : IDAL<TEntity> where TEntity : class, new() where TContext : DbContext
    {
        protected readonly TContext _context;
        protected readonly DbSet<TEntity> _dbset;

        public DAL(TContext context)
        {
            _context = context;
            _dbset = null;
            Type dbtype = typeof(DbSet<>).MakeGenericType(typeof(TEntity));
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

        public virtual int Count { get => _dbset.Count(); }

        public virtual void Insert(TEntity entity)
        {
            _dbset.Add(entity);
            _context.SaveChanges();
        }

        public virtual void Update(TEntity entity)
        {
            _context.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public virtual void Delete(TEntity entity)
        {
            _context.Attach(entity);
            _dbset.Remove(entity);
            _context.SaveChanges();
        }

        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> pred = null)
        {
            return pred switch
            {
                null => _dbset.AsNoTracking().ToList(),
                _ => _dbset.Where(pred).AsNoTracking().ToList()
            };
        }
    }
}
