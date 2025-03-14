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
    public class RegisteringServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly RegisteringService _registeringService;

        public RegisteringServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _registeringService = new RegisteringService(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Delete_Should_Call_Delete_On_Repository()
        {
            // Arrange
            int registeringId = 1;
            _mockUnitOfWork.Setup(u => u.RegisteringRepository.Delete(registeringId)).Returns(Task.CompletedTask);

            // Act
            await _registeringService.Delete(registeringId);

            // Assert
            _mockUnitOfWork.Verify(u => u.RegisteringRepository.Delete(registeringId), Times.Once);
        }

        [Fact]
        public async Task Get_Should_Return_Registering()
        {
            // Arrange
            int registeringId = 1;
            var expectedRegistering = new Registering { Id = registeringId };
            _mockUnitOfWork.Setup(u => u.RegisteringRepository.Get(registeringId)).ReturnsAsync(expectedRegistering);

            // Act
            var result = await _registeringService.Get(registeringId);

            // Assert
            Assert.Equal(expectedRegistering, result);
        }

        [Fact]
        public async Task List_Should_Return_PagedResult()
        {
            // Arrange
            int page = 1, pageSize = 10;
            var expectedResult = new PagedResult<Registering>();
            _mockUnitOfWork.Setup(u => u.RegisteringRepository.List(page, pageSize)).ReturnsAsync(expectedResult);

            // Act
            var result = await _registeringService.List(page, pageSize);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task Save_Should_Call_Save_On_Repository()
        {
            // Arrange
            var registering = new Registering();
            _mockUnitOfWork.Setup(u => u.RegisteringRepository.Save(registering)).Returns(Task.CompletedTask);

            // Act
            await _registeringService.Save(registering);

            // Assert
            _mockUnitOfWork.Verify(u => u.RegisteringRepository.Save(registering), Times.Once);
        }
    }
}
