using TodoAPI.Models;

namespace TodoAPI.Mappers
{
    public static class ToDoMapper
    {
        public static TodoItemDTO? Map(TodoItem todoItem)
        {
            if(todoItem == null) return null;
            return new()
            {
                Id = todoItem.Id,
                Name = todoItem.Name,
                IsComplete = todoItem.IsComplete
            };
        } 
    }
}
