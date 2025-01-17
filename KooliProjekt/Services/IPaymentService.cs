using KooliProjekt.Data;

namespace KooliProjekt.Services
{
    public interface IPaymentService
    {
        Task<PagedResult<Payment>> List(int page, int pageSize);
        Task<Payment> Get(int id);
        Task Save(Payment list);
        Task Delete(int id);
    }
}