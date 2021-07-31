using AutoMapper;
using JetSnail.InventorLibraryManager.Core.Entities;
using JetSnail.InventorLibraryManager.Core.InventorModels;
using JetSnail.InventorLibraryManager.UseCase.FamilyScope.DTOs;
using JetSnail.InventorLibraryManager.UseCase.Plugins.Software;

namespace JetSnail.InventorLibraryManager.Server.Profiles
{
    public class FillFamilyInfoAction : IMappingAction<IFamily, BaseFamilyDto>
    {
        private readonly IInventorService _inventorService;
        private readonly IMapper _mapper;

        public FillFamilyInfoAction(IInventorService inventorService)
        {
            var config = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<InventorLibrary, FamilyLibraryDto>()
                        .ForMember(d => d.LibraryId, opt => opt.MapFrom(src => src.InternalName))
                        .ForMember(d => d.Name, opt => opt.MapFrom(src => src.DisplayName));
                });
            _mapper = config.CreateMapper();
            _inventorService = inventorService;
        }

        public void Process(IFamily source, BaseFamilyDto destination, ResolutionContext context)
        {
            // get display name from inventor
            var family = _inventorService.GetFamilyByInternalNames(source.FamilyId, source.LibraryId);
            if (family!=null)
            {
                destination.Description = family.Description;
                destination.Name = family.DisplayName;
            }
            
            // get library from inventor
            var library = _inventorService.GetLibraryByInternalName(source.LibraryId);
            destination.Library = _mapper.Map<FamilyLibraryDto>(library);
        }
    }
}