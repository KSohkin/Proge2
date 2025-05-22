using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class PaymentsServiceTests : ServiceTestBase
    {
        [Fact]
        public async Task Save_should_add_new_list()
        {
            // Arrange
            var service = new PaymentService(DbContext);
            var payment = new Payment
            {
                Id = 0, // Use 0 to simulate new entity
                Price = "12.99$",
                Payment_nr = "123",
                Deadline = DateTime.Today,
                Description = "What"
            };

            // Act
            await service.Save(payment);

            // Assert
            var count = DbContext.Payments.Count();
            var result = DbContext.Payments.FirstOrDefault();
            Assert.Equal(1, count);
            Assert.Equal("123", result.Payment_nr);
        }

        [Fact]
        public async Task Save_should_update_existing_payment()
        {
            // Arrange
            var service = new PaymentService(DbContext);
            var payment = new Payment
            {
                Id = 0, // Use 0 to simulate new entity
                Price = "12.99$",
                Payment_nr = "123",
                Deadline = DateTime.Today,
                Description = "What"
            };

            DbContext.Payments.Add(payment);
            await DbContext.SaveChangesAsync();

            // Act
            payment.Payment_nr = "Test2";
            await service.Save(payment);

            // Assert
            var newPayment = await DbContext.Payments.FindAsync(payment.Id);
            Assert.NotNull(newPayment);
            Assert.Equal("Test2", newPayment.Payment_nr);
        }

        [Fact]
        public async Task Get_should_return_correct_panel()
        {
            // Arrange
            var service = new PaymentService(DbContext);
            var payment = new Payment
            {
                Id = 0, // Use 0 to simulate new entity
                Price = "12.99$",
                Payment_nr = "123",
                Deadline = DateTime.Today,
                Description = "What"
            };

            DbContext.Payments.Add(payment);
            await DbContext.SaveChangesAsync();

            // Act
            var getPayment = await service.Get(payment.Id);

            // Assert
            Assert.NotNull(getPayment);
            Assert.Equal(payment.Id, getPayment.Id);
        }

        [Fact]
        public async Task Delete_should_remove_given_list()
        {
            // Arrange
            var service = new PaymentService(DbContext);
            var payment = new Payment
            {
                Id = 0, // Use 0 to simulate new entity
                Price = "12.99",
                Payment_nr = "123",
                Deadline = DateTime.Today,
                Description = "What"
            };

            DbContext.Payments.Add(payment);
            await DbContext.SaveChangesAsync();

            // Act
            await service.Delete(payment.Id);

            // Assert
            var count = DbContext.Payments.Count();
            Assert.Equal(0, count);
        }
        [Fact]
        public async Task List_should_return_correct_paged_result()
        {
            // Arrange
            var service = new PaymentService(DbContext);

            // Add 12 dummy records
            for (int i = 1; i <= 12; i++)
            {
                DbContext.Payments.Add(new Payment
                {
                    Deadline = DateTime.Today,
                    Payment_nr = $"Payment{i}",
                    Price = $"Price{i}",
                    Description = $"Description{i}"
                });
            }

            await DbContext.SaveChangesAsync();

            // Act
            var result = await service.List(page: 2, pageSize: 5);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Results.Count);        // Page size
            Assert.Equal(12, result.RowCount);            // Total items in DB
            Assert.Equal(3, result.PageCount);            // 12 items / 5 per page = 3 pages
            Assert.Equal(2, result.CurrentPage);          // We requested page 2
            Assert.Equal(5, result.PageSize);             // Requested size
        }
    }
}
