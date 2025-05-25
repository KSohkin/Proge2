using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using KooliProjekt.Data;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class OrganizerControllerTests : TestBase
    {
        private readonly HttpClient _organizer;
        private readonly ApplicationDbContext _context;

        public OrganizerControllerTests()
        {
            var options = new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            };
            _organizer = Factory.CreateClient(options);
            _context = (ApplicationDbContext)Factory.Services.GetService(typeof(ApplicationDbContext));
        }

        [Fact]
        public async Task Index_should_return_correct_response()
        {
            // Act
            using var response = await _organizer.GetAsync("/Organizers");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Details_should_return_notfound_when_list_was_not_found()
        {
            using var response = await _organizer.GetAsync("/Organizers/Details/99999");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_id_is_missing()
        {
            using var response = await _organizer.GetAsync("/Organizers/Details/");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_ok_when_list_was_found()
        {
            // Arrange
            var list = new Organizer { Name = "Test Name", Description = "Test Desc" };
            _context.Organizers.Add(list);
            await _context.SaveChangesAsync();

            // Act
            using var response = await _organizer.GetAsync($"/Organizers/Details/{list.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData("/Organizers/Details")]
        [InlineData("/Organizers/Details/99999")]
        [InlineData("/Organizers/Delete")]
        [InlineData("/Organizers/Delete/99999")]
        [InlineData("/Organizers/Edit")]
        [InlineData("/Organizers/Edit/99999")]
        public async Task Should_return_notfound(string url)
        {
            using var response = await _organizer.GetAsync(url);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Create_should_save_new_list()
        {
            // Arrange
            var formValues = new Dictionary<string, string>
            {
                { "Name", "Test Organizer" },
                { "Description", "This is a test" }
            };

            _context.Organizers.RemoveRange(_context.Organizers.ToList());
            await _context.SaveChangesAsync();

            using var content = new FormUrlEncodedContent(formValues);

            // Act
            using var response = await _organizer.PostAsync("/Organizers/Create", content);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);

            var savedOrganizer = await _context.Organizers.FirstOrDefaultAsync();
            Assert.NotNull(savedOrganizer);
            Assert.Equal("Test Organizer", savedOrganizer.Name);
        }

        [Fact]
        public async Task Create_should_not_save_invalid_new_list()
        {
            // Arrange
            var formValues = new Dictionary<string, string>
            {
                { "Name", "" },
                { "Description", "" }
            };

            using var content = new FormUrlEncodedContent(formValues);

            // Act
            using var response = await _organizer.PostAsync("/Organizers/Create", content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode); // Invalid data returns to view

            Assert.Empty(_context.Organizers);
        }
    }
}
