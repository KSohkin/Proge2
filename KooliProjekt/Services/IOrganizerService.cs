using KooliProjekt.Data;

namespace KooliProjekt.Services
{
    public interface IOrganizerService
    {
        Task<PagedResult<Organizer>> List(int page, int pageSize);
        Task<Organizer> Get(int? id);
        Task Save(Organizer list);
        Task Delete(int id);
    }
}