
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Data.Repositories
{
    public class EventRepository : BaseRepository<Event>, IEventRepository
    {
        public EventRepository(ApplicationDbContext context) : base(context)
        {
        }

    }
}