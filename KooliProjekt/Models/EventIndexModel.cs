using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Models
{
    public class EventIndexModel
    {
        public EventSearch Search { get; set; }
        public PagedResult<Event> Data { get; set; }
    }
}
