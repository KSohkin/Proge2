using System.Net.Http;
using System.Net.Http.Json;

namespace KooliProjekt.WpfApp.Api
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7136/api/");
        }

        public async Task<List<Client>> List()
        {
            var result = await _httpClient.GetFromJsonAsync<List<Client>>("Clients");

            return result;
        }

        public async Task Save(Client list)
        {
            if(list.Id == 0)
            {
                await _httpClient.PostAsJsonAsync("Clients", list);
            }
            else
            {
                await _httpClient.PutAsJsonAsync("Clients/" + list.Id, list);
            }
        }

        public async Task Delete(int id)
        {
            await _httpClient.DeleteAsync("Clients/" + id);
        }
    }
}