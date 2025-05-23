﻿using System;
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
    public class RegisteringControllerTests : TestBase
    {
        private readonly HttpClient _registering;
        private readonly ApplicationDbContext _context;

        public RegisteringControllerTests()
        {
            var options = new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            };
            _registering = Factory.CreateClient(options);
            _context = (ApplicationDbContext)Factory.Services.GetService(typeof(ApplicationDbContext));
        }

        [Fact]
        public async Task Index_should_return_correct_response()
        {
            using var response = await _registering.GetAsync("/Registerings");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Details_should_return_notfound_when_list_was_not_found()
        {
            using var response = await _registering.GetAsync("/Registerings/Details/100");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_id_is_missing()
        {
            using var response = await _registering.GetAsync("/Registerings/Details/");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_ok_when_list_was_found()
        {
            var list = new Registering
            {
                Date = DateTime.Now,
                Klient_Id = 22,
                Payment_Id = "22",
                Event_Id = 21
            };

            _context.Registerings.Add(list);
            await _context.SaveChangesAsync();

            using var response = await _registering.GetAsync("/Registerings/Details/" + list.Id);
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
            using var response = await _registering.GetAsync(url);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Create_should_save_new_list()
        {
            var formValues = new Dictionary<string, string>
            {
                { "Date", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") },
                { "Klient_Id", "12" },
                { "Payment_Id", "13" },
                { "Event_Id", "13" },
            };

            _context.Registerings.RemoveRange(_context.Registerings.ToList());
            await _context.SaveChangesAsync();

            using var content = new FormUrlEncodedContent(formValues);
            using var response = await _registering.PostAsync("/Registerings/Create", content);

            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);

            var savedRegistering = await _context.Registerings.FirstOrDefaultAsync();
            Assert.NotNull(savedRegistering);
            Assert.Equal("13", savedRegistering.Payment_Id);
        }

        [Fact]
        public async Task Create_should_not_save_invalid_new_list()
        {
            var formValues = new Dictionary<string, string>
            {
                { "Klient_Id", "" },
                { "Id", "" },
                { "Payment_Id", "" },
                { "Date", "" },
                { "Event_Id", "" },
            };

            using var content = new FormUrlEncodedContent(formValues);
            using var response = await _registering.PostAsync("/Registerings/Create", content);

            Assert.True(
                response.StatusCode == HttpStatusCode.OK ||
                response.StatusCode == HttpStatusCode.BadRequest
            );

            Assert.Empty(_context.Registerings);
        }
    }
}
