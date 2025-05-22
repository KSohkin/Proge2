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
    public class PaymentControllerTests : TestBase
    {
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _context;

        public PaymentControllerTests()
        {
            _client = Factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
            _context = (ApplicationDbContext)Factory.Services.GetService(typeof(ApplicationDbContext));
        }

        [Fact]
        public async Task Index_should_return_correct_response()
        {
            using var response = await _client.GetAsync("/Payments");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Details_should_return_notfound_when_list_was_not_found()
        {
            using var response = await _client.GetAsync("/Payments/Details/100");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_id_is_missing()
        {
            using var response = await _client.GetAsync("/Payments/Details/");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_ok_when_list_was_found()
        {
            var payment = new Payment
            {
                Price = "50",
                Payment_nr = "P-123",
                Deadline = DateTime.Now.AddDays(5),
                Description = "Test Payment"
            };
            _context.Payments.Add(payment);
            _context.SaveChanges();

            using var response = await _client.GetAsync("/Payments/Details/" + payment.Id);
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData("/Payments/Details")]
        [InlineData("/Payments/Details/100")]
        [InlineData("/Payments/Delete")]
        [InlineData("/Payments/Delete/100")]
        [InlineData("/Payments/Edit")]
        [InlineData("/Payments/Edit/100")]
        public async Task Should_return_notfound(string url)
        {
            using var response = await _client.GetAsync(url);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Create_should_save_new_payment()
        {
            var formValues = new Dictionary<string, string>
            {
                ["Price"] = "50",
                ["Payment_nr"] = "P-123",
                ["Deadline"] = DateTime.Now.AddDays(5).ToString("o"),
                ["Description"] = "Test Payment"
            };
            using var content = new FormUrlEncodedContent(formValues);
            using var response = await _client.PostAsync("/Payments/Create", content);

            Assert.True(response.StatusCode == HttpStatusCode.Redirect || response.StatusCode == HttpStatusCode.MovedPermanently);

            var payment = _context.Payments.FirstOrDefault();
            Assert.NotNull(payment);
            Assert.NotEqual(0, payment.Id);
            Assert.Equal("50", payment.Price);
        }

        [Fact]
        public async Task Create_should_not_save_invalid_new_payment()
        {
            var formValues = new Dictionary<string, string>
            {
                ["Price"] = "",
                ["Payment_nr"] = "",
                ["Deadline"] = "",
                ["Description"] = ""
            };
            using var content = new FormUrlEncodedContent(formValues);
            using var response = await _client.PostAsync("/Payments/Create", content);

            response.EnsureSuccessStatusCode();
            Assert.False(_context.Payments.Any());
        }
    }
}
