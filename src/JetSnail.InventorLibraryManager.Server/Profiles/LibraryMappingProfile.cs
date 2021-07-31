using AutoMapper;
using JetSnail.InventorLibraryManager.Core.InventorModels;
using JetSnail.InventorLibraryManager.UseCase.LibraryScope.DTOs;

namespace JetSnail.InventorLibraryManager.Server.Profiles
{
    public class LibraryMappingProfile : Profile
    {
        public LibraryMappingProfile()
        {
            CreateMap<InventorLibrary, LibraryDto>()
                .ForMember(d => d.LibraryId, opt => opt.MapFrom(src => src.InternalName))
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.DisplayName))
                .ForMember(d => d.IsReadOnly, opt => opt.MapFrom(src => src.ReadOnly));
        }
    }
}