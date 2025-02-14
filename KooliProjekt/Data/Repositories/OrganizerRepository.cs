using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Data.Repositories
{
    public class OrganizerRepository : BaseRepository<Organizer>, IOrganizerRepository
    {
        public OrganizerRepository(ApplicationDbContext context) : base(context)
        {
        }

    }
}