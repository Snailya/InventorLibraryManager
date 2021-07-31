using AutoMapper;
using JetSnail.InventorLibraryManager.Client.ViewModels;
using JetSnail.InventorLibraryManager.UseCase.FamilyScope.DTOs;
using JetSnail.InventorLibraryManager.UseCase.GroupScope.DTOs;

namespace JetSnail.InventorLibraryManager.Client.Profiles
{
    public class FamilyMappingProfile : Profile
    {
        public FamilyMappingProfile()
        {
            CreateMap<DerivativeFamilyDto, DerivativeLineItemViewModel>()
                .ForMember(d=>d.LibraryId, opt=>opt.MapFrom(src=>src.Library.LibraryId))
                .ForMember(d => d.LibraryName, opt => opt.MapFrom(src=>src.Library.Name));
            CreateMap<GroupDto, DerivativeLineItemViewModel>()
                .ForMember(d => d.LibraryName, opt => opt.Ignore())
                .ForMember(d => d.CreatedAt, opt => opt.Ignore())
                .ForMember(d => d.SynchronizedAt, opt => opt.Ignore());

            CreateMap<PrototypeFamilyDto, PrototypeLineItemViewModel>()
                .ForMember(d => d.LibraryName, opt => opt.MapFrom(src=>src.Library.Name))
                .ForMember(d => d.GroupName, opt => opt.MapFrom(src=>src.Group.Name));
            CreateMap<GroupDto, PrototypeLineItemViewModel>()
                .ForMember(d => d.Description, opt => opt.Ignore())
                .ForMember(d => d.LibraryName, opt => opt.Ignore())
                .ForMember(d => d.GroupName, opt => opt.MapFrom(src => src.Name))
                .ForMember(d => d.Derivatives, opt => opt.Ignore());

            CreateMap<GroupDto, GroupSelectOptionViewModel>()
                .ForMember(d => d.IsDisabled, opt => opt.Ignore());
        }
    }
}