using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class ClientsService : IClientService
    {
        private readonly ApplicationDbContext _context;

        public ClientsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Client>> List(int page, int pageSize)
        {
            return await _context.Clients.GetPagedAsync(page, 5);
        }

        public async Task<Client> Get(int? id)
        {
            return await _context.Clients.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task Save(Client list)
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
            var client = await _context.Events.FindAsync(id);
            if (client != null)
            {
                _context.Events.Remove(client);
                await _context.SaveChangesAsync();
            }
        }
    }
}
