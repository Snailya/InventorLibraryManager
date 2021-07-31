using AutoMapper;
using JetSnail.InventorLibraryManager.Client.ViewModels;
using JetSnail.InventorLibraryManager.UseCase.GroupScope.DTOs;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace JetSnail.InventorLibraryManager.Client.Profiles
{
    public class GroupMappingProfile : Profile
    {
        public GroupMappingProfile()
        {
            CreateMap<GroupPrototypeDto, PrototypeLineItemViewModel>();
            CreateMap<GroupDto, GroupLineItemViewModel>().ReverseMap();
            CreateMap<GroupLineItemViewModel, AddOrEditGroupShortNameModalViewModel>().ReverseMap()
                .ForMember(d => d.Prototypes, opt => opt.Ignore());

            CreateMap<Operation<GroupLineItemViewModel>, Operation<GroupDto>>();
            CreateMap<JsonPatchDocument<GroupLineItemViewModel>, JsonPatchDocument<GroupDto>>();
        }
    }
}