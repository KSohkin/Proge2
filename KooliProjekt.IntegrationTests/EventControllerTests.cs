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
    public class EventControllerTests : TestBase
    {
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _context;

        public EventControllerTests()
        {
            _client = Factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
            _context = (ApplicationDbContext)Factory.Services.GetService(typeof(ApplicationDbContext));
        }

        [Fact]
        public async Task Index_should_return_correct_response()
        {
            using var response = await _client.GetAsync("/Events");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Details_should_return_notfound_when_list_was_not_found()
        {
            using var response = await _client.GetAsync("/Events/Details/100");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_id_is_missing()
        {
            using var response = await _client.GetAsync("/Events/Details/");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_ok_when_list_was_found()
        {
            var ev = new Event
            {
                Name = "Test Event",
                Date = System.DateTime.Now,
                Description = "Description",
                Seats = "100",
                Price = "20",
                Summary = "Summary",
                Organizer = "Organizer"
            };
            _context.Events.Add(ev);
            _context.SaveChanges();

            using var response = await _client.GetAsync("/Events/Details/" + ev.Id);
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData("/Events/Details")]
        [InlineData("/Events/Details/100")]
        [InlineData("/Events/Delete")]
        [InlineData("/Events/Delete/100")]
        [InlineData("/Events/Edit")]
        [InlineData("/Events/Edit/100")]
        public async Task Should_return_notfound(string url)
        {
            using var response = await _client.GetAsync(url);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Create_should_save_new_event()
        {
            var formValues = new Dictionary<string, string>
            {
                ["Name"] = "Test Event",
                ["Date"] = System.DateTime.Now.ToString("o"),
                ["Description"] = "Description",
                ["Seats"] = "100",
                ["Price"] = "20",
                ["Summary"] = "Summary",
                ["Organizer"] = "Organizer"
            };

            using var content = new FormUrlEncodedContent(formValues);
            using var response = await _client.PostAsync("/Events/Create", content);

            Assert.True(response.StatusCode == HttpStatusCode.Redirect || response.StatusCode == HttpStatusCode.MovedPermanently);

            var ev = _context.Events.FirstOrDefault();
            Assert.NotNull(ev);
            Assert.NotEqual(0, ev.Id);
            Assert.Equal("Test Event", ev.Name);
        }

        [Fact]
        public async Task Create_should_not_save_invalid_new_event()
        {
            var formValues = new Dictionary<string, string>
            {
                ["Name"] = "",
                ["Date"] = "",
                ["Description"] = "",
                ["Seats"] = "",
                ["Price"] = "",
                ["Summary"] = "",
                ["Organizer"] = ""
            };
            using var content = new FormUrlEncodedContent(formValues);
            using var response = await _client.PostAsync("/Events/Create", content);

            response.EnsureSuccessStatusCode();
            Assert.False(_context.Events.Any());
        }
    }
}
