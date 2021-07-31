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
    public class AssignGroupThenSynPartNumberUseCase : IAssignGroupThenSynPartNumberUseCase
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IInventorService _inventorService;
        private readonly IPrototypeRepository _prototypeRepository;

        public AssignGroupThenSynPartNumberUseCase(IInventorService inventorService,
            IPrototypeRepository prototypeRepository,
            IGroupRepository groupRepository)
        {
            _inventorService = inventorService;
            _prototypeRepository = prototypeRepository;
            _groupRepository = groupRepository;
        }

        public async Task Execute(int prototypeId, int groupId)
        {
            var prototype = await _prototypeRepository.GetByIdAsync(prototypeId);
            if (prototype == null) throw new ResourceNotFoundException("族原型不存在。");

            var group = await _groupRepository.GetByIdAsync(groupId);
            if (group == null) throw new ResourceNotFoundException("分组不存在。");

            // persist prototype
            prototype.Group = group;
            prototype.GroupModifiedAt = DateTime.Now;
            await _prototypeRepository.UpdateAsync(prototype);

            // update inventor
            var loadedFamily = _inventorService.GetFamiliesByInternalName(prototype.FamilyId).ToArray();
            foreach (var derivative in prototype.Derivatives)
            {
                // skip if not loaded
                if (loadedFamily.All(x => x.Library != derivative.LibraryId)) continue;

                // persist part
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

                // update part number
                _inventorService.EnsurePartNumberColumnCreated(derivative.FamilyId, derivative.LibraryId);
                _inventorService.SynchronizePartNumber(
                    prototype.Parts.ToDictionary(x => x.PartId, x => $"{prototype.Group.ShortName}{x.Id:D8}"),
                    derivative.FamilyId, derivative.LibraryId);
                derivative.SynchronizedAt = DateTime.Now;
            }

            // update synchronizedAt property
            await _prototypeRepository.UpdateAsync(prototype);
        }
    }
}