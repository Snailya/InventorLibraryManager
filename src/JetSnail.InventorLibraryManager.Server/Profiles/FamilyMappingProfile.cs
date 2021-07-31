using AutoMapper;
using JetSnail.InventorLibraryManager.Core.Entities;
using JetSnail.InventorLibraryManager.UseCase.FamilyScope.DTOs;

namespace JetSnail.InventorLibraryManager.Server.Profiles
{
    public class FamilyMappingProfile : Profile
    {
        public FamilyMappingProfile()
        {
            CreateMap<IFamily, BaseFamilyDto>()
                .IncludeAllDerived()
                .AfterMap<FillFamilyInfoAction>();
            CreateMap<DerivativeEntity, DerivativeFamilyDto>()
                .ForMember(d => d.Type, opt => opt.MapFrom(src => FamilyType.Derivative))
                .ForMember(d => d.CreatedAt, opt => opt.Condition(src => src.CreatedAt != default));
            CreateMap<GroupEntity, PrototypeGroupDto>();
            CreateMap<PrototypeFamilyEntity, PrototypeFamilyDto>()
                .ForMember(d => d.Type, opt => opt.MapFrom(src => FamilyType.Prototype));
        }
    }
}