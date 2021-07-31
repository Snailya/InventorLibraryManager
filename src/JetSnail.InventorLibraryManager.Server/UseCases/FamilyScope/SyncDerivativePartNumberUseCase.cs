using System;
using System.Linq;
using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.Core.Entities;
using JetSnail.InventorLibraryManager.Server.Exceptions;
using JetSnail.InventorLibraryManager.UseCase.FamilyScope;
using JetSnail.InventorLibraryManager.UseCase.Plugins.DataStores;
using JetSnail.InventorLibraryManager.UseCase.Plugins.Software;

namespace JetSnail.InventorLibraryManager.Server.UseCases.FamilyScope
{
    public class SyncDerivativePartNumberUseCase : ISyncDerivativePartNumberUseCase
    {
        private readonly IInventorService _inventorService;
        private readonly IPrototypeRepository _prototypeRepository;

        public SyncDerivativePartNumberUseCase(
            IPrototypeRepository prototypeRepository,
            IInventorService inventorService)
        {
            _prototypeRepository = prototypeRepository;
            _inventorService = inventorService;
        }

        public async Task Execute(int prototypeId, string libraryId)
        {
            if (string.IsNullOrEmpty(libraryId)) throw new ArgumentNullException(nameof(libraryId), "不能为空");

            var prototype = await _prototypeRepository.GetByIdAsync(prototypeId);
            if (prototype == null) throw new ResourceNotFoundException("族原型不存在。");

            if (prototype.Group == null) return;

            if (prototype.Derivatives.SingleOrDefault(x => x.LibraryId == libraryId) is not { } derivative)
                throw new ResourceNotFoundException("派生族不存在。");

            // persist part metadata
            var familyTable =
                _inventorService.GetFamilyTableByInternalNames(derivative.FamilyId, derivative.LibraryId);
            foreach (var row in familyTable.Rows.Rows.Where(row =>
                prototype.Parts.All(e => e.PartId != row.InternalName)))
                prototype.Parts.Add(new PartEntity
                {
                    FamilyId = derivative.FamilyId,
                    PartId = row.InternalName
                });

            await _prototypeRepository.UpdateAsync(prototype);

            _inventorService.EnsurePartNumberColumnCreated(prototype.FamilyId, libraryId);
            _inventorService.SynchronizePartNumber(
                prototype.Parts.ToDictionary(x => x.PartId, x => $"{prototype.Group.ShortName}{x.Id:D8}"),
                prototype.FamilyId, libraryId);

            derivative.SynchronizedAt = DateTime.Now;
            await _prototypeRepository.UpdateAsync(prototype);
        }
    }
}