namespace KooliProjekt.WpfApp.Api
{
    public interface IApiClient
    {
        Task<List<Client>> List();
        Task Save(Client list);
        Task Delete(int id);
    }
}