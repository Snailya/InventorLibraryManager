using System.Threading.Tasks;
using AntDesign;
using JetSnail.InventorLibraryManager.Client.ViewModels;

namespace JetSnail.InventorLibraryManager.Client.Controls
{
    public partial class CreateDerivativeSelect
    {
        #region Properties

        private readonly CreateDerivativeSelectViewModel _viewModel = new();

        #endregion

        public sealed class CreateDerivativeSelectViewModel
        {
            public LibrarySelectOptionViewModel[] Options { get; set; }
            public string SelectedValue { get; set; }
        }

        #region Methods

        protected override void OnInitialized()
        {
            _viewModel.Options = Options;
        }

        public override async Task OnFeedbackOkAsync(ModalClosingEventArgs args)
        {
            if (string.IsNullOrEmpty(_viewModel.SelectedValue)) args.Cancel = true;

            await OkCancelRefWithResult!.OnOk(_viewModel.SelectedValue);
            await base.OnFeedbackOkAsync(args);
        }

        #endregion
    }
}