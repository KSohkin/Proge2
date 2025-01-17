using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class EventsService : IEventsService
    {
        private readonly ApplicationDbContext _context;

        public EventsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Event>> List(int page, int pageSize)
        {
            return await _context.Events.GetPagedAsync(page, 5);
        }

        public async Task<Event> Get(int id)
        {
            return await _context.Events.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task Save(Event list)
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
            var todoList = await _context.Events.FindAsync(id);
            if (todoList != null)
            {
                _context.Events.Remove(todoList);
                await _context.SaveChangesAsync();
            }
        }
    }
}
