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
    public class AccountTypeECL : IAccountTypeECL
    {
        private readonly IMapper _mapper;

        public AccountTypeECL(IMapper mapper) => _mapper = mapper;

        public int Count { get => Tools.Locator.AccountTypeDAL.Count; }

        public void Insert(AccountType dto)
        {
            AccountTypeEntity entity = _mapper.Map<AccountTypeEntity>(dto);
            Tools.Locator.AccountTypeDAL.Insert(entity);
            dto.Id = entity.Id;
        }

        public void Update(AccountType dto)
        {
            AccountTypeEntity entity = _mapper.Map<AccountTypeEntity>(dto);
            Tools.Locator.AccountTypeDAL.Update(entity);
        }

        public void Delete(AccountType dto)
        {
            AccountTypeEntity entity = _mapper.Map<AccountTypeEntity>(dto);
            Tools.Locator.AccountTypeDAL.Delete(entity);
        }

        public IEnumerable<AccountType> Get(Expression<Func<AccountTypeEntity, bool>> pred = null)
        {
            var entities = Tools.Locator.AccountTypeDAL.Get(pred);
            return _mapper.Map<List<AccountType>>(entities);
        }

        public AccountType Read(int id) => Get(x => x.Id == id).SingleOrDefault();

        public AccountType Read(string description) => Get(x => x.Description == description).SingleOrDefault();
    }
}
