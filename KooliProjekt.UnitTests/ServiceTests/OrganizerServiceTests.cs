using System;
using System.Linq;
using System.Threading.Tasks;
using KooliProjekt.Data;
using KooliProjekt.Services;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class OrganizerServiceTests : ServiceTestBase
    {
        [Fact]
        public async Task Save_should_add_new_organizer()
        {
            // Arrange
            var service = new OrganizerService(DbContext);
            var organizer = new Organizer
            {
                Id = 0, // Simulate new entity
                Name = "Organizer 1",
                Description = "Test description"
            };

            // Act
            await service.Save(organizer);

            // Assert
            var count = DbContext.Organizers.Count();
            var result = DbContext.Organizers.FirstOrDefault();
            Assert.Equal(1, count);
            Assert.Equal("Organizer 1", result.Name);
        }

        [Fact]
        public async Task Save_should_update_existing_organizer()
        {
            // Arrange
            var service = new OrganizerService(DbContext);
            var organizer = new Organizer
            {
                Id = 0,
                Name = "Original Name",
                Description = "Original Description"
            };

            DbContext.Organizers.Add(organizer);
            await DbContext.SaveChangesAsync();

            // Act
            organizer.Name = "Updated Name";
            await service.Save(organizer);

            // Assert
            var updated = await DbContext.Organizers.FindAsync(organizer.Id);
            Assert.NotNull(updated);
            Assert.Equal("Updated Name", updated.Name);
        }

        [Fact]
        public async Task Get_should_return_correct_organizer()
        {
            // Arrange
            var service = new OrganizerService(DbContext);
            var organizer = new Organizer
            {
                Id = 0,
                Name = "Organizer 1",
                Description = "Test description"
            };

            DbContext.Organizers.Add(organizer);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await service.Get(organizer.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(organizer.Id, result.Id);
        }

        [Fact]
        public async Task Delete_should_remove_given_organizer()
        {
            // Arrange
            var service = new OrganizerService(DbContext);
            var organizer = new Organizer
            {
                Id = 0,
                Name = "To Delete",
                Description = "Will be removed"
            };

            DbContext.Organizers.Add(organizer);
            await DbContext.SaveChangesAsync();

            // Act
            await service.Delete(organizer.Id);

            // Assert
            var count = DbContext.Organizers.Count();
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task List_should_return_correct_paged_result()
        {
            // Arrange
            var service = new OrganizerService(DbContext);

            // Add 12 dummy organizers
            for (int i = 1; i <= 12; i++)
            {
                DbContext.Organizers.Add(new Organizer
                {
                    Name = $"Organizer {i}",
                    Description = $"Description {i}"
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
