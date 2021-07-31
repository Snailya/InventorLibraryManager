using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using JetSnail.InventorLibraryManager.Core.Entities;
using JetSnail.InventorLibraryManager.Server.Exceptions;
using JetSnail.InventorLibraryManager.UseCase.GroupScope;
using JetSnail.InventorLibraryManager.UseCase.GroupScope.DTOs;
using JetSnail.InventorLibraryManager.UseCase.Plugins.DataStores;
using JetSnail.InventorLibraryManager.UseCase.Plugins.Software;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace JetSnail.InventorLibraryManager.Server.UseCases.GroupScope
{
    public class
        UpdateGroupThenUpdatePartNumberIfShortNameChangeUseCase :
            IUpdateGroupThenUpdatePartNumberIfShortNameChangeUseCase
    {
        private readonly ICheckNameUseCase _checkNameUseCase;
        private readonly IGroupRepository _groupRepository;
        private readonly IInventorService _inventorService;
        private readonly IMapper _mapper;

        public UpdateGroupThenUpdatePartNumberIfShortNameChangeUseCase(IMapper mapper, IGroupRepository groupRepository,
            ICheckNameUseCase checkNameUseCase, IInventorService inventorService)
        {
            _mapper = mapper;
            _groupRepository = groupRepository;
            _checkNameUseCase = checkNameUseCase;
            _inventorService = inventorService;
        }

        public async Task Execute(int id, JsonPatchDocument<GroupDto> doc)
        {
            var group = await _groupRepository.GetByIdAsync(id);
            if (group == null) throw new ResourceNotFoundException();

            // check if there is short name change
            var shortNameOp = doc.Operations.SingleOrDefault(x =>
                x.OperationType == OperationType.Replace && x.path == $"/{nameof(GroupDto.ShortName)}");
            if (shortNameOp != null)
            {
                var (result, message) =
                    await _checkNameUseCase.Execute((string) shortNameOp.value, shortNameOp.from);
                if (!result) throw new InvalidOperationException($"无法修改Group:{message}");
            }

            // persist group
            _mapper.Map<JsonPatchDocument<GroupEntity>>(doc).ApplyTo(group);
            await _groupRepository.UpdateAsync(group);

            // update inventor
            if (shortNameOp != null)
                foreach (var prototype in group.Prototypes)
                    // update inventor
                foreach (var derivative in prototype
                    .WithInventorView(_inventorService.GetFamiliesByInternalName(prototype.FamilyId)).Derivatives)
                {
                    // persist part metadata
                    var familyTable =
                        _inventorService.GetFamilyTableByInternalNames(derivative.FamilyId, derivative.LibraryId);
                    foreach (var row in familyTable.Rows.Rows.Where(row =>
                        prototype.Parts.All(e => e.PartId != row.InternalName)))
                        prototype.Parts.Add(new PartEntity
                        {
                            FamilyId = prototype.FamilyId,
                            PartId = row.InternalName
                        });

                    // update inventor
                    _inventorService.EnsurePartNumberColumnCreated(derivative.FamilyId, derivative.LibraryId);
                    _inventorService.SynchronizePartNumber(
                        prototype.Parts.ToDictionary(x => x.PartId, x => $"{prototype.Group.ShortName}{x.Id:D8}"),
                        derivative.FamilyId, derivative.LibraryId);
                    derivative.SynchronizedAt = DateTime.Now;
                }

            await _groupRepository.UpdateAsync(group);
        }
    }
}