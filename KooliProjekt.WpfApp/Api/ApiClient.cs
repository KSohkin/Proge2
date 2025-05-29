using KooliProjekt.WpfApp.Api;
using System.Net.Http;
using System.Net.Http.Json;

public class ApiClient : IApiClient
{
    private readonly HttpClient _httpClient;

    public ApiClient()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://localhost:7136/api/");
    }

    public async Task<Result<List<Client>>> List()
    {
        var result = new Result<List<Client>>();
        try
        {
            result.Value = await _httpClient.GetFromJsonAsync<List<Client>>("Clients");
        }
        catch (Exception ex)
        {
            result.Error = ex.Message;
        }
        return result;
    }

    public async Task<Result> Save(Client list)
    {
        var result = new Result();
        try
        {
            if (list.Id == 0)
            {
                await _httpClient.PostAsJsonAsync("Clients", list);
            }
            else
            {
                await _httpClient.PutAsJsonAsync("Clients/" + list.Id, list);
            }
        }
        catch (Exception ex)
        {
            result.Error = ex.Message;
        }
        return result;
    }

    public async Task<Result> Delete(int id)
    {
        var result = new Result();
        try
        {
            await _httpClient.DeleteAsync("Clients/" + id);
        }
        catch (Exception ex)
        {
            result.Error = ex.Message;
        }
        return result;
    }
}
