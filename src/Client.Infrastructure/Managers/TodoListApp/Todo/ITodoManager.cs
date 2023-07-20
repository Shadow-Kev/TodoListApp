using BlazorHero.CleanArchitecture.Application.Features.Todos.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.Todos.Queries.GetAll;
using BlazorHero.CleanArchitecture.Application.Features.Todos.Queries.GetById;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.TodoListApp.Todo
{
    public interface ITodoManager : IManager
    {
        Task<IResult<List<GetAllTodosResponse>>> GetAllAsync();
        Task<IResult<int>> SaveAsync(AddEditTodoCommand request);
        Task<IResult<int>> DeleteAsync(int id);
        Task<IResult<GetTodoByIdResponse>> GetByIdAsync(GetTodoByIdQuery request);
    }
}
