namespace KooliProjekt.Data.Repositories
{
    public interface IClientRepository : IBaseRepository<Client>
    {
        Task<Client> Get(int id);
        Task Save(Client client);
        Task Delete(int id);
        Task Delete(Client client);
        Task<PagedResult<Client>> List(int page, int pageSize);
    }
}