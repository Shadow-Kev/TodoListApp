using BlazorHero.CleanArchitecture.Application.Features.Todos.Queries.GetAll;
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
        [Authorize(Policy = Permissions.Todos.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var todos = await _mediator.Send(new GetAllTodosQuery());
            return Ok(todos);
        }
    }
}
