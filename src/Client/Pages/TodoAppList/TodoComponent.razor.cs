using BlazorHero.CleanArchitecture.Application.Features.Todos.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.Todos.Queries.GetAll;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.TodoListApp.Todo;
using BlazorHero.CleanArchitecture.Client.Shared.Dialogs;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.TodoAppList
{
    public partial class TodoComponent
    {
        [Inject] private ITodoManager TodoManager { get; set; }

        private List<GetAllTodosResponse> _todoList = new();
        private GetAllTodosResponse _todo = new();
        private AddEditTodoCommand _command { get; set; } = new();
        private string _searchString = "";
        private bool _loading = true;
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        protected override async Task OnInitializedAsync()
        {
            await GetTodosAsync();
            _loading = false;
        }

        private async Task GetTodosAsync()
        {
            await Reset();
        }
        private async Task Reset()
        {
            var result = await TodoManager.GetAllAsync();
            if (result.Succeeded)
            {
                _todoList = result.Data;
                _loading = false;
            }
            else
            {
                foreach (var message in result.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private async Task Delete(int id)
        {
            string deleteContent = _localizer["Delete Content"];
            var parameters = new DialogParameters
            {
                {nameof(DeleteConfirmation.ContentText), string.Format(deleteContent, id) }
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await TodoManager.DeleteAsync(id);
                if (response.Succeeded)
                {
                    await Reset();
                    _snackBar.Add(response.Messages[0], Severity.Success);
                }
                else
                {
                    await Reset();
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }

        private async Task InvokeModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {
                _todo = _todoList.FirstOrDefault(t => t.Id == id);
                if (_todo != null)
                {
                    parameters.Add(nameof(AddEditTodoModal.AddEditTodoModel), new AddEditTodoCommand
                    {
                        Id = _todo.Id,
                        Title = _todo.Title,
                        Description = _todo.Description,
                        Priority = _todo.Priority,
                        ExpirationDate = _todo.ExpirationDate,
                        IsCompleteted = _todo.IsCompleteted
                    });
                }
                var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
                var dialog = _dialogService.Show<AddEditTodoModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
                var result = await dialog.Result;
                if (!result.Cancelled)
                {
                    await Reset();
                }
            }
        }

        private bool Search(GetAllTodosResponse todo)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (todo.Title?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return todo.Description?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true;

        }


    }
}
