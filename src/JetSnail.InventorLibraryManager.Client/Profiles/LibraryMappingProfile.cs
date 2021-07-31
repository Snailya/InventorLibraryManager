using AutoMapper;
using JetSnail.InventorLibraryManager.Client.ViewModels;
using JetSnail.InventorLibraryManager.UseCase.LibraryScope.DTOs;

namespace JetSnail.InventorLibraryManager.Client.Profiles
{
    public class LibraryMappingProfile : Profile
    {
        public LibraryMappingProfile()
        {
            CreateMap<LibraryDto, LibrarySelectOptionViewModel>();
        }
    }
}