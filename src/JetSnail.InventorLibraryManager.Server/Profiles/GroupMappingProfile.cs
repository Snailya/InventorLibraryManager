using AutoMapper;
using JetSnail.InventorLibraryManager.Core.Entities;
using JetSnail.InventorLibraryManager.UseCase.GroupScope.DTOs;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace JetSnail.InventorLibraryManager.Server.Profiles
{
    public class GroupMappingProfile : Profile
    {
        public GroupMappingProfile()
        {
            CreateMap<GroupEntity, GroupDto>();
            CreateMap<PrototypeFamilyEntity, GroupPrototypeDto>()
                .AfterMap<FillFamilyInfoAction>();

            CreateMap<JsonPatchDocument<GroupDto>, JsonPatchDocument<GroupEntity>>();
            CreateMap<Operation<GroupDto>, Operation<GroupEntity>>();
        }
    }
}