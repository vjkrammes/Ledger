using System.Collections.Generic;

using LedgerLib.Entities;

namespace LedgerLib.Interfaces
{
    public interface IIdentityDAL : IDAL<IdentityEntity>
    {
        IEnumerable<IdentityEntity> GetForCompany(int cid);
        IdentityEntity Read(int id);
        IdentityEntity Read(int cid, string url);
        bool CompanyHasIdentities(int cid);
    }
}
