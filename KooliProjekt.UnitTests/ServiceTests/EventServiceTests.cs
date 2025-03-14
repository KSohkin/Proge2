using KooliProjekt.Data.Repositories;
using KooliProjekt.Data;
using KooliProjekt.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class EventsServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly EventsService _eventsService;

        public EventsServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _eventsService = new EventsService(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Delete_Should_Call_Delete_On_Repository()
        {
            // Arrange
            int eventId = 1;
            _mockUnitOfWork.Setup(u => u.EventRepository.Delete(eventId)).Returns(Task.CompletedTask);

            // Act
            await _eventsService.Delete(eventId);

            // Assert
            _mockUnitOfWork.Verify(u => u.EventRepository.Delete(eventId), Times.Once);
        }

        [Fact]
        public async Task Get_Should_Return_Event()
        {
            // Arrange
            int eventId = 1;
            var expectedEvent = new Event { Id = eventId };
            _mockUnitOfWork.Setup(u => u.EventRepository.Get(eventId)).ReturnsAsync(expectedEvent);

            // Act
            var result = await _eventsService.Get(eventId);

            // Assert
            Assert.Equal(expectedEvent, result);
        }

        [Fact]
        public async Task List_Should_Return_PagedResult()
        {
            // Arrange
            int page = 1, pageSize = 10;
            var expectedResult = new PagedResult<Event>();
            _mockUnitOfWork.Setup(u => u.EventRepository.List(page, pageSize)).ReturnsAsync(expectedResult);

            // Act
            var result = await _eventsService.List(page, pageSize);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task Save_Should_Call_Save_On_Repository()
        {
            // Arrange
            var @event = new Event();
            _mockUnitOfWork.Setup(u => u.EventRepository.Save(@event)).Returns(Task.CompletedTask);

            // Act
            await _eventsService.Save(@event);

            // Assert
            _mockUnitOfWork.Verify(u => u.EventRepository.Save(@event), Times.Once);
        }
    }
}
