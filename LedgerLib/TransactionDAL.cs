using LedgerLib.Entities;
using LedgerLib.Interfaces;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LedgerLib
{
    public class TransactionDAL : DAL<TransactionEntity, LedgerContext>, ITransactionDAL
    {
        public TransactionDAL(LedgerContext context) : base(context) { }

        public override IEnumerable<TransactionEntity> Get(Expression<Func<TransactionEntity, bool>> pred = null) => 
            pred switch
            {
                null => DbSet
                        .OrderByDescending(x => x.Date)
                        .AsNoTracking().ToList(),
                _ => DbSet
                        .Where(pred)
                        .OrderByDescending(x => x.Date)
                        .AsNoTracking().ToList()
            };

        public IEnumerable<TransactionEntity> GetForAccount(int aid) => Get(x => x.AccountId == aid);

        public TransactionEntity Read(int id) => Get(x => x.Id == id).SingleOrDefault();

        public decimal Total() => DbSet.Sum(x => x.Payment);

        public decimal TotalForAccount(int aid) => DbSet.Where(x => x.AccountId == aid).Sum(x => x.Payment);

        public bool AccountHasTransactions(int aid) => GetForAccount(aid).Any();
    }
}
