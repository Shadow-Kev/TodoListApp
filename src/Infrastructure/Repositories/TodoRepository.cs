using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.TodoListApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Infrastructure.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly IRepositoryAsync<Todo, int> _todoRepository;
        public TodoRepository(IRepositoryAsync<Todo, int> todoRepository) => _todoRepository = todoRepository;

    }
}
