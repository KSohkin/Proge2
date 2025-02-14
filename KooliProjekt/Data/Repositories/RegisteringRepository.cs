using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Data.Repositories
{
    public class RegisteringRepository : BaseRepository<Registering>, IRegisteringRepository
    {
        public RegisteringRepository(ApplicationDbContext context) : base(context)
        {
        }

    }
}