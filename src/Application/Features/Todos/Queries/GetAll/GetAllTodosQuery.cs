using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.TodoListApp;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using LazyCache;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.Todos.Queries.GetAll
{
    public class GetAllTodosQuery : IRequest<Result<List<GetAllTodosResponse>>>
    {
        public GetAllTodosQuery() { }
    }
    internal class GetAllTodosCachedQueryHandler : IRequestHandler<GetAllTodosQuery, Result<List<GetAllTodosResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;
        public GetAllTodosCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }
        public async Task<Result<List<GetAllTodosResponse>>> Handle(GetAllTodosQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<Todo>>> getAllTodos = () => _unitOfWork.Repository<Todo>().GetAllAsync();
            var todoList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllTodosCacheKey, getAllTodos);
            var mappedTodos = _mapper.Map<List<GetAllTodosResponse>>(todoList);
            return await Result<List<GetAllTodosResponse>>.SuccessAsync(mappedTodos);
        }
    }
}
