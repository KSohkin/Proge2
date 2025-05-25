using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using KooliProjekt.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
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
            // Act
            using var response = await _client.GetAsync("/Payments");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Details_should_return_notfound_when_payment_was_not_found()
        {
            // Act
            using var response = await _client.GetAsync("/Payments/Details/100");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_id_is_missing()
        {
            // Act
            using var response = await _client.GetAsync("/Payments/Details/");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_ok_when_payment_was_found()
        {
            // Arrange
            var payment = new Payment
            {
                Price = "100",
                Payment_nr = "PAY123",
                Deadline = DateTime.Now.AddDays(7),
                Description = "Test payment"
            };
            _context.Payments.Add(payment);
            _context.SaveChanges();

            // Act
            using var response = await _client.GetAsync("/Payments/Details/" + payment.Id);

            // Assert
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
            // Act
            using var response = await _client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Create_should_save_new_payment()
        {
            // Arrange
            var formValues = new Dictionary<string, string>
            {
                {"Price", "100"},
                {"Payment_nr", "PAY123"},
                {"Deadline", DateTime.Now.AddDays(7).ToString("yyyy-MM-ddTHH:mm:ss")},
                {"Description", "Test payment"}
            };

            // Remove any existing test data
            _context.Payments.RemoveRange(_context.Payments.ToList());
            await _context.SaveChangesAsync();

            using var content = new FormUrlEncodedContent(formValues);

            // Act
            using var response = await _client.PostAsync("/Payments/Create", content);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);

            // Verify the payment was saved
            var savedPayment = await _context.Payments.FirstOrDefaultAsync();
            Assert.NotNull(savedPayment);
            Assert.Equal("PAY123", savedPayment.Payment_nr);
        }

        [Fact]
        public async Task Create_should_not_save_invalid_new_payment()
        {
            // Arrange
            var formValues = new Dictionary<string, string>
            {
                {"Price", ""},
                {"Payment_nr", ""},
                {"Deadline", ""},
                {"Description", ""}
            };

            using var content = new FormUrlEncodedContent(formValues);

            // Act
            using var response = await _client.PostAsync("/Payments/Create", content);

            // Assert
            Assert.True(
                response.StatusCode == HttpStatusCode.OK ||
                response.StatusCode == HttpStatusCode.BadRequest
            );

            // Verify no payment was saved to the database
            Assert.Empty(_context.Payments);
        }
    }
}