using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Models
{
    public class PaymentIndexModel
    {
        public PaymentSearch Search { get; set; }
        public PagedResult<Payment> Data { get; set; }
    }
}
