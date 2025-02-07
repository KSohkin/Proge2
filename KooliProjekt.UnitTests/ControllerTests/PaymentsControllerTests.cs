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
    public class PaymentsControllerTests
    {
        private readonly Mock<IPaymentService> _paymentsServiceMock;
        private readonly PaymentsController _controller;

        public PaymentsControllerTests()
        {
            _paymentsServiceMock = new Mock<IPaymentService>();
            _controller = new PaymentsController(_paymentsServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_view_and_data()
        {
            // Arrange
            var page = 1;
            var data = new List<Payment>
            {
                new Payment { Id = 1, Price = "Test 1", Description = "Bleh", Payment_nr = "12", Deadline = DateTime.Today },
                new Payment { Id = 2, Price = "Test 2", Description = "Blah", Payment_nr = "15", Deadline = DateTime.Today}
            };
            var pagedResult = new PagedResult<Payment>
            {
                Results = data,
                CurrentPage = 1,
                PageCount = 1,
                PageSize = 5,
                RowCount = 2
            };
            _paymentsServiceMock
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
            var list = (Payment)null;
            _paymentsServiceMock
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
            var list = new Payment { Id = id };
            _paymentsServiceMock
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
            var list = (Payment)null;
            _paymentsServiceMock
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
            var list = new Payment { Id = id };
            _paymentsServiceMock
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
            var list = (Payment)null;
            _paymentsServiceMock
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
            var list = new Payment { Id = id };
            _paymentsServiceMock
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
            var list = new Payment { Id = 1 };
            _controller.ModelState.AddModelError("Id", "Requiered");

            var result = await _controller.Create(list) as ViewResult;


            Assert.NotNull(result);
            Assert.Equal(list, result.Model);
        }

        [Fact]
        public async Task Edit_should_save_and_redirect_when_model_is_valid()
        {
            var list = new Payment { Id = 1, Price = "Test 1" };

            _paymentsServiceMock
                .Setup(x => x.Save(It.IsAny<Payment>()))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

            var result = await _controller.Edit(list.Id, list) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _paymentsServiceMock.VerifyAll();
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_id_does_not_match_model_id()
        {
            var list = new Payment { Id = 1 };
            int id = 2;

            _paymentsServiceMock
                .Setup(x => x.Get(id))
                    .ReturnsAsync(list);

            var result = await _controller.Edit(id, list) as NotFoundResult;

            Assert.NotNull(result);
        }

        [Fact]
        public async Task DeleteConfirmed_should_delete_and_redirect_when_valid_id()
        {
            int id = 1;
            var list = new Payment { Id = id, Price = "Test 1" };
            _paymentsServiceMock.Setup(x => x.Get(id)).ReturnsAsync(list);

            var result = await _controller.DeleteConfirmed(id) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _paymentsServiceMock.VerifyAll();

        }

        [Fact]
        public async Task Create_ValidClient_ReturnsRedirectToAction()
        {
            // Arrange
            var payment = new Payment { Id = 1, Price = "Test 1" };
            _paymentsServiceMock.Setup(service => service.Save(It.IsAny<Payment>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(payment);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            _paymentsServiceMock.Verify(service => service.Save(payment), Times.Once);
        }

        [Fact]
        public async Task Create_InvalidModelState_ReturnsViewWithClient()
        {
            // Arrange
            var payment = new Payment { Id = 1, Price = "" }; // Invalid data
            _controller.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = await _controller.Create(payment);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(payment, viewResult.Model);
            _paymentsServiceMock.Verify(service => service.Save(It.IsAny<Payment>()), Times.Never);
        }
        [Fact]
        public async Task Create_Valid_Client_ReturnsRedirectToAction()
        {
            // Arrange
            var payment = new Payment { Id = 1, Price = "Test 1" };
            _paymentsServiceMock.Setup(service => service.Save(It.IsAny<Payment>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(payment);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            _paymentsServiceMock.Verify(service => service.Save(payment), Times.Once);
        }

        [Fact]
        public async Task Create_InvalidModel_State_ReturnsViewWithClient()
        {
            // Arrange
            var payment = new Payment { Id = 1, Price = "" }; // Invalid data
            _controller.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = await _controller.Create(payment);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(payment, viewResult.Model);
            _paymentsServiceMock.Verify(service => service.Save(It.IsAny<Payment>()), Times.Never);
        }



        [Fact]
        public async Task Delete_ClientExists_ReturnsViewWithClient()
        {
            // Arrange
            var payment = new Payment { Id = 1, Price = "John Doe" };
            _paymentsServiceMock.Setup(service => service.Get(payment.Id)).ReturnsAsync(payment);

            // Act
            var result = await _controller.Delete(payment.Id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(payment, viewResult.Model);
        }

        [Fact]
        public async Task Delete_ClientDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            _paymentsServiceMock.Setup(service => service.Get(It.IsAny<int>())).ReturnsAsync((Payment)null);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteConfirmed_ClientExists_DeletesClientAndRedirects()
        {
            // Arrange
            var payment = new Payment { Id = 1, Price = "John Doe" };
            _paymentsServiceMock.Setup(service => service.Get(payment.Id)).ReturnsAsync(payment);
            _paymentsServiceMock.Setup(service => service.Delete(payment.Id)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteConfirmed(payment.Id);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            _paymentsServiceMock.Verify(service => service.Delete(payment.Id), Times.Once);
        }

        [Fact]
        public async Task DeleteConfirmed_ClientDoesNotExist_RedirectsToIndex()
        {
            // Arrange
            _paymentsServiceMock.Setup(service => service.Get(It.IsAny<int>())).ReturnsAsync((Payment)null);

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
            Payment payment = new Payment { Id = id };
            var exception = new DbUpdateConcurrencyException();
            // Act

            _paymentsServiceMock.Setup(x => x.Save(It.IsAny<Payment>())).ThrowsAsync(exception).Verifiable();
            _paymentsServiceMock.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync((Payment)null).Verifiable();
            var result = await _controller.Edit(id, payment);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            _paymentsServiceMock.VerifyAll();
        }
        [Fact]
        public async Task Edit_InvalidModelState_ReturnsView()
        {
            _controller.ModelState.AddModelError("Error", "Invalid");
            var payment = new Payment();

            var result = await _controller.Edit(payment.Id, payment);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(payment, viewResult.Model);
        }

        [Fact]
        public async Task Edit_ConcurrencyException_ClientNotFound_ReturnsNotFound()
        {
            var payment = new Payment { Id = 1 };
            _paymentsServiceMock.Setup(s => s.Save(payment)).ThrowsAsync(new DbUpdateConcurrencyException());
            _paymentsServiceMock.Setup(s => s.Get(payment.Id)).ReturnsAsync((Payment)null);

            var result = await _controller.Edit(payment.Id, payment);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ConcurrencyException_ClientExists_ThrowsException()
        {
            var payment = new Payment { Id = 1 };

            _paymentsServiceMock.Setup(s => s.Save(It.IsAny<Payment>()))
                    .ThrowsAsync(new DbUpdateConcurrencyException());

            _paymentsServiceMock.Setup(s => s.Get(payment.Id))
                    .ReturnsAsync(payment);

            // Ensure Save is actually called
            _paymentsServiceMock.Verify(s => s.Save(payment), Times.Never);

            var exception = await Assert.ThrowsAsync<DbUpdateConcurrencyException>(
                () => _controller.Edit(payment.Id, payment)
            );

            Assert.NotNull(exception);
        }


        [Fact]
        public async Task Edit_ValidClient_RedirectsToIndex()
        {
            var payment = new Payment { Id = 1 };
            _paymentsServiceMock.Setup(s => s.Save(payment)).Returns(Task.CompletedTask);

            var result = await _controller.Edit(payment.Id, payment);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);

        }
        [Fact]
        public async Task ClientExists_WhenClientExists_ReturnsTrue()
        {
            // Arrange
            var payment = new Payment { Id = 1 };
            _paymentsServiceMock.Setup(s => s.Get(payment.Id)).ReturnsAsync(payment);

            // Act
            var result = await _controller.ClientExists(payment.Id);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ClientExists_WhenClientDoesNotExist_ReturnsFalse()
        {
            // Arrange
            _paymentsServiceMock.Setup(s => s.Get(It.IsAny<int>())).ReturnsAsync((Payment)null);

            // Act
            var result = await _controller.ClientExists(1);

            // Assert
            Assert.False(result);
        }
    }
}