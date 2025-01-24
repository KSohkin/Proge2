
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Data.Repositories
{
    public class CategoryRepository : BaseRepository<Client>, IClientRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

    }
}