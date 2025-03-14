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
    public class PaymentServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly PaymentService _paymentService;

        public PaymentServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _paymentService = new PaymentService(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Delete_Should_Call_Delete_On_Repository()
        {
            // Arrange
            int paymentId = 1;
            _mockUnitOfWork.Setup(u => u.PaymentRepository.Delete(paymentId)).Returns(Task.CompletedTask);

            // Act
            await _paymentService.Delete(paymentId);

            // Assert
            _mockUnitOfWork.Verify(u => u.PaymentRepository.Delete(paymentId), Times.Once);
        }

        [Fact]
        public async Task Get_Should_Return_Payment()
        {
            // Arrange
            int paymentId = 1;
            var expectedPayment = new Payment { Id = paymentId };
            _mockUnitOfWork.Setup(u => u.PaymentRepository.Get(paymentId)).ReturnsAsync(expectedPayment);

            // Act
            var result = await _paymentService.Get(paymentId);

            // Assert
            Assert.Equal(expectedPayment, result);
        }

        [Fact]
        public async Task List_Should_Return_PagedResult()
        {
            // Arrange
            int page = 1, pageSize = 10;
            var expectedResult = new PagedResult<Payment>();
            _mockUnitOfWork.Setup(u => u.PaymentRepository.List(page, pageSize)).ReturnsAsync(expectedResult);

            // Act
            var result = await _paymentService.List(page, pageSize);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task Save_Should_Call_Save_On_Repository()
        {
            // Arrange
            var payment = new Payment();
            _mockUnitOfWork.Setup(u => u.PaymentRepository.Save(payment)).Returns(Task.CompletedTask);

            // Act
            await _paymentService.Save(payment);

            // Assert
            _mockUnitOfWork.Verify(u => u.PaymentRepository.Save(payment), Times.Once);
        }
    }
}
