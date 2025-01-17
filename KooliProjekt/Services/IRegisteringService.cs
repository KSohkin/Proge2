using KooliProjekt.Data;

namespace KooliProjekt.Services
{
    public interface IRegisteringService
    {
        Task<PagedResult<Registering>> List(int page, int pageSize);
        Task<Registering> Get(int id);
        Task Save(Registering list);
        Task Delete(int id);
    }
}