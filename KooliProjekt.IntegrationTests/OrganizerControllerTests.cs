using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using KooliProjekt.Data;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class OrganizerControllerTests : TestBase
    {
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _context;

        public OrganizerControllerTests()
        {
            _client = Factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
            _context = (ApplicationDbContext)Factory.Services.GetService(typeof(ApplicationDbContext));
        }

        [Fact]
        public async Task Index_should_return_correct_response()
        {
            using var response = await _client.GetAsync("/Organizers");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Details_should_return_notfound_when_list_was_not_found()
        {
            using var response = await _client.GetAsync("/Organizers/Details/100");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_id_is_missing()
        {
            using var response = await _client.GetAsync("/Organizers/Details/");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_ok_when_list_was_found()
        {
            var org = new Organizer { Name = "Le organizer", Description = "Description" };
            _context.Organizers.Add(org);
            _context.SaveChanges();

            using var response = await _client.GetAsync("/Organizers/Details/" + org.Id);
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData("/Organizers/Details")]
        [InlineData("/Organizers/Details/100")]
        [InlineData("/Organizers/Delete")]
        [InlineData("/Organizers/Delete/100")]
        [InlineData("/Organizers/Edit")]
        [InlineData("/Organizers/Edit/100")]
        public async Task Should_return_notfound(string url)
        {
            using var response = await _client.GetAsync(url);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Create_should_save_new_organizer()
        {
            var formValues = new Dictionary<string, string>
            {
                ["Name"] = "Test Organizer",
                ["Description"] = "Description"
            };
            using var content = new FormUrlEncodedContent(formValues);
            using var response = await _client.PostAsync("/Organizers/Create", content);

            Assert.True(response.StatusCode == HttpStatusCode.Redirect || response.StatusCode == HttpStatusCode.MovedPermanently);

            var org = _context.Organizers.FirstOrDefault();
            Assert.NotNull(org);
            Assert.NotEqual(0, org.Id);
            Assert.Equal("Test Organizer", org.Name);
        }

        [Fact]
        public async Task Create_should_not_save_invalid_new_organizer()
        {
            var formValues = new Dictionary<string, string>
            {
                ["Name"] = "",
                ["Description"] = ""
            };
            using var content = new FormUrlEncodedContent(formValues);
            using var response = await _client.PostAsync("/Organizers/Create", content);

            response.EnsureSuccessStatusCode();
            Assert.False(_context.Organizers.Any());
        }
    }
}
