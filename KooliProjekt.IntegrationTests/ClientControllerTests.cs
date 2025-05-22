using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using KooliProjekt.Data;
using KooliProjekt.IntegrationTests.Helpers;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class ClientsControllerTests : TestBase
    {
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _context;

        public ClientsControllerTests()
        {
            _client = Factory.CreateClient();
            _context = (ApplicationDbContext)Factory.Services.GetService(typeof(ApplicationDbContext));
        }

        [Fact]
        public async Task Index_should_return_success_status_code()
        {
            // Arrange
            // Act
            using var response = await _client.GetAsync("/Clients");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Details_should_return_notfound_when_id_is_missing()
        {
            // Arrange
            // Act
            using var response = await _client.GetAsync("/Clients/Details/");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_client_not_found()
        {
            // Arrange
            // Act
            using var response = await _client.GetAsync("/Clients/Details/100");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_success_when_client_exists()
        {
            // Arrange
            var client = new Client { Name = "Test Client", Email = "test@example.com", Phonenumber = "123456789" };
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            // Act
            using var response = await _client.GetAsync($"/Clients/Details/{client.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Create_should_return_success_status_code()
        {
            // Arrange
            // Act
            using var response = await _client.GetAsync("/Clients/Create");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_id_is_missing()
        {
            // Arrange
            // Act
            using var response = await _client.GetAsync("/Clients/Edit/");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_client_not_found()
        {
            // Arrange
            // Act
            using var response = await _client.GetAsync("/Clients/Edit/100");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Edit_should_return_success_when_client_exists()
        {
            // Arrange
            var client = new Client { Name = "Test Client", Email = "test@example.com", Phonenumber = "123456789" };
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            // Act
            using var response = await _client.GetAsync($"/Clients/Edit/{client.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_should_return_notfound_when_id_is_missing()
        {
            // Arrange
            // Act
            using var response = await _client.GetAsync("/Clients/Delete/");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_should_return_notfound_when_client_not_found()
        {
            // Arrange
            // Act
            using var response = await _client.GetAsync("/Clients/Delete/100");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_should_return_success_when_client_exists()
        {
            // Arrange
            var client = new Client { Name = "Test Client", Email = "test@example.com", Phonenumber = "123456789" };
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            // Act
            using var response = await _client.GetAsync($"/Clients/Delete/{client.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}