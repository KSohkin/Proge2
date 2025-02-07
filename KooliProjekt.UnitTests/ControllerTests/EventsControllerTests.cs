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
    public class EventsServiceTests
    {
        private readonly Mock<IEventsService> _eventserviceMock;
        private readonly EventsController _controller;

        public EventsServiceTests()
        {
            _eventserviceMock = new Mock<IEventsService>();
            _controller = new EventsController(_eventserviceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_view_and_data()
        {
            // Arrange
            var page = 1;
            var data = new List<Event>
            {
                new Event { Id = 1, Name = "Test 1", Date = DateTime.Now, Description = "Blah", Organizer = "John Cena",Seats = "1500", Price = "67.1 Million", Summary = "Cena back" },
                new Event { Id = 2, Name = "Test 2", Date = DateTime.Now, Description = "Bleh", Organizer = "John Mema",Seats = "1800", Price = "67.5 Million", Summary = "Cena back 2" }
            };
            var pagedResult = new PagedResult<Event>
            {
                Results = data,
                CurrentPage = 1,
                PageCount = 1,
                PageSize = 5,
                RowCount = 2
            };
            _eventserviceMock.Setup(x => x.List(page, It.IsAny<int>())).ReturnsAsync(pagedResult);

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
            var list = (Event)null;
            _eventserviceMock.Setup(x => x.Get(id)).ReturnsAsync(list);

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
            var list = new Event { Id = id };
            _eventserviceMock.Setup(x => x.Get(id)).ReturnsAsync(list);

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
            var list = (Event)null;
            _eventserviceMock.Setup(x => x.Get(id)).ReturnsAsync(list);

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
            var list = new Event { Id = id };
            _eventserviceMock.Setup(x => x.Get(id)).ReturnsAsync(list);

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
            var list = (Event)null;
            _eventserviceMock.Setup(x => x.Get(id)).ReturnsAsync(list);

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
            var list = new Event { Id = id };
            _eventserviceMock.Setup(x => x.Get(id)).ReturnsAsync(list);

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
                var list = new Event { Id = 1 };
                _controller.ModelState.AddModelError("Id", "Requiered");

                var result = await _controller.Create(list) as ViewResult;


                Assert.NotNull(result);
                Assert.Equal(list, result.Model);
            }

            [Fact]
            public async Task Edit_should_save_and_redirect_when_model_is_valid()
            {
                var list = new Event { Id = 1, Name = "Test 1"};

            _eventserviceMock
                .Setup(x => x.Save(It.IsAny<Event>()))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

                var result = await _controller.Edit(list.Id, list) as RedirectToActionResult;

                Assert.NotNull(result);
                Assert.Equal("Index", result.ActionName);
            _eventserviceMock.VerifyAll();
            }

            [Fact]
            public async Task Edit_should_return_notfound_when_id_does_not_match_model_id()
            {
                var list = new Event { Id = 1 };
                int id = 2;

            _eventserviceMock
                .Setup(x => x.Get(id))
                    .ReturnsAsync(list);

                var result = await _controller.Edit(id, list) as NotFoundResult;

                Assert.NotNull(result);
            }

            [Fact]
            public async Task DeleteConfirmed_should_delete_and_redirect_when_valid_id()
            {
                int id = 1;
                var list = new Event { Id = id, Name = "Test 1",};
            _eventserviceMock.Setup(x => x.Get(id)).ReturnsAsync(list);

                var result = await _controller.DeleteConfirmed(id) as RedirectToActionResult;

                Assert.NotNull(result);
                Assert.Equal("Index", result.ActionName);
            _eventserviceMock.VerifyAll();

            }

            [Fact]
            public async Task Create_ValidClient_ReturnsRedirectToAction()
            {
                // Arrange
                var @event = new Event { Id = 1, Name = "Test 1"};
            _eventserviceMock.Setup(service => service.Save(It.IsAny<Event>())).Returns(Task.CompletedTask);

                // Act
                var result = await _controller.Create(@event);

                // Assert
                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirectToActionResult.ActionName);
            _eventserviceMock.Verify(service => service.Save(@event), Times.Once);
            }

            [Fact]
            public async Task Create_InvalidModelState_ReturnsViewWithClient()
            {
                // Arrange
                var @event = new Event { Id = 1, Name = ""}; // Invalid data
                _controller.ModelState.AddModelError("Name", "Name is required");

                // Act
                var result = await _controller.Create(@event);

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Equal(@event, viewResult.Model);
            _eventserviceMock.Verify(service => service.Save(It.IsAny<Event>()), Times.Never);
            }
            [Fact]
            public async Task Create_Valid_Client_ReturnsRedirectToAction()
            {
                // Arrange
                var @event = new Event { Id = 1, Name = "Test 1"};
            _eventserviceMock.Setup(service => service.Save(It.IsAny<Event>())).Returns(Task.CompletedTask);

                // Act
                var result = await _controller.Create(@event);

                // Assert
                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirectToActionResult.ActionName);
            _eventserviceMock.Verify(service => service.Save(@event), Times.Once);
            }

            [Fact]
            public async Task Create_InvalidModel_State_ReturnsViewWithClient()
            {
                // Arrange
                var @event = new Event { Id = 1, Name = ""}; // Invalid data
                _controller.ModelState.AddModelError("Name", "Name is required");

                // Act
                var result = await _controller.Create(@event);

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Equal(@event, viewResult.Model);
            _eventserviceMock.Verify(service => service.Save(It.IsAny<Event>()), Times.Never);
            }



            [Fact]
            public async Task Delete_ClientExists_ReturnsViewWithClient()
            {
                // Arrange
                var @event = new Event { Id = 1, Name = "John Doe" };
            _eventserviceMock.Setup(service => service.Get(@event.Id)).ReturnsAsync(@event);

                // Act
                var result = await _controller.Delete(@event.Id);

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Equal(@event, viewResult.Model);
            }

            [Fact]
            public async Task Delete_ClientDoesNotExist_ReturnsNotFound()
            {
            // Arrange
            _eventserviceMock.Setup(service => service.Get(It.IsAny<int>())).ReturnsAsync((Event)null);

                // Act
                var result = await _controller.Delete(1);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }

            [Fact]
            public async Task DeleteConfirmed_ClientExists_DeletesClientAndRedirects()
            {
                // Arrange
                var @event = new Event { Id = 1, Name = "John Doe" };
            _eventserviceMock.Setup(service => service.Get(@event.Id)).ReturnsAsync(@event);
            _eventserviceMock.Setup(service => service.Delete(@event.Id)).Returns(Task.CompletedTask);

                // Act
                var result = await _controller.DeleteConfirmed(@event.Id);

                // Assert
                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirectToActionResult.ActionName);
            _eventserviceMock.Verify(service => service.Delete(@event.Id), Times.Once);
            }

            [Fact]
            public async Task DeleteConfirmed_ClientDoesNotExist_RedirectsToIndex()
            {
            // Arrange
            _eventserviceMock.Setup(service => service.Get(It.IsAny<int>())).ReturnsAsync((Event)null);

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
                Event @event = new Event { Id = id };
                var exception = new DbUpdateConcurrencyException();
            // Act

            _eventserviceMock.Setup(x => x.Save(It.IsAny<Event>())).ThrowsAsync(exception).Verifiable();
            _eventserviceMock.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync((Event)null).Verifiable();
                var result = await _controller.Edit(id, @event);

                // Assert
                Assert.NotNull(result);
                Assert.IsType<NotFoundResult>(result);
            _eventserviceMock.VerifyAll();
            }
            [Fact]
            public async Task Edit_InvalidModelState_ReturnsView()
            {
                _controller.ModelState.AddModelError("Error", "Invalid");
                var @event = new Event();

                var result = await _controller.Edit(@event.Id, @event);

                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Equal(@event, viewResult.Model);
            }

            [Fact]
            public async Task Edit_ConcurrencyException_ClientNotFound_ReturnsNotFound()
            {
                var @event = new Event { Id = 1 };
            _eventserviceMock.Setup(s => s.Save(@event)).ThrowsAsync(new DbUpdateConcurrencyException());
            _eventserviceMock.Setup(s => s.Get(@event.Id)).ReturnsAsync((Event)null);

                var result = await _controller.Edit(@event.Id, @event);

                Assert.IsType<NotFoundResult>(result);
            }

            [Fact]
            public async Task Edit_ConcurrencyException_ClientExists_ThrowsException()
            {
                var @event = new Event { Id = 1 };

            _eventserviceMock.Setup(s => s.Save(It.IsAny<Event>()))
                    .ThrowsAsync(new DbUpdateConcurrencyException());

            _eventserviceMock.Setup(s => s.Get(@event.Id))
                    .ReturnsAsync(@event);

            // Ensure Save is actually called
            _eventserviceMock.Verify(s => s.Save(@event), Times.Never);

                var exception = await Assert.ThrowsAsync<DbUpdateConcurrencyException>(
                    () => _controller.Edit(@event.Id, @event)
                );

                Assert.NotNull(exception);
            }


            [Fact]
            public async Task Edit_ValidClient_RedirectsToIndex()
            {
                var @event = new Event { Id = 1 };
            _eventserviceMock.Setup(s => s.Save(@event)).Returns(Task.CompletedTask);

                var result = await _controller.Edit(@event.Id, @event);

                var redirectResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirectResult.ActionName);

            }
            [Fact]
            public async Task ClientExists_WhenClientExists_ReturnsTrue()
            {
                // Arrange
                var @event = new Event { Id = 1 };
            _eventserviceMock.Setup(s => s.Get(@event.Id)).ReturnsAsync(@event);

                // Act
                var result = await _controller.ClientExists(@event.Id);

                // Assert
                Assert.True(result);
            }

            [Fact]
            public async Task ClientExists_WhenClientDoesNotExist_ReturnsFalse()
            {
            // Arrange
            _eventserviceMock.Setup(s => s.Get(It.IsAny<int>())).ReturnsAsync((Event)null);

                // Act
                var result = await _controller.ClientExists(1);

                // Assert
                Assert.False(result);
            }
        }
    }

