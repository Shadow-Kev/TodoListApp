using BlazorHero.CleanArchitecture.Application.Features.Todos.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.Todos.Queries.GetAll;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Extensions;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.TodoListApp.Todo
{
    public class TodoManager : ITodoManager
    {
        private readonly HttpClient _httpClient;
        public TodoManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IResult<List<GetAllTodosResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.TodosEndpoint.GetAll);
            return await response.ToResult<List<GetAllTodosResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditTodoCommand request)
        {
           var response = await _httpClient.PostAsJsonAsync(Routes.TodosEndpoint.Save, request);
            return await response.ToResult<int>();
        }
    }
}
