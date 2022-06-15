using Microsoft.AspNetCore.Mvc;
using TodoAPI.Mappers;
using TodoAPI.Models;

namespace TodoAPI.Services
{
    public interface IToDoService
    {
        bool IsItemExists(long id);

        List<TodoItemDTO> GetAll();

        Task<TodoItemDTO> FindByIdAsync(long id);

        Task Add(TodoItemDTO todoItemDTO);

        Task Update(long id, TodoItemDTO todoItemDTO);

        Task Delete(long id);
    }

    public class ToDoService : IToDoService
    {
        private readonly TodoContext _context;

        public ToDoService(TodoContext context)
        {
            _context = context;
        }

        public bool IsItemExists(long id)
        {
            return (_context.TodoItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public List<TodoItemDTO> GetAll()
        {
            return _context.TodoItems.Select(x => ToDoMapper.Map(x)).ToList();
        }

        public async Task<TodoItemDTO> FindByIdAsync(long id)
        {
            var entity = await _context.TodoItems.FindAsync(id);
            return ToDoMapper.Map(entity);
        }
        
        public async Task Add(TodoItemDTO todoItemDTO)
        {
            var todoItem = new TodoItem()
            {
                Name = todoItemDTO.Name,
                IsComplete = todoItemDTO.IsComplete
            };

            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();
        }

        public async Task Update(long id, TodoItemDTO todoItemDTO)
        {
            var entity = await _context.TodoItems.FindAsync(id);
            entity.Name = todoItemDTO.Name;
            entity.IsComplete = todoItemDTO.IsComplete;

            await _context.SaveChangesAsync();
        }

        public async Task Delete(long id)
        {
            var entity = await _context.TodoItems.FindAsync(id);

            _context.TodoItems.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
