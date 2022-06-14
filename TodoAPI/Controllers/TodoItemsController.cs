
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TodoAPI.Models;

namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoItemsController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/TodoItems/GetTodoItems
        [HttpGet]
        //[Produces("application/xml")]
        [Route("GetTodoItems", Name = "TodoItems")]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
        {
            if (_context.TodoItems == null)
            {
                return NotFound();
            }
            return await _context.TodoItems.Select(x => ItemToDTO(x)).ToListAsync();
        }

        // GET: api/TodoItems/GetTodoItems/5
        [HttpGet]
        [Route("GetTodoItems/{id}")]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
        {
            if (_context.TodoItems == null)
            {
                return NotFound();
            }

            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return ItemToDTO(todoItem);
        }

        // POST: api/TodoItems/PostTodoItem
        [HttpPost]
        [Route("PostTodoItem")]
        public async Task<ActionResult<TodoItemDTO>> PostTodoItem(TodoItemDTO todoItemDTO)
        {
            var todoItem = new TodoItem()
            {
                Name = todoItemDTO.Name,
                IsComplete = todoItemDTO.IsComplete
            };

            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
            //return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
        }

        // PUT: api/TodoItems/PutTodoItem/5
        [HttpPut]
        [Route("PutTodoItem/{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItemDTO todoItemDTO)
        {
            if (id != todoItemDTO.Id)
            {
                return BadRequest();
            }

            var todoItem = await _context.TodoItems.FindAsync(id);
            if(todoItem == null)
            {
                return NotFound();
            }

            todoItem.Name = todoItemDTO.Name;
            todoItem.IsComplete = todoItemDTO.IsComplete;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TodoItemExists(id))
            {
                if (!TodoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/TodoItems/DeleteTodoItem/5
        [HttpDelete]
        [Route("DeleteTodoItem/{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            if (_context.TodoItems == null)
            {
                return NotFound();
            }
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoItemExists(long id)
        {
            return (_context.TodoItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
           new TodoItemDTO
           {
               Id = todoItem.Id,
               Name = todoItem.Name,
               IsComplete = todoItem.IsComplete
           };

        // GET: api/TodoItems/GetTodoItemsWithFormat
        [HttpGet("GetTodoItemsWithFormat.{format}"), FormatFilter]
        //[Produces("application/xml")]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItemsWithFormat()
        {
            if (_context.TodoItems == null)
            {
                return NotFound();
            }
            return await _context.TodoItems.Select(x => ItemToDTO(x)).ToListAsync();
        }

        // POST: api/TodoItems/PostTodoItemWithFormat
        [HttpPost("PostTodoItemWithFormat.{format}"), FormatFilter]
        public async Task<ActionResult<TodoItemDTO>> PostTodoItemWithFormat(TodoItemDTO todoItemDTO)
        {
            var todoItem = new TodoItem()
            {
                Name = todoItemDTO.Name,
                IsComplete = todoItemDTO.IsComplete
            };

            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
        }

        // GET: api/TodoItems/GetTodoItemsWithDist
        [HttpGet]
        [Route("GetTodoItemsWithDist")]
        public ActionResult GetTodoItemsWithDist()
        {
            IDictionary<TodoItemDTO, int> resource = new Dictionary<TodoItemDTO, int>
            {
                { new TodoItemDTO {Id = 1, Name = "Abc", IsComplete = true}, 0 },
                { new TodoItemDTO {Id = 2, Name = "XYZ", IsComplete = false}, 1 },
            };
            string json = JsonConvert.SerializeObject(resource.ToList());
            return Ok(json);
            
            //return Ok(JsonConvert.SerializeObject(resource));
            //return Ok(resource);
        }
    }
}
