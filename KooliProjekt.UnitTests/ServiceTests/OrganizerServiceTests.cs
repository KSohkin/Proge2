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
    public class OrganizerServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly OrganizerService _organizerService;

        public OrganizerServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _organizerService = new OrganizerService(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Delete_Should_Call_Delete_On_Repository()
        {
            // Arrange
            int organizerId = 1;
            _mockUnitOfWork.Setup(u => u.OrganizerRepository.Delete(organizerId)).Returns(Task.CompletedTask);

            // Act
            await _organizerService.Delete(organizerId);

            // Assert
            _mockUnitOfWork.Verify(u => u.OrganizerRepository.Delete(organizerId), Times.Once);
        }

        [Fact]
        public async Task Get_Should_Return_Organizer()
        {
            // Arrange
            int organizerId = 1;
            var expectedOrganizer = new Organizer { Id = organizerId };
            _mockUnitOfWork.Setup(u => u.OrganizerRepository.Get(organizerId)).ReturnsAsync(expectedOrganizer);

            // Act
            var result = await _organizerService.Get(organizerId);

            // Assert
            Assert.Equal(expectedOrganizer, result);
        }

        [Fact]
        public async Task List_Should_Return_PagedResult()
        {
            // Arrange
            int page = 1, pageSize = 10;
            var expectedResult = new PagedResult<Organizer>();
            _mockUnitOfWork.Setup(u => u.OrganizerRepository.List(page, pageSize)).ReturnsAsync(expectedResult);

            // Act
            var result = await _organizerService.List(page, pageSize);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task Save_Should_Call_Save_On_Repository()
        {
            // Arrange
            var organizer = new Organizer();
            _mockUnitOfWork.Setup(u => u.OrganizerRepository.Save(organizer)).Returns(Task.CompletedTask);

            // Act
            await _organizerService.Save(organizer);

            // Assert
            _mockUnitOfWork.Verify(u => u.OrganizerRepository.Save(organizer), Times.Once);
        }
    }
}
