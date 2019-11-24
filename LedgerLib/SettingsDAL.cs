using LedgerLib.Entities;
using LedgerLib.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace LedgerLib
{
    public class SettingsDAL : ISettingsDAL
    {

        private readonly LedgerContext _context;

        public SettingsDAL(LedgerContext context) => _context = context;

        public void Update(SettingsEntity entity)
        {
            _context.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
