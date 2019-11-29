using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using LedgerLib.Entities;
using LedgerLib.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace LedgerLib
{
    public class TransactionDAL : DAL<TransactionEntity, LedgerContext>, ITransactionDAL
    {
        public TransactionDAL(LedgerContext context) : base(context) { }

        public override IEnumerable<TransactionEntity> Get(Expression<Func<TransactionEntity, bool>> pred = null)
        {
            return pred switch
            {
                null => _dbset
                        .OrderByDescending(x => x.Date)
                        .AsNoTracking().ToList(),
                _ => _dbset
                        .Where(pred)
                        .OrderByDescending(x => x.Date)
                        .AsNoTracking().ToList()
            };
        }

        public IEnumerable<TransactionEntity> GetForAccount(int aid) => Get(x => x.AccountId == aid);

        public TransactionEntity Read(int id) => Get(x => x.Id == id).SingleOrDefault();

        public decimal Total() => _dbset.Sum(x => x.Payment);

        public decimal TotalForAccount(int aid) => _dbset.Where(x => x.AccountId == aid).Sum(x => x.Payment);

        public bool AccountHasTransactions(int aid) => GetForAccount(aid).Any();
    }
}
