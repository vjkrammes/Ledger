using AutoMapper;

using LedgerClient.ECL.DTO;
using LedgerClient.ECL.Interfaces;
using LedgerClient.Infrastructure;

using LedgerLib.Entities;
using LedgerLib.Infrastructure;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LedgerClient.ECL
{
    public class AccountECL : IAccountECL
    {
        private readonly IMapper _mapper;

        public AccountECL(IMapper mapper) => _mapper = mapper;

        public int Count => Tools.Locator.AccountDAL.Count;

        public void Insert(Account dto)
        {
            var entity = _mapper.Map<AccountEntity>(dto);
            Tools.Locator.AccountDAL.Insert(entity);
            dto.Id = entity.Id;
            dto.RowVersion = entity.RowVersion.ArrayCopy();
        }

        public void Update(Account dto)
        {
            var entity = _mapper.Map<AccountEntity>(dto);
            Tools.Locator.AccountDAL.Update(entity);
            dto.RowVersion = entity.RowVersion.ArrayCopy();
        }

        public void Delete(Account dto)
        {
            var entity = _mapper.Map<AccountEntity>(dto);
            Tools.Locator.AccountDAL.Delete(entity);
        }

        public IEnumerable<Account> Get(Expression<Func<AccountEntity, bool>> pred = null)
        {
            var entities = Tools.Locator.AccountDAL.Get(pred);
            return _mapper.Map<List<Account>>(entities);
        }

        public IEnumerable<Account> GetForCompany(int cid) => Get(x => x.CompanyId == cid);

        public IEnumerable<Account> GetForAccountType(int atid) => Get(x => x.AccountTypeId == atid);

        public Account Read(int id) => Get(x => x.Id == id).SingleOrDefault();

        public bool CompanyHasAccounts(int cid) => GetForCompany(cid).Any();

        public bool AccountTypeHasAccounts(int atid) => GetForAccountType(atid).Any();

        public Account Create(Account account, string number)
        {
            var salt = Tools.GenerateSalt(Constants.SaltLength);
            var num = Tools.Locator.StringCypher.Encrypt(number, Tools.Locator.PasswordManager.Get(Constants.LedgerPassword), salt);
            var accountentity = _mapper.Map<AccountEntity>(account);
            accountentity = Tools.Locator.AccountDAL.Create(accountentity, salt, num);
            return _mapper.Map<Account>(accountentity);
        }
    }
}
