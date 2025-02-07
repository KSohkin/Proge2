using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class ClientsServiceTests
    {
        private readonly Mock<IClientService> _clientServiceMock;
        private readonly ClientsController _controller;

        public ClientsServiceTests()
        {
            _clientServiceMock = new Mock<IClientService>();
            _controller = new ClientsController(_clientServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_view_and_data()
        {
            // Arrange
            var page = 1;
            var data = new List<Client>
            {
                new Client { Id = 1, Name = "Test 1" },
                new Client { Id = 2, Name = "Test 2" }
            };
            var pagedResult = new PagedResult<Client>
            {
                Results = data,
                CurrentPage = 1,
                PageCount = 1,
                PageSize = 5,
                RowCount = 2
            };
            _clientServiceMock
                .Setup(x => x.List(page, It.IsAny<int>()))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Index(page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(
                string.IsNullOrEmpty(result.ViewName) ||
                result.ViewName == "Index"
            );
            Assert.Equal(pagedResult, result.Model);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_id_is_missing()
        {
            // Arrange
            int? id = null;

            // Act
            var result = await _controller.Details(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_list_is_missing()
        {
            // Arrange
            int id = 1;
            var list = (Client)null;
            _clientServiceMock.Setup(x => x.Get(id)).ReturnsAsync(list);

            // Act
            var result = await _controller.Details(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_should_return_view_with_model_when_list_was_found()
        {
            // Arrange
            int id = 1;
            var list = new Client { Id = id };
            _clientServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(list);

            // Act
            var result = await _controller.Details(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(
                string.IsNullOrEmpty(result.ViewName) ||
                result.ViewName == "Details"
            );
            Assert.Equal(list, result.Model);
        }

        [Fact]
        public void Create_should_return_view()
        {
            // Act
            var result = _controller.Create() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(
                string.IsNullOrEmpty(result.ViewName) ||
                result.ViewName == "Create"
            );
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_id_is_missing()
        {
            // Arrange
            int? id = null;

            // Act
            var result = await _controller.Edit(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_list_is_missing()
        {
            // Arrange
            int id = 1;
            var list = (Client)null;
            _clientServiceMock.Setup(x => x.Get(id)).ReturnsAsync(list);

            // Act
            var result = await _controller.Edit(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_should_return_view_with_model_when_list_was_found()
        {
            // Arrange
            int id = 1;
            var list = new Client { Id = id };
            _clientServiceMock.Setup(x => x.Get(id)).ReturnsAsync(list);

            // Act
            var result = await _controller.Edit(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(
                string.IsNullOrEmpty(result.ViewName) ||
                result.ViewName == "Edit"
            );
            Assert.Equal(list, result.Model);
        }

        [Fact]
        public async Task Delete_should_return_notfound_when_id_is_missing()
        {
            // Arrange
            int? id = null;

            // Act
            var result = await _controller.Delete(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_should_return_notfound_when_list_is_missing()
        {
            // Arrange
            int id = 1;
            var list = (Client)null;
            _clientServiceMock.Setup(x => x.Get(id)).ReturnsAsync(list);

            // Act
            var result = await _controller.Delete(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Create_should_return_view_when_model_is_invalid()
        {
            var list = new Client { Id = 1 };
            _controller.ModelState.AddModelError("Id", "Requiered");

            var result = await _controller.Create(list) as ViewResult;


            Assert.NotNull(result);
            Assert.Equal(list, result.Model);
        }

        [Fact]
        public async Task Edit_should_save_and_redirect_when_model_is_valid()
        {
            var list = new Client { Id = 1, Email = "JohnDoe@gmail.com", Name = "John Doe", Phonenumber = "5122252"  };

            _clientServiceMock
                .Setup(x => x.Save(It.IsAny<Client>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var result = await _controller.Edit(list.Id, list) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _clientServiceMock.VerifyAll();
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_id_does_not_match_model_id()
        {
            var list = new Client { Id = 1 };
            int id = 2;

            _clientServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(list);

            var result = await _controller.Edit(id, list) as NotFoundResult;

            Assert.NotNull(result);
        }

        [Fact]
        public async Task DeleteConfirmed_should_delete_and_redirect_when_valid_id()
        {
            int id = 1;
            var list = new Client { Id = id, Name = "Jane Woe", Phonenumber = "5166215", Email = "JaneWoe@gmail.com"  };
            _clientServiceMock.Setup(x => x.Get(id)).ReturnsAsync(list);

            var result = await _controller.DeleteConfirmed(id) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _clientServiceMock.VerifyAll();

        }

        [Fact]
        public async Task Create_ValidClient_ReturnsRedirectToAction()
        {
            // Arrange
            var client = new Client { Id = 1, Name = "John Doe", Email = "john@example.com", Phonenumber = "1234567890" };
            _clientServiceMock.Setup(service => service.Save(It.IsAny<Client>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(client);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            _clientServiceMock.Verify(service => service.Save(client), Times.Once);
        }

        [Fact]
        public async Task Create_InvalidModelState_ReturnsViewWithClient()
        {
            // Arrange
            var client = new Client { Id = 1, Name = "", Email = "invalid-email", Phonenumber = "1234567890" }; // Invalid data
            _controller.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = await _controller.Create(client);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(client, viewResult.Model);
            _clientServiceMock.Verify(service => service.Save(It.IsAny<Client>()), Times.Never);
        }
        [Fact]
        public async Task Create_Valid_Client_ReturnsRedirectToAction()
        {
            // Arrange
            var client = new Client { Id = 1, Name = "John Doe", Email = "john@example.com", Phonenumber = "1234567890" };
            _clientServiceMock.Setup(service => service.Save(It.IsAny<Client>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(client);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            _clientServiceMock.Verify(service => service.Save(client), Times.Once);
        }

        [Fact]
        public async Task Create_InvalidModel_State_ReturnsViewWithClient()
        {
            // Arrange
            var client = new Client { Id = 1, Name = "", Email = "invalid-email", Phonenumber = "1234567890" }; // Invalid data
            _controller.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = await _controller.Create(client);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(client, viewResult.Model);
            _clientServiceMock.Verify(service => service.Save(It.IsAny<Client>()), Times.Never);
        }

    

        [Fact]
        public async Task Delete_ClientExists_ReturnsViewWithClient()
        {
            // Arrange
            var client = new Client { Id = 1, Name = "John Doe" };
            _clientServiceMock.Setup(service => service.Get(client.Id)).ReturnsAsync(client);

            // Act
            var result = await _controller.Delete(client.Id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(client, viewResult.Model);
        }

        [Fact]
        public async Task Delete_ClientDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            _clientServiceMock.Setup(service => service.Get(It.IsAny<int>())).ReturnsAsync((Client)null);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteConfirmed_ClientExists_DeletesClientAndRedirects()
        {
            // Arrange
            var client = new Client { Id = 1, Name = "John Doe" };
            _clientServiceMock.Setup(service => service.Get(client.Id)).ReturnsAsync(client);
            _clientServiceMock.Setup(service => service.Delete(client.Id)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteConfirmed(client.Id);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            _clientServiceMock.Verify(service => service.Delete(client.Id), Times.Once);
        }

        [Fact]
        public async Task DeleteConfirmed_ClientDoesNotExist_RedirectsToIndex()
        {
            // Arrange
            _clientServiceMock.Setup(service => service.Get(It.IsAny<int>())).ReturnsAsync((Client)null);

            // Act
            var result = await _controller.DeleteConfirmed(1);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task Create_should_return_notfound_when_exception()
        {
            // Arrange
            int id = 1;
            Client client = new Client { Id = id };
            var exception = new DbUpdateConcurrencyException();
            // Act

            _clientServiceMock.Setup(x => x.Save(It.IsAny<Client>())).ThrowsAsync(exception).Verifiable();
            _clientServiceMock.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync((Client)null).Verifiable();
            var result = await _controller.Edit(id, client);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            _clientServiceMock.VerifyAll();
        }
        [Fact]
        public async Task Edit_InvalidModelState_ReturnsView()
        {
            _controller.ModelState.AddModelError("Error", "Invalid");
            var client = new Client();

            var result = await _controller.Edit(client.Id, client);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(client, viewResult.Model);
        }

        [Fact]
        public async Task Edit_ConcurrencyException_ClientNotFound_ReturnsNotFound()
        {
            var client = new Client { Id = 1 };
            _clientServiceMock.Setup(s => s.Save(client)).ThrowsAsync(new DbUpdateConcurrencyException());
            _clientServiceMock.Setup(s => s.Get(client.Id)).ReturnsAsync((Client)null);

            var result = await _controller.Edit(client.Id, client);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ConcurrencyException_ClientExists_ThrowsException()
        {
            var client = new Client { Id = 1 };

            _clientServiceMock.Setup(s => s.Save(It.IsAny<Client>()))
                .ThrowsAsync(new DbUpdateConcurrencyException());

            _clientServiceMock.Setup(s => s.Get(client.Id))
                .ReturnsAsync(client);

            // Ensure Save is actually called
            _clientServiceMock.Verify(s => s.Save(client), Times.Never);

            var exception = await Assert.ThrowsAsync<DbUpdateConcurrencyException>(
                () => _controller.Edit(client.Id, client)
            );

            Assert.NotNull(exception);
        }


        [Fact]
        public async Task Edit_ValidClient_RedirectsToIndex()
        {
            var client = new Client { Id = 1 };
            _clientServiceMock.Setup(s => s.Save(client)).Returns(Task.CompletedTask);

            var result = await _controller.Edit(client.Id, client);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);

        }
        [Fact]
        public async Task ClientExists_WhenClientExists_ReturnsTrue()
        {
            // Arrange
            var client = new Client { Id = 1 };
            _clientServiceMock.Setup(s => s.Get(client.Id)).ReturnsAsync(client);

            // Act
            var result = await _controller.ClientExists(client.Id);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ClientExists_WhenClientDoesNotExist_ReturnsFalse()
        {
            // Arrange
            _clientServiceMock.Setup(s => s.Get(It.IsAny<int>())).ReturnsAsync((Client)null);

            // Act
            var result = await _controller.ClientExists(1);

            // Assert
            Assert.False(result);
        }
    }
}
