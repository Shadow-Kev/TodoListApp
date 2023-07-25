using Blazored.FluentValidation;
using BlazorHero.CleanArchitecture.Application.Features.Todos.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.Todos.Queries.GetAll;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.TodoListApp.Todo;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.TodoAppList
{
    public partial class AddOrEditTodoModal
    {
        [Inject] private ITodoManager TodoManager { get; set; }
        [Parameter] public AddEditTodoCommand AddOrEditTodoModel { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        private List<GetAllTodosResponse> _todoList = new();
        private DateTime _maxDate = new DateTime(DateTime.Today.Year, 12, 31);
        
        private void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            var result = await TodoManager.SaveAsync(AddOrEditTodoModel);
            if (result.Succeeded)
            {
                _snackBar.Add(result.Messages[0], Severity.Success);
                MudDialog.Close();
            }
            else
            {
                foreach(var message in result.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
        }
        private async Task LoadDataAsync()
        {
            await Task.CompletedTask;
        }
    }
}
