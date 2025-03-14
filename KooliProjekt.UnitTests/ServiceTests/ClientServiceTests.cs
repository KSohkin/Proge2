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
    public class ClientsServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly ClientsService _clientsService;

        public ClientsServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _clientsService = new ClientsService(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Delete_Should_Call_Delete_On_Repository()
        {
            // Arrange
            int clientId = 1;
            _mockUnitOfWork.Setup(u => u.ClientRepository.Delete(clientId)).Returns(Task.CompletedTask);

            // Act
            await _clientsService.Delete(clientId);

            // Assert
            _mockUnitOfWork.Verify(u => u.ClientRepository.Delete(clientId), Times.Once);
        }

        [Fact]
        public async Task Get_Should_Return_Client()
        {
            // Arrange
            int clientId = 1;
            var expectedClient = new Client { Id = clientId };
            _mockUnitOfWork.Setup(u => u.ClientRepository.Get(clientId)).ReturnsAsync(expectedClient);

            // Act
            var result = await _clientsService.Get(clientId);

            // Assert
            Assert.Equal(expectedClient, result);
        }

        [Fact]
        public async Task List_Should_Return_PagedResult()
        {
            // Arrange
            int page = 1, pageSize = 10;
            var expectedResult = new PagedResult<Client>();
            _mockUnitOfWork.Setup(u => u.ClientRepository.List(page, pageSize)).ReturnsAsync(expectedResult);

            // Act
            var result = await _clientsService.List(page, pageSize);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task Save_Should_Call_Save_On_Repository()
        {
            // Arrange
            var client = new Client();
            _mockUnitOfWork.Setup(u => u.ClientRepository.Save(client)).Returns(Task.CompletedTask);

            // Act
            await _clientsService.Save(client);

            // Assert
            _mockUnitOfWork.Verify(u => u.ClientRepository.Save(client), Times.Once);
        }
    }

}
