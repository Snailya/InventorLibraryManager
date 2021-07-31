using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using JetSnail.InventorLibraryManager.UseCase.FamilyScope;
using JetSnail.InventorLibraryManager.UseCase.FamilyScope.DTOs;
using JetSnail.InventorLibraryManager.UseCase.Plugins.DataStores;
using JetSnail.InventorLibraryManager.UseCase.Plugins.Software;

namespace JetSnail.InventorLibraryManager.Server.UseCases.FamilyScope
{
    public class GetPrototypesUseCase : IGetPrototypesUseCase
    {
        private readonly IInventorService _inventorService;
        private readonly IMapper _mapper;
        private readonly IPrototypeRepository _prototypeRepository;

        public GetPrototypesUseCase(IMapper mapper, IInventorService inventorService,
            IPrototypeRepository prototypeRepository)
        {
            _mapper = mapper;
            _inventorService = inventorService;
            _prototypeRepository = prototypeRepository;
        }

        public async Task<PrototypeFamilyDto[]> Execute()
        {
            return (await _prototypeRepository.GetAllAsync()).Select(x => _mapper.Map<PrototypeFamilyDto>(
                x.WithInventorView(_inventorService.GetFamiliesByInternalName(x.FamilyId)))).ToArray();
        }
    }
}