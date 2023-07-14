using BlazorHero.CleanArchitecture.Application.Features.Todos.Queries.GetAll;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlazorHero.CleanArchitecture.Server.Controllers.v1.TodoAppList
{
    public class TodosController : BaseApiController<TodosController>
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var todos = await _mediator.Send(new GetAllTodosQuery());
            return Ok(todos);
        }
    }
}
