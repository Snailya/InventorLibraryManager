using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using JetSnail.InventorLibraryManager.Core.Entities;
using JetSnail.InventorLibraryManager.Server.Exceptions;
using JetSnail.InventorLibraryManager.UseCase.FamilyScope;
using JetSnail.InventorLibraryManager.UseCase.FamilyScope.DTOs;
using JetSnail.InventorLibraryManager.UseCase.Plugins.DataStores;
using JetSnail.InventorLibraryManager.UseCase.Plugins.Software;

namespace JetSnail.InventorLibraryManager.Server.UseCases.FamilyScope
{
    /// <summary>
    ///     弃用
    /// </summary>
    public class SaveDerivativeThenSynPartNumberUseCase : ISaveDerivativeThenSynPartNumberUseCase
    {
        private readonly IInventorService _inventorService;
        private readonly IMapper _mapper;
        private readonly IPrototypeRepository _prototypeRepository;

        public SaveDerivativeThenSynPartNumberUseCase(
            IMapper mapper,
            IPrototypeRepository prototypeRepository,
            IInventorService inventorService)
        {
            _mapper = mapper;
            _prototypeRepository = prototypeRepository;
            _inventorService = inventorService;
        }

        public async Task<DerivativeFamilyDto> Execute(int prototypeId, string libraryId)
        {
            var prototype = await _prototypeRepository.GetByIdAsync(prototypeId);
            if (prototype == null) throw new ResourceNotFoundException("族原型不存在。");

            // validate existence of derivative
            if (_inventorService.GetFamilyByInternalNames(prototype.FamilyId, libraryId) == null)
                throw new ResourceNotFoundException("派生族不存在");

            // persist derived family if not exist
            var derivative = prototype.Derivatives.SingleOrDefault(x => x.LibraryId == libraryId);
            if (derivative == null)
            {
                derivative = new DerivativeEntity
                    {FamilyId = prototype.FamilyId, LibraryId = libraryId, Prototype = prototype};
                prototype.Derivatives.Add(derivative);
                await _prototypeRepository.UpdateAsync(prototype);
            }

            // persist part metadata
            var familyTable = _inventorService.GetFamilyTableByInternalNames(derivative.FamilyId, libraryId);
            foreach (var row in familyTable.Rows.Rows.Where(row =>
                prototype.Parts.All(e => e.PartId != row.InternalName)))
                prototype.Parts.Add(new PartEntity
                {
                    FamilyId = derivative.FamilyId,
                    PartId = row.InternalName
                });

            await _prototypeRepository.UpdateAsync(prototype);

            if (prototype.Group == null) return _mapper.Map<DerivativeFamilyDto>(derivative);

            // update part number if has group
            _inventorService.EnsurePartNumberColumnCreated(prototype.FamilyId, libraryId);
            _inventorService.SynchronizePartNumber(
                prototype.Parts.ToDictionary(x => x.PartId, x => $"{prototype.Group.ShortName}{x.Id:D8}"),
                prototype.FamilyId, libraryId);

            derivative.SynchronizedAt = DateTime.Now;
            await _prototypeRepository.UpdateAsync(prototype);

            return _mapper.Map<DerivativeFamilyDto>(derivative);
        }
    }
}