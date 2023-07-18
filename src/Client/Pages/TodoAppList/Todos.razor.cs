using BlazorHero.CleanArchitecture.Application.Features.Todos.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Client.Extensions;
using BlazorHero.CleanArchitecture.Application.Features.Todos.Queries.GetAll;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.TodoListApp.Todo;
using BlazorHero.CleanArchitecture.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.TodoAppList
{
    public partial class Todos
    {
        [Inject] private ITodoManager TodoManager { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllTodosResponse> _todosList = new ();
        private GetAllTodosResponse _todo = new();
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canSearchTodos;
        private bool _canCreateTodos;
        private bool _canEditTodos;
        private bool _canViewTodos;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canSearchTodos = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Todos.Search)).Succeeded;
            _canViewTodos = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Todos.View)).Succeeded;
            _canCreateTodos = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Todos.Create)).Succeeded;
            _canEditTodos = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Todos.Edit)).Succeeded;

            await GetTodosAsync();
            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager, _localStorage);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }

        }

        private async Task GetTodosAsync()
        {
            var response = await TodoManager.GetAllAsync();
            if (response.Succeeded)
            {
                _todosList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private async Task InvokeModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {
                _todo = _todosList.FirstOrDefault(t => t.Id == id);
                if (_todo != null )
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

        private async Task Reset()
        {
            _todo = new GetAllTodosResponse();
            await GetTodosAsync();
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
