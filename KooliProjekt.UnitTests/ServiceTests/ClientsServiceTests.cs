using System;
using System.Linq;
using System.Threading.Tasks;
using KooliProjekt.Data;
using KooliProjekt.Services;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class ClientsServiceTests : ServiceTestBase
    {
        [Fact]
        public async Task Save_should_add_new_client()
        {
            // Arrange
            var service = new ClientsService(DbContext);
            var client = new Client
            {
                Id = 0,
                Name = "John Doe",
                Email = "john@example.com",
                Phonenumber = "123456789"
            };

            // Act
            await service.Save(client);

            // Assert
            var count = DbContext.Clients.Count();
            var result = DbContext.Clients.FirstOrDefault();
            Assert.Equal(1, count);
            Assert.Equal("John Doe", result.Name);
        }

        [Fact]
        public async Task Save_should_update_existing_client()
        {
            // Arrange
            var service = new ClientsService(DbContext);
            var client = new Client
            {
                Id = 0,
                Name = "John Doe",
                Email = "john@example.com",
                Phonenumber = "123456789"
            };

            DbContext.Clients.Add(client);
            await DbContext.SaveChangesAsync();

            // Act
            client.Name = "Jane Doe";
            await service.Save(client);

            // Assert
            var updated = await DbContext.Clients.FindAsync(client.Id);
            Assert.NotNull(updated);
            Assert.Equal("Jane Doe", updated.Name);
        }

        [Fact]
        public async Task Get_should_return_correct_client()
        {
            // Arrange
            var service = new ClientsService(DbContext);
            var client = new Client
            {
                Id = 0,
                Name = "John Doe",
                Email = "john@example.com",
                Phonenumber = "123456789"
            };

            DbContext.Clients.Add(client);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await service.Get(client.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(client.Id, result.Id);
        }

        [Fact]
        public async Task Delete_should_remove_given_client()
        {
            // Arrange
            var service = new ClientsService(DbContext);
            var client = new Client
            {
                Id = 0,
                Name = "To Delete",
                Email = "delete@example.com",
                Phonenumber = "999999999"
            };

            DbContext.Clients.Add(client);
            await DbContext.SaveChangesAsync();

            // Act
            await service.Delete(client.Id);

            // Assert
            var count = DbContext.Clients.Count();
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task List_should_return_correct_paged_result()
        {
            // Arrange
            var service = new ClientsService(DbContext);

            for (int i = 1; i <= 12; i++)
            {
                DbContext.Clients.Add(new Client
                {
                    Name = $"Client {i}",
                    Email = $"client{i}@example.com",
                    Phonenumber = $"555000{i:D3}"
                });
            }

            await DbContext.SaveChangesAsync();

            // Act
            var result = await service.List(page: 2, pageSize: 5);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Results.Count);
            Assert.Equal(12, result.RowCount);
            Assert.Equal(3, result.PageCount);
            Assert.Equal(2, result.CurrentPage);
            Assert.Equal(5, result.PageSize);
        }
    }
}
