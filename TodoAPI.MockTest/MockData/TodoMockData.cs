using TodoAPI.Models;

namespace TodoAPI.TestNew.MockData
{
    public class TodoMockData
    {
        public static  List<TodoItemDTO> GetTodos()
        {
            return new List<TodoItemDTO>
            {
                new TodoItemDTO
                {
                    Id = 1,
                    Name = "Abc",
                    IsComplete = true
                },
                new TodoItemDTO
                {
                    Id = 2,
                    Name = "Xyz",
                    IsComplete = false
                }
            };
        }

        public static List<TodoItemDTO> GetEmptyTodo()
        {
            return new List<TodoItemDTO>();
        }

        public static TodoItemDTO NewTodo()
        {
            return new TodoItemDTO
            {
                Id = 0,
                Name = "Test",
                IsComplete = false
            };
        }

        public static List<TodoItem> GetTodoData()
        {
            return new List<TodoItem>
            {
                new TodoItem
                {
                    Id = 1,
                    Name = "Abc",
                    IsComplete = true
                },
                new TodoItem
                {
                    Id = 2,
                    Name = "Xyz",
                    IsComplete = false
                }
            };
        }
    }
}
