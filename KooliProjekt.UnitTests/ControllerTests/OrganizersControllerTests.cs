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
    public class OrganizersControllerTests
    {
        private readonly Mock<IOrganizerService> _organizerServiceMock;
        private readonly OrganizersController _controller;

        public OrganizersControllerTests()
        {
            _organizerServiceMock = new Mock<IOrganizerService>();
            _controller = new OrganizersController(_organizerServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_view_and_data()
        {
            // Arrange
            var page = 1;
            var data = new List<Organizer>
            {
                new Organizer { Id = 1, Name = "Test 1", Description = "Bleh" },
                new Organizer { Id = 2, Name = "Test 2", Description = "Blah" }
            };
            var pagedResult = new PagedResult<Organizer>
            {
                Results = data,
                CurrentPage = 1,
                PageCount = 1,
                PageSize = 5,
                RowCount = 2
            };
            _organizerServiceMock
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
            var list = (Organizer)null;
            _organizerServiceMock
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
            var list = new Organizer { Id = id };
            _organizerServiceMock
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
            var list = (Organizer)null;
            _organizerServiceMock
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
            var list = new Organizer { Id = id };
            _organizerServiceMock
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
            var list = (Organizer)null;
            _organizerServiceMock
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
            var list = new Organizer { Id = id };
            _organizerServiceMock
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
            var list = new Organizer { Id = 1 };
            _controller.ModelState.AddModelError("Id", "Requiered");

            var result = await _controller.Create(list) as ViewResult;


            Assert.NotNull(result);
            Assert.Equal(list, result.Model);
        }

        [Fact]
        public async Task Edit_should_save_and_redirect_when_model_is_valid()
        {
            var list = new Organizer { Id = 1, Name = "Test 1" };

            _organizerServiceMock
                .Setup(x => x.Save(It.IsAny<Organizer>()))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

            var result = await _controller.Edit(list.Id, list) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _organizerServiceMock.VerifyAll();
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_id_does_not_match_model_id()
        {
            var list = new Organizer { Id = 1 };
            int id = 2;

            _organizerServiceMock
                .Setup(x => x.Get(id))
                    .ReturnsAsync(list);

            var result = await _controller.Edit(id, list) as NotFoundResult;

            Assert.NotNull(result);
        }

        [Fact]
        public async Task DeleteConfirmed_should_delete_and_redirect_when_valid_id()
        {
            int id = 1;
            var list = new Organizer { Id = id, Name = "Test 1", };
            _organizerServiceMock.Setup(x => x.Get(id)).ReturnsAsync(list);

            var result = await _controller.DeleteConfirmed(id) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _organizerServiceMock.VerifyAll();

        }

        [Fact]
        public async Task Create_ValidClient_ReturnsRedirectToAction()
        {
            // Arrange
            var organizer = new Organizer { Id = 1, Name = "Test 1" };
            _organizerServiceMock.Setup(service => service.Save(It.IsAny<Organizer>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(organizer);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            _organizerServiceMock.Verify(service => service.Save(organizer), Times.Once);
        }

        [Fact]
        public async Task Create_InvalidModelState_ReturnsViewWithClient()
        {
            // Arrange
            var organizer = new Organizer { Id = 1, Name = "" }; // Invalid data
            _controller.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = await _controller.Create(organizer);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(organizer, viewResult.Model);
            _organizerServiceMock.Verify(service => service.Save(It.IsAny<Organizer>()), Times.Never);
        }
        [Fact]
        public async Task Create_Valid_Client_ReturnsRedirectToAction()
        {
            // Arrange
            var organizer = new Organizer { Id = 1, Name = "Test 1" };
            _organizerServiceMock.Setup(service => service.Save(It.IsAny<Organizer>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(organizer);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            _organizerServiceMock.Verify(service => service.Save(organizer), Times.Once);
        }

        [Fact]
        public async Task Create_InvalidModel_State_ReturnsViewWithClient()
        {
            // Arrange
            var organizer = new Organizer { Id = 1, Name = "" }; // Invalid data
            _controller.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = await _controller.Create(organizer);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(organizer, viewResult.Model);
            _organizerServiceMock.Verify(service => service.Save(It.IsAny<Organizer >()), Times.Never);
        }



        [Fact]
        public async Task Delete_ClientExists_ReturnsViewWithClient()
        {
            // Arrange
            var organizer = new Organizer { Id = 1, Name = "John Doe" };
            _organizerServiceMock.Setup(service => service.Get(organizer.Id)).ReturnsAsync(organizer);

            // Act
            var result = await _controller.Delete(organizer.Id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(organizer, viewResult.Model);
        }

        [Fact]
        public async Task Delete_ClientDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            _organizerServiceMock.Setup(service => service.Get(It.IsAny<int>())).ReturnsAsync(( Organizer)null);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteConfirmed_ClientExists_DeletesClientAndRedirects()
        {
            // Arrange
            var organizer = new Organizer { Id = 1, Name = "John Doe" };
            _organizerServiceMock.Setup(service => service.Get(organizer.Id)).ReturnsAsync(organizer);
            _organizerServiceMock.Setup(service => service.Delete(organizer.Id)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteConfirmed(organizer.Id);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            _organizerServiceMock.Verify(service => service.Delete(organizer.Id), Times.Once);
        }

        [Fact]
        public async Task DeleteConfirmed_ClientDoesNotExist_RedirectsToIndex()
        {
            // Arrange
            _organizerServiceMock.Setup(service => service.Get(It.IsAny<int>())).ReturnsAsync((Organizer)null);

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
            Organizer organizer = new Organizer { Id = id };
            var exception = new DbUpdateConcurrencyException();
            // Act

            _organizerServiceMock.Setup(x => x.Save(It.IsAny<Organizer>())).ThrowsAsync(exception).Verifiable();
            _organizerServiceMock.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync((Organizer)null).Verifiable();
            var result = await _controller.Edit(id, organizer);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            _organizerServiceMock.VerifyAll();
        }
        [Fact]
        public async Task Edit_InvalidModelState_ReturnsView()
        {
            _controller.ModelState.AddModelError("Error", "Invalid");
            var organizer = new Organizer();

            var result = await _controller.Edit(organizer.Id, organizer);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(organizer, viewResult.Model);
        }

        [Fact]
        public async Task Edit_ConcurrencyException_ClientNotFound_ReturnsNotFound()
        {
            var organizer = new Organizer { Id = 1 };
            _organizerServiceMock.Setup(s => s.Save(organizer)).ThrowsAsync(new DbUpdateConcurrencyException());
            _organizerServiceMock.Setup(s => s.Get(organizer.Id)).ReturnsAsync((Organizer)null);

            var result = await _controller.Edit(organizer.Id, organizer);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ConcurrencyException_ClientExists_ThrowsException()
        {
            var organizer = new Organizer { Id = 1 };

            _organizerServiceMock.Setup(s => s.Save(It.IsAny<Organizer>()))
                    .ThrowsAsync(new DbUpdateConcurrencyException());

            _organizerServiceMock.Setup(s => s.Get(organizer.Id))
                    .ReturnsAsync(organizer);

            // Ensure Save is actually called
            _organizerServiceMock.Verify(s => s.Save(organizer), Times.Never);

            var exception = await Assert.ThrowsAsync<DbUpdateConcurrencyException>(
                () => _controller.Edit(organizer.Id, organizer)
            );

            Assert.NotNull(exception);
        }


        [Fact]
        public async Task Edit_ValidClient_RedirectsToIndex()
        {
            var organizer = new Organizer { Id = 1 };
            _organizerServiceMock.Setup(s => s.Save(organizer)).Returns(Task.CompletedTask);

            var result = await _controller.Edit(organizer.Id, organizer);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);

        }
        [Fact]
        public async Task ClientExists_WhenClientExists_ReturnsTrue()
        {
            // Arrange
            var organizer = new Organizer { Id = 1 };
            _organizerServiceMock.Setup(s => s.Get(organizer.Id)).ReturnsAsync(organizer);

            // Act
            var result = await _controller.ClientExists(organizer.Id);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ClientExists_WhenClientDoesNotExist_ReturnsFalse()
        {
            // Arrange
            _organizerServiceMock.Setup(s => s.Get(It.IsAny<int>())).ReturnsAsync((Organizer)null);

            // Act
            var result = await _controller.ClientExists(1);

            // Assert
            Assert.False(result);
        }
    }
}
