using KooliProjekt.Data;

namespace KooliProjekt.Services
{
    public interface IEventsService
    {
        Task<PagedResult<Event>> List(int page, int pageSize);
        Task<Event> Get(int id);
        Task Save(Event list);
        Task Delete(int id);
    }
}