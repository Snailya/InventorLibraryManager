using System.Threading.Tasks;
using AntDesign;
using JetSnail.InventorLibraryManager.Client.ViewModels;

namespace JetSnail.InventorLibraryManager.Client.Controls
{
    public sealed partial class AddOrEditGroupModal
    {
        #region Properties

        private AddOrEditGroupShortNameModalViewModel _viewModel;
        private Form<AddOrEditGroupShortNameModalViewModel> _form;

        #endregion

        #region Methods

        protected override async Task OnInitializedAsync()
        {
            // 不修改传入的变量
            _viewModel = _mapper.Map<AddOrEditGroupShortNameModalViewModel>(Options);
            await base.OnInitializedAsync();
        }

        public override async Task OnFeedbackOkAsync(ModalClosingEventArgs args)
        {
            if (!_form.Validate())
            {
                args.Cancel = true;
            }
            else
            {
                await OkCancelRefWithResult!.OnOk(_viewModel);
                await base.OnFeedbackOkAsync(args);
            }
        }

        #endregion
    }
}