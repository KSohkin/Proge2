using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Models
{
    public class OrganizerIndexModel
    {
        public OrganizerSearch Search { get; set; }
        public PagedResult<Organizer> Data { get; set; }
    }
}
