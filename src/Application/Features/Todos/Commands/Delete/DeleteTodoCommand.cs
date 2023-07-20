using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.TodoListApp;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.Todos.Commands.Delete
{
    public class DeleteTodoCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }
    internal class DeleteTodoCommandHandler : IRequestHandler<DeleteTodoCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteTodoCommand> _localizer;

        public DeleteTodoCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteTodoCommand> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteTodoCommand command, CancellationToken cancellationToken)
        {
            var todo = await _unitOfWork.Repository<Todo>().GetByIdAsync(command.Id);
            if (todo != null)
            {
                await _unitOfWork.Repository<Todo>().DeleteAsync(todo);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllTodosCacheKey);
                return await Result<int>.SuccessAsync(command.Id, _localizer["Todo Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Todo not found"]);
            }
        }
    }
}
