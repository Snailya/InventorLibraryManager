using System.Threading.Tasks;
using AntDesign;
using JetSnail.InventorLibraryManager.Client.ViewModels;

namespace JetSnail.InventorLibraryManager.Client.Controls
{
    public sealed partial class AssignGroupSelect
    {
        #region Properties

        private readonly AssignGroupSelectViewModel _viewModel = new();

        #endregion

        public sealed class AssignGroupSelectViewModel
        {
            public GroupSelectOptionViewModel[] Options { get; set; }

            public int SelectedValue { get; set; }
        }

        #region Methods

        protected override void OnInitialized()
        {
            _viewModel.Options = Options;
        }

        public override async Task OnFeedbackOkAsync(ModalClosingEventArgs args)
        {
            if (_viewModel.SelectedValue == 0) args.Cancel = true;

            await OkCancelRefWithResult!.OnOk(_viewModel.SelectedValue);
            await base.OnFeedbackOkAsync(args);
        }

        #endregion
    }
}