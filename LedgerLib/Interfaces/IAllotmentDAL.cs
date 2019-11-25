using System.Collections.Generic;

using LedgerLib.Entities;

namespace LedgerLib.Interfaces
{
    public interface IAllotmentDAL : IDAL<AllotmentEntity>
    {
        void DeleteAll(int pid);
        IEnumerable<AllotmentEntity> GetForPool(int pid);
        IEnumerable<AllotmentEntity> GetForCompany(int cid);
        AllotmentEntity Read(int id);
        bool PoolHasAllotments(int pid);
        bool CompanyHasAllotments(int cid);
    }
}
