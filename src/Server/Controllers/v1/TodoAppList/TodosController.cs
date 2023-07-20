using BlazorHero.CleanArchitecture.Application.Features.Todos.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.Todos.Commands.Delete;
using BlazorHero.CleanArchitecture.Application.Features.Todos.Queries.GetAll;
using BlazorHero.CleanArchitecture.Application.Features.Todos.Queries.GetById;
using BlazorHero.CleanArchitecture.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlazorHero.CleanArchitecture.Server.Controllers.v1.TodoAppList
{
    public class TodosController : BaseApiController<TodosController>
    {
        /// <summary>
        /// Get All Todos
        /// </summary>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.Todos.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var todos = await _mediator.Send(new GetAllTodosQuery());
            return Ok(todos);
        }
        /// <summary>
        /// Create/Update a Todo
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.Todos.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditTodoCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        ///<summary>
        ///Get a Todo By Id
        ///</summary>
        ///<param name="id"></param>
        ///<returns>Status 200 OK</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var todo = await _mediator.Send(new GetTodoByIdQuery { Id = id });
            return Ok(todo);
        }

        ///<summary>
        ///Delete a Todo
        ///</summary>
        ///<param name="id"></param>
        ///<returns>Status 200 OK</returns>

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var todo = await _mediator.Send(new DeleteTodoCommand { Id = id });
            return Ok(todo);
        }
    }
}
