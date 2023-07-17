using BlazorHero.CleanArchitecture.Application.Features.Todos.Queries.GetAll;
using BlazorHero.CleanArchitecture.Client.Extensions;
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
        private bool _canViewTodos;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canSearchTodos = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Todos.Search)).Succeeded;
            _canViewTodos = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Todos.View)).Succeeded;

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
