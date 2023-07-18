﻿using Blazored.FluentValidation;
using BlazorHero.CleanArchitecture.Application.Features.Todos.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Client.Extensions;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.TodoListApp.Todo;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.TodoAppList
{
    public partial class AddEditTodoModal
    {
        [Inject] private ITodoManager TodoManager { get; set; }
        [Parameter] public AddEditTodoCommand AddEditTodoModel { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get ; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options =>
        {
            options.IncludeAllRuleSets();
        });

        public void Cancel()
        {
            MudDialog.Cancel();
        }
        private async Task SaveAsync()
        {
            var response = await TodoManager.SaveAsync(AddEditTodoModel);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], Severity.Success);
                MudDialog.Close();
            } 
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
            await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
            HubConnection = HubConnection.TryInitialize(_navigationManager, _localStorage);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task LoadDataAsync()
        {
            await Task.CompletedTask;
        }
    }
}
