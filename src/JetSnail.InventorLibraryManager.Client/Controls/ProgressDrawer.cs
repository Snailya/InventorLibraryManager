using JetSnail.InventorLibraryManager.Client.ViewModels;

namespace JetSnail.InventorLibraryManager.Client.Controls
{
    public partial class ProgressDrawer
    {
        #region Properties

        private ProgressDrawerViewModel _viewModel;

        #endregion

        #region Methods

        protected override void OnInitialized()
        {
            _viewModel = Options;
        }

        #endregion
    }
}