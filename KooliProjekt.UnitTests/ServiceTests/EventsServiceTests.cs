using System;
using System.Linq;
using System.Threading.Tasks;
using KooliProjekt.Data;
using KooliProjekt.Services;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class EventsServiceTests : ServiceTestBase
    {
        [Fact]
        public async Task Save_should_add_new_event()
        {
            // Arrange
            var service = new EventsService(DbContext);
            var ev = new Event
            {
                Id = 0,
                Name = "Music Festival",
                Date = DateTime.Today,
                Description = "A large outdoor music festival.",
                Seats = "2000",
                Price = "49.99",
                Summary = "Live music and food trucks",
                Organizer = "Music Org Inc."
            };

            // Act
            await service.Save(ev);

            // Assert
            var count = DbContext.Events.Count();
            var result = DbContext.Events.FirstOrDefault();
            Assert.Equal(1, count);
            Assert.Equal("Music Festival", result.Name);
        }

        [Fact]
        public async Task Save_should_update_existing_event()
        {
            // Arrange
            var service = new EventsService(DbContext);
            var ev = new Event
            {
                Id = 0,
                Name = "Original Name",
                Date = DateTime.Today,
                Description = "Original Description",
                Seats = "1000",
                Price = "25",
                Summary = "Original Summary",
                Organizer = "Original Org"
            };

            DbContext.Events.Add(ev);
            await DbContext.SaveChangesAsync();

            // Act
            ev.Name = "Updated Name";
            await service.Save(ev);

            // Assert
            var updated = await DbContext.Events.FindAsync(ev.Id);
            Assert.NotNull(updated);
            Assert.Equal("Updated Name", updated.Name);
        }

        [Fact]
        public async Task Get_should_return_correct_event()
        {
            // Arrange
            var service = new EventsService(DbContext);
            var ev = new Event
            {
                Id = 0,
                Name = "GetTest",
                Date = DateTime.Today,
                Description = "To get",
                Seats = "300",
                Price = "15",
                Summary = "A test event",
                Organizer = "Test Org"
            };

            DbContext.Events.Add(ev);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await service.Get(ev.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ev.Id, result.Id);
        }

        [Fact]
        public async Task Delete_should_remove_given_event()
        {
            // Arrange
            var service = new EventsService(DbContext);
            var ev = new Event
            {
                Id = 0,
                Name = "To Delete",
                Date = DateTime.Today,
                Description = "To be deleted",
                Seats = "100",
                Price = "10",
                Summary = "Deleting",
                Organizer = "DeleteOrg"
            };

            DbContext.Events.Add(ev);
            await DbContext.SaveChangesAsync();

            // Act
            await service.Delete(ev.Id);

            // Assert
            var count = DbContext.Events.Count();
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task List_should_return_correct_paged_result()
        {
            // Arrange
            var service = new EventsService(DbContext);

            for (int i = 1; i <= 12; i++)
            {
                DbContext.Events.Add(new Event
                {
                    Name = $"Event {i}",
                    Date = DateTime.Today,
                    Description = $"Description {i}",
                    Seats = $"{100 + i}",
                    Price = $"{20 + i}.00",
                    Summary = $"Summary {i}",
                    Organizer = $"Organizer {i}"
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
