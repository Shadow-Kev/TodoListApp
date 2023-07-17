using BlazorHero.CleanArchitecture.Application.Features.Brands.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.Todos.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.Todos.Queries.GetAll;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.TodoListApp.Todo
{
    public interface ITodoManager : IManager
    {
        Task<IResult<List<GetAllTodosResponse>>> GetAllAsync();
        Task<IResult<int>> SaveAsync(AddEditTodoCommand request);
    }
}
