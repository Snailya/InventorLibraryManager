using System.Collections.Generic;
using System.Threading.Tasks;
using AntDesign;
using JetSnail.InventorLibraryManager.Client.Controls;
using JetSnail.InventorLibraryManager.Client.ViewModels;
using JetSnail.InventorLibraryManager.UseCase.FamilyScope;

namespace JetSnail.InventorLibraryManager.Client.Pages
{
    public partial class Dashboard
    {
        #region Methods

        private async Task OnScanPrototypes()
        {
            var options = new DrawerOptions
            {
                Placement = "top",
                Title = "进度",
                MaskClosable = false,
                WrapClassName = nameof(Dashboard)
            };

            var drawerRef = await _drawer.CreateAsync<ProgressDrawer, ProgressDrawerViewModel, ProgressDrawerViewModel>(
                options,
                _viewModel.ProgressDrawer = new ProgressDrawerViewModel());

            drawerRef.OnClosed = _ =>
            {
                _viewModel.ProgressDrawer.Source.Cancel();
                return Task.CompletedTask;
            };

            await foreach (var _ in _viewModel.ScanPrototypes()) await drawerRef.UpdateConfigAsync();
        }

        #endregion

        #region Class

        public sealed class DashboardPageViewModel
        {
            private readonly IScanPrototypesUseCase _scanPrototypesUseCase;

            public DashboardPageViewModel(IScanPrototypesUseCase scanPrototypesUseCase)
            {
                _scanPrototypesUseCase = scanPrototypesUseCase;
            }

            public ProgressDrawerViewModel ProgressDrawer { get; set; }

            public async IAsyncEnumerable<Task> ScanPrototypes()
            {
                await foreach (var dto in _scanPrototypesUseCase.Execute(ProgressDrawer.Source.Token))
                {
                    ProgressDrawer.Logs.Add(dto.Message);
                    ProgressDrawer.Percent = dto.Percent;
                    yield return Task.CompletedTask;
                }
            }
        }

        #endregion
    }
}