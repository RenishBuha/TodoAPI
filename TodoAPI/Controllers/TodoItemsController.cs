﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoAPI.AuthMiddleware;
using TodoAPI.Models;
using TodoAPI.Services;

namespace TodoAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    
    public class TodoItemsController : ControllerBase
    {
        private readonly IToDoService _toDoService;

        public TodoItemsController(IToDoService toDoService)
        {
            _toDoService = toDoService;
        }

        /// <summary>
        /// GET: api/TodoItems
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[Produces("application/json")]
        public ActionResult<IEnumerable<TodoItemDTO>> GetTodoItems()
        {
            return _toDoService.GetAll();
        }
        
        /// <summary>
        /// GET: api/TodoItems/5 
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
        {
            var todoItem = await _toDoService.FindByIdAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        /// <summary>
        /// POST: api/TodoItems
        /// </summary>
        /// <returns></returns>
        // 
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> PostTodoItem(TodoItemDTO todoItemDTO)
        {
            if (todoItemDTO == null)
            {
                return NotFound();
            }

            await _toDoService.Add(todoItemDTO);
            return Ok();
        }

        // PUT: api/TodoItems/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PutTodoItem([FromRoute] long id, TodoItemDTO todoItemDTO)
        {
            if (id != todoItemDTO.Id)
            {
                return BadRequest();
            }

            bool IsExists = _toDoService.IsItemExists(todoItemDTO.Id);
            if (!IsExists)
            {
                return NotFound();
            }

            await _toDoService.Update(id, todoItemDTO);
            return Ok();
        }

        /// <summary>
        /// DELETE: api/TodoItems/5 
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteTodoItem([FromRoute] long id)
        {
            bool IsExists = _toDoService.IsItemExists(id);
            if (!IsExists)
            {
                return NotFound();
            }

            await _toDoService.Delete(id);

            return Ok();
        }

        /// <summary>
        /// GET: api/TodoItems/GetTodoItemsWithFormat 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetTodoItemsWithFormat.{format}"), FormatFilter]
        public ActionResult<IEnumerable<TodoItemDTO>> GetTodoItemsWithFormat()
        {
            return _toDoService.GetAll();
        }

        /// <summary>
        /// POST: api/TodoItems/PostTodoItemWithFormat 
        /// </summary>
        /// <returns></returns>
        [HttpPost("PostTodoItemWithFormat.{format}"), FormatFilter]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TodoItemDTO>> PostTodoItemWithFormat(TodoItemDTO todoItemDTO)
        {
            if (todoItemDTO == null)
            {
                return NotFound();
            }

            await _toDoService.Add(todoItemDTO);

            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItemDTO.Id }, todoItemDTO);
        }

        /// <summary>
        /// GET: api/TodoItems/get-all
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _toDoService.GetAllAsync();
            if (result == null)
            {
                return NoContent();
            }
            else if (result.Count == 0)
            {
                return NoContent();
            }
            return Ok(result);
        }

        /// <summary>
        /// POST: api/TodoItems/save
        /// </summary>
        /// <returns></returns>
        [HttpPost("save")]
        public async Task<IActionResult> SaveAsync(TodoItemDTO todoItemDTO)
        {
            await _toDoService.Add(todoItemDTO);
            return Ok();
        }
    }
}
