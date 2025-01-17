using KooliProjekt.Data;

namespace KooliProjekt.Services
{
    public interface IClientService
    {
        Task<PagedResult<Client>> List(int page, int pageSize);
        Task<Client> Get(int id);
        Task Save(Client list);
        Task Delete(int id);
    }
}