using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.TodoListApp;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.Todos.Commands.AddEdit
{
    public partial class AddEditTodoCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime? ExpirationDate { get; set; }
        [Required]
        public int Priority { get; set; }
        public bool IsCompleteted { get; set; }
    }
    internal class AddEditTodoCommandHandler : IRequestHandler<AddEditTodoCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditTodoCommand> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;
        public AddEditTodoCommandHandler(IMapper mapper, IStringLocalizer<AddEditTodoCommand> localizer, IUnitOfWork<int> unitOfWork)
        {
            _mapper = mapper;
            _localizer = localizer;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle (AddEditTodoCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var todo = _mapper.Map<Todo>(command);
                await _unitOfWork.Repository<Todo>().AddAsync(todo);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllTodosCacheKey);
                return await Result<int>.SuccessAsync(todo.Id, _localizer["Todo Saved"]);
            }
            else
            {
                var todo = await _unitOfWork.Repository<Todo>().GetByIdAsync(command.Id);
                if (todo != null)
                {
                    todo.Title = command.Title ?? todo.Title;
                    todo.Description = command.Description ?? todo.Description;
                    if (command.ExpirationDate != default(DateTime))
                    {
                        todo.ExpirationDate = (DateTime)command.ExpirationDate;
                    }
                    todo.Priority = (command.Priority != todo.Priority) ? command.Priority : todo.Priority;
                    todo.IsCompleteted = (command.IsCompleteted != todo.IsCompleteted) ? command.IsCompleteted : todo.IsCompleteted;
                    await _unitOfWork.Repository<Todo>().UpdateAsync(todo);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllTodosCacheKey);
                    return await Result<int>.SuccessAsync(todo.Id, _localizer["Todo Updated"]);
                }else
                {
                    return await Result<int>.FailAsync(_localizer["Todo not found"]);
                }
            }
        }
    }
}
