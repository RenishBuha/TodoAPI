using TodoAPI.Services;
using TodoAPI.Models;
using TodoAPI.TestNew.MockData;

using Microsoft.EntityFrameworkCore;
using TodoAPI.Mappers;

namespace TodoAPI.TestNew.System.Services
{
    public class TestToDoService : IDisposable
    {
        protected readonly TodoContext _TodoContext;

        public TestToDoService()
        {
            var option = new DbContextOptionsBuilder<TodoContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _TodoContext = new TodoContext(option);

            _TodoContext.Database.EnsureCreated();
        }

        [Fact]
        public async Task GetAllAsync_Test()
        {
            //Arrange

            _TodoContext.TodoItems.AddRange(TodoMockData.GetTodoData());
            await _TodoContext.SaveChangesAsync();

            var sut = new ToDoService(_TodoContext);

            //Act
            var result = await sut.GetAllAsync();

            //Assert
            result.Should().HaveCount(TodoMockData.GetTodoData().Count);
        }

        [Fact]
        public async Task SaveAsync_Test()
        {
            //Arrange
            var newTodo = TodoMockData.NewTodo();

            _TodoContext.TodoItems.AddRange(TodoMockData.GetTodoData());
            _TodoContext.SaveChanges();

            var sut = new ToDoService(_TodoContext);

            //Act
            await sut.Add(newTodo);

            //Assert
            int expectedCount = (TodoMockData.GetTodoData().Count + 1);
            _TodoContext.TodoItems.Count().Should().Be(expectedCount);
        }

        public void Dispose()
        {
            _TodoContext.Database.EnsureDeleted();
            _TodoContext.Dispose();
        }
    }
}
