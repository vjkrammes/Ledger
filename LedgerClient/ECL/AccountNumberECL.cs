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
    public class AccountNumberECL : IAccountNumberECL
    {
        private readonly IMapper _mapper;

        public AccountNumberECL(IMapper mapper) => _mapper = mapper;

        public int Count { get => Tools.Locator.AccountNumberDAL.Count; }

        public void Insert(AccountNumber dto)
        {
            AccountNumberEntity entity = _mapper.Map<AccountNumberEntity>(dto);
            Tools.Locator.AccountNumberDAL.Insert(entity);
            dto.Id = entity.Id;
        }

        public void Update(AccountNumber dto)
        {
            AccountNumberEntity entity = _mapper.Map<AccountNumberEntity>(dto);
            Tools.Locator.AccountNumberDAL.Update(entity);
            dto.RowVersion = entity.RowVersion.ArrayCopy();
        }

        public void Delete(AccountNumber dto)
        {
            AccountNumberEntity entity = _mapper.Map<AccountNumberEntity>(dto);
            Tools.Locator.AccountNumberDAL.Delete(entity);
        }

        public IEnumerable<AccountNumber> Get(Expression<Func<AccountNumberEntity, bool>> pred = null)
        {
            var entities = Tools.Locator.AccountNumberDAL.Get(pred);
            return _mapper.Map<List<AccountNumber>>(entities);
        }

        public IEnumerable<AccountNumber> GetForAccount(int aid) => Get(x => x.AccountId == aid);

        public AccountNumber Read(int id) => Get(x => x.Id == id).SingleOrDefault();

        public AccountNumber Current(int aid) => _mapper.Map<AccountNumber>(Tools.Locator.AccountNumberDAL.Current(aid));

        public bool AccountHasAccountNumbers(int aid) => GetForAccount(aid).Any();
    }
}
