using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _context;

        public PaymentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Payment>> List(int page, int pageSize)
        {
            return await _context.Payments.GetPagedAsync(page, 5);
        }

        public async Task<Payment> Get(int id)
        {
            return await _context.Payments.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task Save(Payment list)
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
            var Payment = await _context.Events.FindAsync(id);
            if (Payment != null)
            {
                _context.Events.Remove(Payment);
                await _context.SaveChangesAsync();
            }
        }
    }
}
