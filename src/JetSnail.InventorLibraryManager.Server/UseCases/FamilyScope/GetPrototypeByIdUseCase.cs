using System.Threading.Tasks;
using AutoMapper;
using JetSnail.InventorLibraryManager.Server.Exceptions;
using JetSnail.InventorLibraryManager.UseCase.FamilyScope;
using JetSnail.InventorLibraryManager.UseCase.FamilyScope.DTOs;
using JetSnail.InventorLibraryManager.UseCase.Plugins.DataStores;
using JetSnail.InventorLibraryManager.UseCase.Plugins.Software;

namespace JetSnail.InventorLibraryManager.Server.UseCases.FamilyScope
{
    public class GetPrototypeByIdUseCase : IGetPrototypeByIdUseCase
    {
        private readonly IInventorService _inventorService;
        private readonly IMapper _mapper;
        private readonly IPrototypeRepository _prototypeRepository;

        public GetPrototypeByIdUseCase(IMapper mapper, IInventorService inventorService,
            IPrototypeRepository prototypeRepository)
        {
            _mapper = mapper;
            _inventorService = inventorService;
            _prototypeRepository = prototypeRepository;
        }

        public async Task<PrototypeFamilyDto> Execute(int id)
        {
            var prototype = await _prototypeRepository.GetByIdAsync(id);
            if (prototype == null) throw new ResourceNotFoundException();

            return _mapper.Map<PrototypeFamilyDto>(
                prototype.WithInventorView(_inventorService.GetFamiliesByInternalName(prototype.FamilyId)));
        }
    }
}