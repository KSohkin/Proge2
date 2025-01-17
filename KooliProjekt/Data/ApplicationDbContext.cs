using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Organizer> Organizers { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Registering> Registerings { get; set; }

    }
}