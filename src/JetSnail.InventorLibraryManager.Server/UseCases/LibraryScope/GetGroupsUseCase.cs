using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using JetSnail.InventorLibraryManager.UseCase.LibraryScope;
using JetSnail.InventorLibraryManager.UseCase.LibraryScope.DTOs;
using JetSnail.InventorLibraryManager.UseCase.Plugins.Software;

namespace JetSnail.InventorLibraryManager.Server.UseCases.LibraryScope
{
    public class GetLibrariesUseCase : IGetLibrariesUseCase
    {
        private readonly IInventorService _inventorService;
        private readonly IMapper _mapper;

        public GetLibrariesUseCase(IMapper mapper, IInventorService inventorService)
        {
            _mapper = mapper;
            _inventorService = inventorService;
        }

        Task<LibraryDto[]> IGetLibrariesUseCase.Execute()
        {
            return Task.FromResult(_inventorService.GetServerLibraries().Select(x => _mapper.Map<LibraryDto>(x))
                .ToArray());
        }
    }
}