using System;
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
    public class RegisteringControllerTests : TestBase
    {
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _context;

        public RegisteringControllerTests()
        {
            _client = Factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
            _context = (ApplicationDbContext)Factory.Services.GetService(typeof(ApplicationDbContext));
        }

        [Fact]
        public async Task Index_should_return_correct_response()
        {
            using var response = await _client.GetAsync("/Registerings");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Details_should_return_notfound_when_list_was_not_found()
        {
            using var response = await _client.GetAsync("/Registerings/Details/100");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_id_is_missing()
        {
            using var response = await _client.GetAsync("/Registerings/Details/");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_ok_when_list_was_found()
        {
            var reg = new Registering
            {
                Klient_Id = 1,
                Payment_Id = "P-123",
                Date = DateTime.Now,
                Event_Id = 1
            };
            _context.Registerings.Add(reg);
            _context.SaveChanges();

            using var response = await _client.GetAsync("/Registerings/Details/" + reg.Id);
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData("/Registerings/Details")]
        [InlineData("/Registerings/Details/100")]
        [InlineData("/Registerings/Delete")]
        [InlineData("/Registerings/Delete/100")]
        [InlineData("/Registerings/Edit")]
        [InlineData("/Registerings/Edit/100")]
        public async Task Should_return_notfound(string url)
        {
            using var response = await _client.GetAsync(url);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Create_should_save_new_registering()
        {
            var formValues = new Dictionary<string, string>
            {
                ["Klient_Id"] = "1",
                ["Payment_Id"] = "P-123",
                ["Date"] = DateTime.Now.ToString("o"),
                ["Event_Id"] = "1"
            };
            using var content = new FormUrlEncodedContent(formValues);
            using var response = await _client.PostAsync("/Registerings/Create", content);

            Assert.True(response.StatusCode == HttpStatusCode.Redirect || response.StatusCode == HttpStatusCode.MovedPermanently);

            var reg = _context.Registerings.FirstOrDefault();
            Assert.NotNull(reg);
            Assert.NotEqual(0, reg.Id);
            Assert.Equal(1, reg.Klient_Id);
        }

        [Fact]
        public async Task Create_should_not_save_invalid_new_registering()
        {
            var formValues = new Dictionary<string, string>
            {
                ["Klient_Id"] = "",
                ["Payment_Id"] = "",
                ["Date"] = "",
                ["Event_Id"] = ""
            };
            using var content = new FormUrlEncodedContent(formValues);
            using var response = await _client.PostAsync("/Registerings/Create", content);

            response.EnsureSuccessStatusCode();
            Assert.False(_context.Registerings.Any());
        }
    }
}
