using System.Collections.Generic;

using LedgerClient.ECL.DTO;

using LedgerLib.Entities;

namespace LedgerClient.ECL.Interfaces
{
    public interface IIdentityECL : IECL<IdentityEntity, Identity>
    {
        IEnumerable<Identity> GetForCompany(int cid);
        Identity Read(int id);
        Identity Read(int cid, string url);
        bool CompanyHasIdentities(int cid);
    }
}
