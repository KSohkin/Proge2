using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class OrganizerService : IOrganizerService
    {
        private readonly ApplicationDbContext _context;

        public OrganizerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Organizer>> List(int page, int pageSize)
        {
            return await _context.Organizers.GetPagedAsync(page, 5);
        }

        public async Task<Organizer> Get(int id)
        {
            return await _context.Organizers.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task Save(Organizer list)
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
            var Organizer = await _context.Events.FindAsync(id);
            if (Organizer != null)
            {
                _context.Events.Remove(Organizer);
                await _context.SaveChangesAsync();
            }
        }
    }
}
