using System.Linq;
using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.Server.Exceptions;
using JetSnail.InventorLibraryManager.UseCase.FamilyScope;
using JetSnail.InventorLibraryManager.UseCase.Plugins.DataStores;
using JetSnail.InventorLibraryManager.UseCase.Plugins.Software;

namespace JetSnail.InventorLibraryManager.Server.UseCases.FamilyScope
{
    public class DeleteDerivativeUseCase : IDeleteDerivativeUseCase
    {
        private readonly IInventorService _inventorService;
        private readonly IPrototypeRepository _prototypeRepository;

        public DeleteDerivativeUseCase(IInventorService inventorService,
            IPrototypeRepository prototypeRepository)
        {
            _inventorService = inventorService;
            _prototypeRepository = prototypeRepository;
        }

        public async Task Execute(int prototypeId, string libraryId)
        {
            var prototype = await _prototypeRepository.GetByIdAsync(prototypeId);
            if (prototype == null) throw new ResourceNotFoundException("族原型不存在。");

            if (_inventorService.GetFamilyByInternalNames(prototype.FamilyId, libraryId) == null)
                throw new ResourceNotFoundException("未找到族。");

            _inventorService.DeleteFamily(prototype.FamilyId, libraryId);

            if (prototype.Derivatives.SingleOrDefault(e => e.LibraryId == libraryId) is { } derivative)
            {
                prototype.Derivatives.Remove(derivative);
                await _prototypeRepository.UpdateAsync(prototype);
            }
        }
    }
}