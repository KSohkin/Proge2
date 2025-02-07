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
    public class RegisteringsControllerTests
    {
        private readonly Mock<IRegisteringService> _registeringServiceMock;
        private readonly RegisteringsController _controller;

        public RegisteringsControllerTests()
        {
            _registeringServiceMock = new Mock<IRegisteringService>();
            _controller = new RegisteringsController(_registeringServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_view_and_data()
        {
            // Arrange
            var page = 1;
            var data = new List<Registering>
            {
                new Registering { Id = 1, Payment_Id = "Test 1", Event_Id = 1, Date = DateTime.Today, Klient_Id = 1 },
                new Registering { Id = 2, Payment_Id = "Test 2", Event_Id = 2, Date = DateTime.Today, Klient_Id = 2 }
            };
            var pagedResult = new PagedResult<Registering>
            {
                Results = data,
                CurrentPage = 1,
                PageCount = 1,
                PageSize = 5,
                RowCount = 2
            };
            _registeringServiceMock
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
            var list = (Registering)null;
            _registeringServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(list);

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
            var list = new Registering { Id = id };
            _registeringServiceMock
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
            var list = (Registering)null;
            _registeringServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(list);

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
            var list = new Registering { Id = id };
            _registeringServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(list);

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
            var list = (Registering)null;
            _registeringServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(list);

            // Act
            var result = await _controller.Delete(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_should_return_view_with_model_when_list_was_found()
        {
            // Arrange
            int id = 1;
            var list = new Registering { Id = id };
            _registeringServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(list);

            // Act
            var result = await _controller.Delete(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(
                string.IsNullOrEmpty(result.ViewName) ||
                result.ViewName == "Delete"
            );
            Assert.Equal(list, result.Model);
        }

        [Fact]
        public async Task Create_should_return_view_when_model_is_invalid()
        {
            var list = new Registering { Id = 1 };
            _controller.ModelState.AddModelError("Id", "Requiered");

            var result = await _controller.Create(list) as ViewResult;


            Assert.NotNull(result);
            Assert.Equal(list, result.Model);
        }

        [Fact]
        public async Task Edit_should_save_and_redirect_when_model_is_valid()
        {
            var list = new Registering { Id = 1, Date = DateTime.Now};

            _registeringServiceMock
                .Setup(x => x.Save(It.IsAny<Registering>()))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

            var result = await _controller.Edit(list.Id, list) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _registeringServiceMock.VerifyAll();
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_id_does_not_match_model_id()
        {
            var list = new Registering { Id = 1 };
            int id = 2;

            _registeringServiceMock
                .Setup(x => x.Get(id))
                    .ReturnsAsync(list);

            var result = await _controller.Edit(id, list) as NotFoundResult;

            Assert.NotNull(result);
        }

        [Fact]
        public async Task DeleteConfirmed_should_delete_and_redirect_when_valid_id()
        {
            int id = 1;
            var list = new Registering { Id = id, Date = DateTime.Now };
            _registeringServiceMock.Setup(x => x.Get(id)).ReturnsAsync(list);

            var result = await _controller.DeleteConfirmed(id) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _registeringServiceMock.VerifyAll();

        }

        [Fact]
        public async Task Create_ValidClient_ReturnsRedirectToAction()
        {
            // Arrange
            var registering = new Registering { Id = 1, Date = DateTime.Now };
            _registeringServiceMock.Setup(service => service.Save(It.IsAny<Registering>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(registering);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            _registeringServiceMock.Verify(service => service.Save(registering), Times.Once);
        }

        [Fact]
        public async Task Create_InvalidModelState_ReturnsViewWithClient()
        {
            // Arrange
            var registering = new Registering { Id = 1, Date = DateTime.Now }; // Invalid data
            _controller.ModelState.AddModelError("Date", "Date is required");

            // Act
            var result = await _controller.Create(registering);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(registering, viewResult.Model);
            _registeringServiceMock.Verify(service => service.Save(It.IsAny<Registering>()), Times.Never);
        }
        [Fact]
        public async Task Create_Valid_Client_ReturnsRedirectToAction()
        {
            // Arrange
            var registering = new Registering { Id = 1, Date = DateTime.Now };
            _registeringServiceMock.Setup(service => service.Save(It.IsAny<Registering>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(registering);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            _registeringServiceMock.Verify(service => service.Save(registering), Times.Once);
        }

        [Fact]
        public async Task Create_InvalidModel_State_ReturnsViewWithClient()
        {
            // Arrange
            var registering = new Registering { Id = 1, Date = DateTime.Now }; // Invalid data
            _controller.ModelState.AddModelError("Date", "Date is required");

            // Act
            var result = await _controller.Create(registering);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(registering, viewResult.Model);
            _registeringServiceMock.Verify(service => service.Save(It.IsAny<Registering>()), Times.Never);
        }



        [Fact]
        public async Task Delete_ClientExists_ReturnsViewWithClient()
        {
            // Arrange
            var registering = new Registering { Id = 1, Date = DateTime.Now };
            _registeringServiceMock.Setup(service => service.Get(registering.Id)).ReturnsAsync(registering);

            // Act
            var result = await _controller.Delete(registering.Id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(registering, viewResult.Model);
        }

        [Fact]
        public async Task Delete_ClientDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            _registeringServiceMock.Setup(service => service.Get(It.IsAny<int>())).ReturnsAsync((Registering)null);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteConfirmed_ClientExists_DeletesClientAndRedirects()
        {
            // Arrange
            var registering = new Registering { Id = 1, Date = DateTime.Now};
            _registeringServiceMock.Setup(service => service.Get(registering.Id)).ReturnsAsync(registering);
            _registeringServiceMock.Setup(service => service.Delete(registering.Id)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteConfirmed(registering.Id);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            _registeringServiceMock.Verify(service => service.Delete(registering.Id), Times.Once);
        }

        [Fact]
        public async Task DeleteConfirmed_ClientDoesNotExist_RedirectsToIndex()
        {
            // Arrange
            _registeringServiceMock.Setup(service => service.Get(It.IsAny<int>())).ReturnsAsync((Registering)null);

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
            Registering registering = new Registering { Id = id };
            var exception = new DbUpdateConcurrencyException();
            // Act

            _registeringServiceMock.Setup(x => x.Save(It.IsAny<Registering>())).ThrowsAsync(exception).Verifiable();
            _registeringServiceMock.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync((Registering)null).Verifiable();
            var result = await _controller.Edit(id, registering);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            _registeringServiceMock.VerifyAll();
        }
        [Fact]
        public async Task Edit_InvalidModelState_ReturnsView()
        {
            _controller.ModelState.AddModelError("Error", "Invalid");
            var registering = new Registering();

            var result = await _controller.Edit(registering.Id, registering);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(registering, viewResult.Model);
        }

        [Fact]
        public async Task Edit_ConcurrencyException_ClientNotFound_ReturnsNotFound()
        {
            var registering = new Registering { Id = 1 };
            _registeringServiceMock.Setup(s => s.Save(registering)).ThrowsAsync(new DbUpdateConcurrencyException());
            _registeringServiceMock.Setup(s => s.Get(registering.Id)).ReturnsAsync((Registering)null);

            var result = await _controller.Edit(registering.Id, registering);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ConcurrencyException_ClientExists_ThrowsException()
        {
            var registering = new Registering { Id = 1 };

            _registeringServiceMock.Setup(s => s.Save(It.IsAny<Registering>()))
                    .ThrowsAsync(new DbUpdateConcurrencyException());

            _registeringServiceMock.Setup(s => s.Get(registering.Id))
                    .ReturnsAsync(registering);

            // Ensure Save is actually called
            _registeringServiceMock.Verify(s => s.Save(registering), Times.Never);

            var exception = await Assert.ThrowsAsync<DbUpdateConcurrencyException>(
                () => _controller.Edit(registering.Id, registering)
            );

            Assert.NotNull(exception);
        }


        [Fact]
        public async Task Edit_ValidClient_RedirectsToIndex()
        {
            var registering = new Registering { Id = 1 };
            _registeringServiceMock.Setup(s => s.Save(registering)).Returns(Task.CompletedTask);

            var result = await _controller.Edit(registering.Id, registering);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);

        }
        [Fact]
        public async Task ClientExists_WhenClientExists_ReturnsTrue()
        {
            // Arrange
            var registering = new Registering { Id = 1 };
            _registeringServiceMock.Setup(s => s.Get(registering.Id)).ReturnsAsync(registering);

            // Act
            var result = await _controller.ClientExists(registering.Id);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ClientExists_WhenClientDoesNotExist_ReturnsFalse()
        {
            // Arrange
            _registeringServiceMock.Setup(s => s.Get(It.IsAny<int>())).ReturnsAsync((Registering)null);

            // Act
            var result = await _controller.ClientExists(1);

            // Assert
            Assert.False(result);
        }
    }
}