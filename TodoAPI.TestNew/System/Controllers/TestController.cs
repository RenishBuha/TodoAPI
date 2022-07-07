using TodoAPI.Controllers;
using TodoAPI.Services;
using TodoAPI.TestNew.MockData;
using Microsoft.AspNetCore.Mvc;

namespace TodoAPI.TestNew.System.Controllers
{
    public class TestController
    {
        [Fact]
        public async Task GetAllAsync_Test()
        {
            //Arrange
            var todoservice = new Mock<IToDoService>();
            todoservice.Setup(x => x.GetAllAsync()).ReturnsAsync(TodoMockData.GetTodos());
            var sut = new TodoItemsController(todoservice.Object);

            //Act
            var result = (OkObjectResult) await sut.GetAllAsync();

            //Assert
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetAllAsync_EmptyTest()
        {
            /// Arrange
            var todoService = new Mock<IToDoService>();
            todoService.Setup(x => x.GetAllAsync()).ReturnsAsync(TodoMockData.GetEmptyTodo());
            var sut = new TodoItemsController(todoService.Object);

            /// Act
            var result = (NoContentResult)await sut.GetAllAsync();

            /// Assert
            result.StatusCode.Should().Be(204);
            todoService.Verify(x => x.GetAllAsync(), Times.Exactly(1));
        }

        [Fact]
        public async Task SaveAsync_PostTest()
        {
            //Arrange
            var todoService = new Mock<IToDoService>();
            
            var newTodo = TodoMockData.NewTodo();

            var sut = new TodoItemsController(todoService.Object);

            //Act
            var result = await sut.SaveAsync(newTodo);

            //Assert
            todoService.Verify(x => x.Add(newTodo), Times.Exactly(1));
        }
    }
}
