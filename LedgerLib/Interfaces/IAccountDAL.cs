﻿using System.Collections.Generic;

using LedgerLib.Entities;

namespace LedgerLib.Interfaces
{
    public interface IAccountDAL : IDAL<AccountEntity>
    {
        AccountEntity Create(AccountEntity account, byte[] salt, string number);
        IEnumerable<AccountEntity> GetForCompany(int cid);
        IEnumerable<AccountEntity> GetForAccountType(int atid);
        AccountEntity Read(int id);
        bool CompanyHasAccounts(int cid);
        bool AccountTypeHasAccounts(int atid);
    }
}
