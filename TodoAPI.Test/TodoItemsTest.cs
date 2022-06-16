using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace TodoAPI.Test
{
    public class TodoItemsTest
    {
        public readonly DbContextOptions<TodoDb> _context;
        public TodoItemsTest()
        {
            _context = new DbContextOptionsBuilder<TodoDb>()
                .UseInMemoryDatabase(databaseName: "TodoList")
                .Options;
        }
        [Fact]
        public void Test1()
        {
            Assert.Equal(1, 1);     
        }   

        [Theory(DisplayName = "Add Numbers")]
        [InlineData(4, 5, 9)]
        [InlineData(2, 3, 5)]
        public void TestAddNumbers(int x, int y, int expectedResult)
        {
            var result = x + y;
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void AddData()
        {
            var blogContext = new TodoDb(_context);

            var todoItem = new TodoModel()
            {
                Id = 1,
                Name = "Renish",
                MobileNumber = "12345"
            };

            blogContext.TodoItem.Add(todoItem);
            blogContext.SaveChanges();

            Assert.Equal(1, blogContext.TodoItem.Count());
        }

        [Fact]
        public void GetData()
        {
            var blogContext = new TodoDb(_context);

            var result = blogContext.TodoItem.Count();
            Assert.Equal(1, result);
        }

        [Fact]
        public void UpdateData()
        {
            var blogContext = new TodoDb(_context);

            var entity = blogContext.TodoItem.Find(1);
            if (entity == null)
            {
                Assert.Equal(1, 0);
                return;
            }

            entity.Name = "Renish New";
            entity.MobileNumber = "67890";

            blogContext.SaveChanges();

            var result = blogContext.TodoItem.Count();
            Assert.Equal(1, result);
        }

        //[Fact]
        //public void DeleteData()
        //{
        //    var blogContext = new TodoDb(_context);

        //    var entity = blogContext.TodoItem.Find(1);
        //    if (entity == null)
        //    {
        //        Assert.Equal(1, 0);
        //        return;
        //    }

        //    blogContext.TodoItem.Remove(entity);
        //    blogContext.SaveChanges();

        //    var result = blogContext.TodoItem.Count();
        //    Assert.Equal(0, result);
        //}
    }
}