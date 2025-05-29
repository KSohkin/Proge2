using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Models
{
    public class ClientIndexModel
    {
        public ClientSearch Search { get; set; }
        public PagedResult<Client> Data { get; set; }
    }
}
