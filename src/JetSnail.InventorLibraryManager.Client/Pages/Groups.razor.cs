using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AntDesign;
using AutoMapper;
using JetSnail.InventorLibraryManager.Client.Controls;
using JetSnail.InventorLibraryManager.Client.ViewModels;
using JetSnail.InventorLibraryManager.UseCase.GroupScope;
using JetSnail.InventorLibraryManager.UseCase.GroupScope.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace JetSnail.InventorLibraryManager.Client.Pages
{
    public partial class Groups
    {
        #region Properties

        private bool _isLoading;

        #endregion

        #region Class

        public sealed class GroupPageViewModel
        {
            private readonly IAddGroupUseCase _addGroupUseCase;
            private readonly IDeleteGroupIfNoDerivedUseCase _deleteGroupUseCase;
            private readonly IGetGroupsUseCase _getGroupsUseCase;
            private readonly IMapper _mapper;
            private readonly IUpdateGroupThenUpdatePartNumberIfShortNameChangeUseCase _updateGroupUseCase;

            public GroupPageViewModel(IMapper mapper, IGetGroupsUseCase getGroupsUseCase,
                IAddGroupUseCase addGroupUseCase,
                IUpdateGroupThenUpdatePartNumberIfShortNameChangeUseCase updateGroupUseCase,
                IDeleteGroupIfNoDerivedUseCase deleteGroupUseCase)
            {
                _mapper = mapper;
                _getGroupsUseCase = getGroupsUseCase;
                _addGroupUseCase = addGroupUseCase;
                _updateGroupUseCase = updateGroupUseCase;
                _deleteGroupUseCase = deleteGroupUseCase;
            }

            public IList<GroupLineItemViewModel> Groups { get; set; }

            public async Task InitializedAsync()
            {
                var temp = await _getGroupsUseCase.Execute();
                Groups = (await _getGroupsUseCase.Execute()).Select(d => _mapper.Map<GroupLineItemViewModel>(d))
                    .ToList();
            }

            public async Task AddGroup(string name, string shortName)
            {
                var group = await _addGroupUseCase.Execute(name,
                    shortName);
                Groups.Insert(0, _mapper.Map<GroupLineItemViewModel>(group));
            }

            public async Task UpdateGroup(GroupLineItemViewModel group, JsonPatchDocument<GroupLineItemViewModel> doc)
            {
                await _updateGroupUseCase.Execute(group.Id, _mapper.Map<JsonPatchDocument<GroupDto>>(doc));
                doc.ApplyTo(group);
            }

            public async Task DeleteGroup(GroupLineItemViewModel item)
            {
                await _deleteGroupUseCase.Execute(item.Id);
                Groups.Remove(item);
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

        private async Task OnAddGroup()
        {
            var modalRef = await _modal
                .CreateModalAsync<AddOrEditGroupModal, AddOrEditGroupShortNameModalViewModel,
                    AddOrEditGroupShortNameModalViewModel>(
                    new ModalOptions
                    {
                        Title = "新建",
                        MaskClosable = false
                    }, new AddOrEditGroupShortNameModalViewModel());

            modalRef.OnOk = async result =>
            {
                _isLoading = true;
                await InvokeAsync(StateHasChanged);

                await _viewModel.AddGroup(result.Name, result.ShortName);

                _isLoading = false;
                await InvokeAsync(StateHasChanged);
            };
        }

        private async Task OnEditGroup(GroupLineItemViewModel context)
        {
            var modalRef = await _modal
                .CreateModalAsync<AddOrEditGroupModal, AddOrEditGroupShortNameModalViewModel,
                    AddOrEditGroupShortNameModalViewModel>(
                    new ModalOptions
                    {
                        Title = "编辑",
                        MaskClosable = false
                    }, _mapper.Map<AddOrEditGroupShortNameModalViewModel>(context));

            modalRef.OnOk = async result =>
            {
                _isLoading = true;
                await InvokeAsync(StateHasChanged);

                var patch = new JsonPatchDocument<GroupLineItemViewModel>();
                if (result.Name != context.Name) patch.Replace(p => p.Name, result.Name);
                if (result.ShortName != context.ShortName) patch.Replace(p => p.ShortName, result.ShortName);
                await _viewModel.UpdateGroup(context, patch);

                _isLoading = false;
                await InvokeAsync(StateHasChanged);
            };
        }

        private async Task OnDeleteGroup(GroupLineItemViewModel context)
        {
            _isLoading = true;

            try
            {
                await _viewModel.DeleteGroup(context);
                await _message.Success("删除成功");
            }
            catch (Exception e)
            {
                await _message.Error($"删除失败:{e.Message}");
            }
            finally
            {
                _isLoading = false;
            }
        }

        #endregion
    }
}