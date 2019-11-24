using System.Collections.Generic;

using LedgerLib.Entities;

namespace LedgerLib.Interfaces
{
    public interface IAllotmentDAL : IDAL<AllotmentEntity>
    {
        IEnumerable<AllotmentEntity> GetForPool(int pid);
        IEnumerable<AllotmentEntity> GetForCompany(int cid);
        AllotmentEntity Read(int id);
        bool PoolHasAllotments(int pid);
        bool CompanyHasAllotments(int cid);
    }
}
