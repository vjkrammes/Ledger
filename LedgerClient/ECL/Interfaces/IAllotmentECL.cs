using System.Collections.Generic;

using LedgerClient.ECL.DTO;

using LedgerLib.Entities;

namespace LedgerClient.ECL.Interfaces
{
    public interface IAllotmentECL : IECL<AllotmentEntity, Allotment>
    {
        void DeleteAll(int pid);
        IEnumerable<Allotment> GetForPool(int pid);
        IEnumerable<Allotment> GetForCompany(int cid);
        Allotment Read(int id);
        bool PoolHasAllotments(int pid);
        bool CompanyHasAllotments(int cid);
    }
}
