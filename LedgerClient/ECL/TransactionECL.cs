using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using AutoMapper;

using LedgerClient.ECL.DTO;
using LedgerClient.ECL.Interfaces;
using LedgerClient.Infrastructure;

using LedgerLib.Entities;

namespace LedgerClient.ECL
{
    public class TransactionECL : ITransactionECL
    {
        private readonly IMapper _mapper;

        public TransactionECL(IMapper mapper) => _mapper = mapper;

        public int Count { get => Tools.Locator.TransactionDAL.Count; }

        public void Insert(Transaction dto)
        {
            TransactionEntity entity = _mapper.Map<TransactionEntity>(dto);
            Tools.Locator.TransactionDAL.Insert(entity);
            dto.Id = entity.Id;
            dto.RowVersion = entity.RowVersion.ArrayCopy();
        }

        public void Update(Transaction dto)
        {
            TransactionEntity entity = _mapper.Map<TransactionEntity>(dto);
            Tools.Locator.TransactionDAL.Update(entity);
            dto.RowVersion = entity.RowVersion.ArrayCopy();
        }

        public void Delete(Transaction dto)
        {
            TransactionEntity entity = _mapper.Map<TransactionEntity>(dto);
            Tools.Locator.TransactionDAL.Delete(entity);
        }

        public IEnumerable<Transaction> Get(Expression<Func<TransactionEntity, bool>> pred = null)
        {
            var entities = Tools.Locator.TransactionDAL.Get(pred);
            return _mapper.Map<List<Transaction>>(entities);
        }

        public IEnumerable<Transaction> GetForAccount(int aid) => Get(x => x.AccountId == aid);

        public Transaction Read(int id) => Get(x => x.Id == id).SingleOrDefault();

        public decimal Total() => Get().Sum(x => x.Payment);

        public decimal TotalForAccount(int aid) => Get(x => x.AccountId == aid).Sum(x => x.Payment);

        public bool AccountHasTransactions(int aid) => GetForAccount(aid).Any();
    }
}
