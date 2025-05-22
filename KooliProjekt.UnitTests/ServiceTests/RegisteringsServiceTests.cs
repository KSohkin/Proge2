using System;
using System.Linq;
using System.Threading.Tasks;
using KooliProjekt.Data;
using KooliProjekt.Services;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class RegisteringsServiceTests : ServiceTestBase
    {
        [Fact]
        public async Task Save_should_add_new_list()
        {
            // Arrange
            var service = new RegisteringService(DbContext);
            var registering = new Registering
            {
                Id = 0, // Use 0 to simulate new entity
                Date = DateTime.Now,
                Payment_Id = "123",
                Event_Id = 123,
                Klient_Id = 123
            };

            // Act
            await service.Save(registering);

            // Assert
            var count = DbContext.Registerings.Count();
            var result = DbContext.Registerings.FirstOrDefault();
            Assert.Equal(1, count);
            Assert.Equal("123", result.Payment_Id);
        }

        [Fact]
        public async Task Save_should_update_existing_panel()
        {
            // Arrange
            var service = new RegisteringService(DbContext);
            var registering = new Registering
            {
                Id = 123,
                Date = DateTime.Now,
                Payment_Id = "123",
                Event_Id = 123,
                Klient_Id = 123
            };

            DbContext.Registerings.Add(registering);
            await DbContext.SaveChangesAsync();

            // Act
            registering.Payment_Id = "Test2";
            await service.Save(registering);

            // Assert
            var newRegister = await DbContext.Registerings.FindAsync(registering.Id);
            Assert.NotNull(newRegister);
            Assert.Equal("Test2", newRegister.Payment_Id);
        }

        [Fact]
        public async Task Get_should_return_correct_panel()
        {
            // Arrange
            var service = new RegisteringService(DbContext);
            var registering = new Registering
            {
                Id = 123,
                Date = DateTime.Now,
                Payment_Id = "123",
                Event_Id = 123,
                Klient_Id = 123
            };

            DbContext.Registerings.Add(registering);
            await DbContext.SaveChangesAsync();

            // Act
            var getRegister = await service.Get(registering.Id);

            // Assert
            Assert.NotNull(getRegister);
            Assert.Equal(registering.Id, getRegister.Id);
        }

        [Fact]
        public async Task Delete_should_remove_given_list()
        {
            // Arrange
            var service = new RegisteringService(DbContext);
            var registering = new Registering
            {
                Id = 123,
                Date = DateTime.Now,
                Payment_Id = "123",
                Event_Id = 123,
                Klient_Id = 123
            };

            DbContext.Registerings.Add(registering);
            await DbContext.SaveChangesAsync();

            // Act
            await service.Delete(registering.Id);

            // Assert
            var count = DbContext.Registerings.Count();
            Assert.Equal(0, count);
        }
        [Fact]
        public async Task List_should_return_correct_paged_result()
        {
            // Arrange
            var service = new RegisteringService(DbContext);

            // Add 12 dummy records
            for (int i = 1; i <= 12; i++)
            {
                DbContext.Registerings.Add(new Registering
                {
                    Date = DateTime.Now,
                    Payment_Id = $"Payment{i}",
                    Event_Id = i,
                    Klient_Id = i
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
