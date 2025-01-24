using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class TodoListsControllerTests
    {
        private readonly Mock<ITodoListService> _todoListServiceMock;
        private readonly TodoListsController _controller;

        public TodoListsControllerTests()
        {
            _todoListServiceMock = new Mock<ITodoListService>();
            _controller = new TodoListsController(_todoListServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_correct_view_with_data()
        {
            // Arrange
            int page = 1;
            var data = new List<TodoList>
            {
                new TodoList { Id = 1, Title = "Test 1" },
                new TodoList { Id = 2, Title = "Test 2" }
            };
            var pagedResult = new PagedResult<TodoList> { Results = data };
            _todoListServiceMock.Setup(x => x.List(page, It.IsAny<int>())).ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Index(page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pagedResult, result.Model);
        }
    }
}