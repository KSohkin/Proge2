using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using KooliProjekt.Data;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
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

            var options = new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            };
            _client = Factory.CreateClient(options);
            _context = (ApplicationDbContext)Factory.Services.GetService(typeof(ApplicationDbContext));
        }

        [Fact]
        public async Task Index_should_return_correct_response()
        {
            // Arrange

            // Act
            using var response = await _client.GetAsync("/Clients");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Details_should_return_notfound_when_list_was_not_found()
        {
            // Arrange

            // Act
            using var response = await _client.GetAsync("/Clients/Details/100");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
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
        public async Task Details_should_return_ok_when_list_was_found()
        {
            // Arrange
            var list = new Client { Name = "Jessica Beans",Email = "ggg@Gmail.com", Phonenumber = "69696969" };
            _context.Clients.Add(list);
            _context.SaveChanges();

            // Act
            using var response = await _client.GetAsync("/Clients/Details/" + list.Id);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData("/Clients/Details")]
        [InlineData("/Clients/Details/100")]
        [InlineData("/Clients/Delete")]
        [InlineData("/Clients/Delete/100")]
        [InlineData("/Clients/Edit")]
        [InlineData("/Clients/Edit/100")]
        public async Task Should_return_notfound(string url)
        {
            // Arrange

            // Act
            using var response = await _client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }



        [Fact]
        public async Task Create_should_save_new_list()
        {
            // Arrange
            var formValues = new Dictionary<string, string>();
            formValues.Add("Name", "Walter White");
            formValues.Add("Email", "walter@example.com");  // Added required field
            formValues.Add("Phonenumber", "99999");  // Fixed property name (matches model)

            using var content = new FormUrlEncodedContent(formValues);

            // Act
            using var response = await _client.PostAsync("/Clients/Create", content);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);  // More specific assertion

            var client = _context.Clients.FirstOrDefault();
            Assert.NotNull(client);
            Assert.NotEqual(0, client.Id);
            Assert.Equal("Walter White", client.Name);
            Assert.Equal("walter@example.com", client.Email);  // Verify all fields
        }

        [Fact]
        public async Task Create_should_not_save_invalid_new_list()
        {
            // Arrange
            var formValues = new Dictionary<string, string>();
            formValues.Add("Id", "");
            formValues.Add("Name", "");       // Required field (empty)
            formValues.Add("Email", "");     // Required field (empty)
            formValues.Add("Phonenumber", ""); // Required field (empty, note the exact property name)

            using var content = new FormUrlEncodedContent(formValues);

            // Act
            using var response = await _client.PostAsync("/Clients/Create", content);

            // Assert
            // Should return a 200 (OK) or 400 (BadRequest) for invalid data, not a redirect
            Assert.True(
                response.StatusCode == HttpStatusCode.OK ||
                response.StatusCode == HttpStatusCode.BadRequest
            );

            // Verify no client was saved to the database
            Assert.Empty(_context.Clients);
        }
    }
}