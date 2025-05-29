using KooliProjekt.WpfApp.Api;

public interface IApiClient
{
    Task<Result<List<Client>>> List();
    Task<Result> Save(Client list);
    Task<Result> Delete(int id);
}
