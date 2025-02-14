using KooliProjekt.Data;
using KooliProjekt.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _uof;

        public PaymentService(IUnitOfWork uof)
        {
            _uof = uof;
        }

        public async Task Delete(int Id)
        {
            await _uof.PaymentRepository.Delete(Id);
        }

        public async Task<Payment> Get(int? Id)
        {
            return await _uof.PaymentRepository.Get((int)Id);
        }

        public async Task<PagedResult<Payment>> List(int page, int pageSize)
        {
            return await _uof.PaymentRepository.List(page, pageSize);
        }

        public async Task Save(Payment payment)
        {
            await _uof.PaymentRepository.Save(payment);
        }
    }
}