using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.TodoListApp;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.Todos.Queries.GetById
{
    public class GetTodoByIdQuery : IRequest<Result<GetTodoByIdResponse>>
    {
        public int Id { get; set; }
    }
    internal class GetTodoByIdQueryHandler : IRequestHandler<GetTodoByIdQuery, Result<GetTodoByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetTodoByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetTodoByIdResponse>> Handle(GetTodoByIdQuery query, CancellationToken cancellationToken)
        {
            var todo = await _unitOfWork.Repository<Todo>().GetByIdAsync(query.Id);
            var mappedTodo = _mapper.Map<GetTodoByIdResponse>(todo);
            return await Result<GetTodoByIdResponse>.SuccessAsync(mappedTodo);
        }
    }

}
