using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class RegisteringService : IRegisteringService
    {
        private readonly ApplicationDbContext _context;

        public RegisteringService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Registering>> List(int page, int pageSize)
        {
            return await _context.Registerings.GetPagedAsync(page, 5);
        }

        public async Task<Registering> Get(int id)
        {
            return await _context.Registerings.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task Save(Registering list)
        {
            if (list.Id == 0)
            {
                _context.Add(list);
            }
            else
            {
                _context.Update(list);
            }

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var Registering = await _context.Events.FindAsync(id);
            if (Registering != null)
            {
                _context.Events.Remove(Registering);
                await _context.SaveChangesAsync();
            }
        }
    }
}
