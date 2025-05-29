using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Models
{
    public class RegisteringIndexModel
    {
        public RegisteringsSearch Search { get; set; }
        public PagedResult<Registering> Data { get; set; }
    }
}
