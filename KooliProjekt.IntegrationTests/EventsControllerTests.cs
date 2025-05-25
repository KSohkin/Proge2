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
using Microsoft.EntityFrameworkCore;
using Mono.TextTemplating;
using Xunit;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class EventsControllerTests : TestBase
    {
        private readonly HttpClient _events;
        private readonly ApplicationDbContext _context;

        public EventsControllerTests()
        {

            var options = new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            };
            _events = Factory.CreateClient(options);
            _context = (ApplicationDbContext)Factory.Services.GetService(typeof(ApplicationDbContext));
        }

        [Fact]
        public async Task Index_should_return_correct_response()
        {
            // Arrange

            // Act
            using var response = await _events.GetAsync("/Events");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Details_should_return_notfound_when_list_was_not_found()
        {
            // Arrange

            // Act
            using var response = await _events.GetAsync("/Events/Details/100");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_id_is_missing()
        {
            // Arrange

            // Act
            using var response = await _events.GetAsync("/Events/Details/");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_ok_when_list_was_found()
        {
            // Arrange
            var list = new Event { Name = "Googly", Price = "20", Date = DateTime.Now, Description = "idk", Organizer = "idk", Seats = "2",  Summary = "idk" };
            _context.Events.Add(list);
            _context.SaveChanges();

            // Act
            using var response = await _events.GetAsync("/Events/Details/" + list.Id);

            // Assert
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
            // Arrange

            // Act
            using var response = await _events.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }



        [Fact]
        public async Task Create_should_save_new_list()
        {
            // Arrange
            var formValues = new Dictionary<string, string>
    {
        {"Name", "Googly"},
        {"Price", "20"},
        {"Date", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")}, // Proper format
        {"Description", "idk"},
        {"Organizer", "idk"},
        {"Seats", "2"},
        {"Summary", "idk"}
    };

            // Remove any existing test data
            _context.Events.RemoveRange(_context.Events.ToList());
            await _context.SaveChangesAsync();

            using var content = new FormUrlEncodedContent(formValues);

            // Act
            using var response = await _events.PostAsync("/Events/Create", content);

            // Assert
            // Check for redirect first
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);

            // Verify the event was saved
            var savedEvent = await _context.Events.FirstOrDefaultAsync();
            Assert.NotNull(savedEvent);
            Assert.Equal("Googly", savedEvent.Name);
        }

        [Fact]
        public async Task Create_should_not_save_invalid_new_list()
        {
            // Arrange
            var formValues = new Dictionary<string, string>();
            formValues.Add("Name", "");
            formValues.Add("Id", "");
            formValues.Add("Price", "");
            formValues.Add("Date", "");
            formValues.Add("Description", "");
            formValues.Add("Organizer", "");
            formValues.Add("Seats", "");
            formValues.Add("Summary", "");

            using var content = new FormUrlEncodedContent(formValues);

            // Act
            using var response = await _events.PostAsync("/Events/Create", content);

            // Assert
            // Should return a 200 (OK) or 400 (BadRequest) for invalid data, not a redirect
            Assert.True(
                response.StatusCode == HttpStatusCode.OK ||
                response.StatusCode == HttpStatusCode.BadRequest
            );

            // Verify no client was saved to the database
            Assert.Empty(_context.Events);
        }
    }
}