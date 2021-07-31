using System;
using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.UseCase.GroupScope;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace JetSnail.InventorLibraryManager.Client.Validators
{
    public class GroupValidator : ComponentBase
    {
        private ValidationMessageStore _messageStore;
        [Inject] public ICheckNameUseCase CheckNameUseCase { get; set; }
        [Parameter] public string CurrentShortName { get; set; }
        [CascadingParameter] private EditContext CurrentEditContext { get; set; }

        protected override void OnInitialized()
        {
            if (CurrentEditContext == null)
                throw new InvalidOperationException(
                    $"{nameof(GroupValidator)} requires a cascading " +
                    $"parameter of type {nameof(EditContext)}. " +
                    $"For example, you can use {nameof(GroupValidator)} " +
                    $"inside an {nameof(EditForm)}.");

            _messageStore = new ValidationMessageStore(CurrentEditContext);

            CurrentEditContext.OnValidationRequested += async (o, args) => await ValidateName(o, args);

            CurrentEditContext.OnFieldChanged += (s, e) =>
            {
                _messageStore.Clear(e.FieldIdentifier);
                CurrentEditContext.Validate();
            };
        }

        private async Task ValidateName(object sender, ValidationRequestedEventArgs e)
        {
            if (CurrentEditContext.Model is IGroupShortName viewModel)
            {
                var (result, message) =
                    await CheckNameUseCase.Execute(viewModel.ShortName, CurrentShortName);
                if (!result)
                {
                    _messageStore.Add(CurrentEditContext.Field(nameof(IGroupShortName.ShortName)), message);
                    CurrentEditContext.NotifyValidationStateChanged();
                }
            }
        }
    }
}