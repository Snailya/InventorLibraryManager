using System.Linq;
using System.Threading.Tasks;
using AntDesign;
using AutoMapper;
using JetSnail.InventorLibraryManager.Client.Controls;
using JetSnail.InventorLibraryManager.Client.ViewModels;
using JetSnail.InventorLibraryManager.UseCase.FamilyScope;
using JetSnail.InventorLibraryManager.UseCase.GroupScope;
using JetSnail.InventorLibraryManager.UseCase.GroupScope.DTOs;
using JetSnail.InventorLibraryManager.UseCase.LibraryScope;
using JetSnail.InventorLibraryManager.UseCase.LibraryScope.DTOs;

namespace JetSnail.InventorLibraryManager.Client.Pages
{
    public partial class Prototypes
    {
        #region Properteis

        private bool _isLoading;

        #endregion

        #region Class

        public sealed class PrototypePageViewModel
        {
            private readonly IAssignGroupThenSynPartNumberUseCase _assignGroupUseCase;
            private readonly ICreateDerivativeThenSynPartNumberUseCase _createDerivativeUseCase;
            private readonly IDeleteDerivativeUseCase _deleteDerivativeUseCase;
            private readonly IGetPrototypesUseCase _getFamiliesUseCase;
            private readonly IGetGroupsUseCase _getGroupsUseCase;
            private readonly IGetLibrariesUseCase _getLibrariesUseCase;
            private readonly IMapper _mapper;
            private readonly ISaveDerivativeThenSynPartNumberUseCase _saveDerivativeUseCase;

            public PrototypePageViewModel(IMapper mapper, IGetGroupsUseCase getGroupsUseCase,
                IGetLibrariesUseCase getLibrariesUseCase,
                IGetPrototypesUseCase getFamiliesUseCase,
                IAssignGroupThenSynPartNumberUseCase assignGroupUseCase,
                ICreateDerivativeThenSynPartNumberUseCase createDerivativeUseCase,
                ISaveDerivativeThenSynPartNumberUseCase saveDerivativeUseCase,
                IDeleteDerivativeUseCase deleteDerivativeUseCase)
            {
                _mapper = mapper;
                _getGroupsUseCase = getGroupsUseCase;
                _getLibrariesUseCase = getLibrariesUseCase;
                _getFamiliesUseCase = getFamiliesUseCase;
                _assignGroupUseCase = assignGroupUseCase;
                _createDerivativeUseCase = createDerivativeUseCase;
                _saveDerivativeUseCase = saveDerivativeUseCase;
                _deleteDerivativeUseCase = deleteDerivativeUseCase;
            }

            public LibraryDto[] LibraryCaches { get; set; }
            public GroupDto[] GroupCaches { get; set; }
            public PrototypeLineItemViewModel[] Prototypes { get; set; }

            public async Task InitializedAsync()
            {
                LibraryCaches = await _getLibrariesUseCase.Execute();
                GroupCaches = await _getGroupsUseCase.Execute();

                Prototypes = (await _getFamiliesUseCase.Execute())
                    .Select(d => _mapper.Map<PrototypeLineItemViewModel>(d)).ToArray();
            }

            public async Task UpdateGroup(PrototypeLineItemViewModel prototype, int groupId)
            {
                await _assignGroupUseCase.Execute(prototype.Id, groupId);
                prototype.GroupName = GroupCaches.Single(x => x.Id == groupId).Name;
            }

            public async Task CreateDerivative(PrototypeLineItemViewModel prototype, string libraryId)
            {
                var derivative =
                    _mapper.Map<DerivativeLineItemViewModel>(
                        await _createDerivativeUseCase.Execute(prototype.Id, libraryId));
                prototype.Derivatives.Insert(0, derivative);
            }

            public async Task DeleteDerivative(PrototypeLineItemViewModel prototype,
                DerivativeLineItemViewModel derivative)
            {
                await _deleteDerivativeUseCase.Execute(prototype.Id, derivative.LibraryId);
                prototype.Derivatives.Remove(derivative);
            }

            public async Task SaveDerivative(PrototypeLineItemViewModel prototype,
                DerivativeLineItemViewModel derivative)
            {
                derivative =
                    _mapper.Map<DerivativeLineItemViewModel>(
                        await _saveDerivativeUseCase.Execute(prototype.Id, derivative.LibraryId));
            }

            public async Task UpdateDerivative(PrototypeLineItemViewModel prototype,
                DerivativeLineItemViewModel derivative)
            {
                derivative =
                    _mapper.Map<DerivativeLineItemViewModel>(
                        await _saveDerivativeUseCase.Execute(prototype.Id, derivative.LibraryId));
            }
        }

        #endregion

        #region Methods

        protected override async Task OnInitializedAsync()
        {
            _isLoading = true;
            await _viewModel.InitializedAsync();
            _isLoading = false;
        }

        private async Task OnAssignGroup(PrototypeLineItemViewModel context)
        {
            var options = _viewModel.GroupCaches.Select(x => _mapper.Map<GroupSelectOptionViewModel>(x)).ToArray();
            options.ForEach(x => { x.IsDisabled = x.Name == context.GroupName; });
            var modalRef =
                await _modal.CreateModalAsync<AssignGroupSelect, GroupSelectOptionViewModel[], int>(
                    new ModalOptions
                    {
                        Title = "指派",
                        MaskClosable = false
                    }, options);

            modalRef.OnOk = async result =>
            {
                _isLoading = true;
                await InvokeAsync(StateHasChanged);

                await _viewModel.UpdateGroup(context, result);

                _isLoading = false;
                await InvokeAsync(StateHasChanged);
            };
        }

        private async Task OnCreateDerivative(PrototypeLineItemViewModel context)
        {
            var options = _viewModel.LibraryCaches.Where(x => !x.IsReadOnly)
                .Select(x => _mapper.Map<LibrarySelectOptionViewModel>(x)).ToArray();
            options.ForEach(x => { x.IsDisabled = context.Derivatives.Any(y => y.LibraryId == x.LibraryId); });

            var modalRef =
                await _modal.CreateModalAsync<CreateDerivativeSelect, LibrarySelectOptionViewModel[], string>(
                    new ModalOptions
                    {
                        Title = "复制",
                        MaskClosable = false
                    },
                    options);

            modalRef.OnOk = async result =>
            {
                _isLoading = true;
                await InvokeAsync(StateHasChanged);

                await _viewModel.CreateDerivative(context, result);

                _isLoading = false;
                await InvokeAsync(StateHasChanged);
            };
        }

        private async Task OnSaveDerivative(PrototypeLineItemViewModel prototype,
            DerivativeLineItemViewModel derivative)
        {
            _isLoading = true;
            await _viewModel.SaveDerivative(prototype, derivative);
            _isLoading = false;
        }

        private async Task OnUpdateDerivative(PrototypeLineItemViewModel prototype,
            DerivativeLineItemViewModel derivative)
        {
            _isLoading = true;
            await _viewModel.UpdateDerivative(prototype, derivative);
            _isLoading = false;
        }

        private async Task OnDeleteDerivative(PrototypeLineItemViewModel prototype,
            DerivativeLineItemViewModel derivative)
        {
            _isLoading = true;
            await _viewModel.DeleteDerivative(prototype, derivative);
            _isLoading = false;
        }

        #endregion
    }
}