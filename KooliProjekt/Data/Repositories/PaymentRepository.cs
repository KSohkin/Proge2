using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Data.Repositories
{
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(ApplicationDbContext context) : base(context)
        {
        }

    }
}