using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using JetSnail.InventorLibraryManager.Core.Entities;
using JetSnail.InventorLibraryManager.UseCase.FamilyScope;
using JetSnail.InventorLibraryManager.UseCase.FamilyScope.DTOs;
using JetSnail.InventorLibraryManager.UseCase.Plugins.DataStores;
using JetSnail.InventorLibraryManager.UseCase.Plugins.Software;

namespace JetSnail.InventorLibraryManager.Server.UseCases.FamilyScope
{
    public class CreateDerivativeThenSynPartNumberUseCase : ICreateDerivativeThenSynPartNumberUseCase
    {
        private readonly IInventorService _inventorService;
        private readonly IMapper _mapper;
        private readonly IPrototypeRepository _prototypeRepository;

        public CreateDerivativeThenSynPartNumberUseCase(IMapper mapper, IInventorService inventorService,
            IPrototypeRepository prototypeRepository)
        {
            _mapper = mapper;
            _inventorService = inventorService;
            _prototypeRepository = prototypeRepository;
        }

        public async Task<DerivativeFamilyDto> Execute(int prototypeId, string toLibraryId)
        {
            // check if prototype exsit
            var prototype = await _prototypeRepository.GetByIdAsync(prototypeId);
            if (prototype == null) throw new InvalidOperationException("族原型不存在。");

            // create in inventor
            var invFamily = _inventorService.CopyFamily(prototype.FamilyId, prototype.LibraryId, toLibraryId);

            // persist in database if not tracked before
            var derivative = prototype.Derivatives.SingleOrDefault(x => x.LibraryId == toLibraryId);
            if (derivative == null)
            {
                // persist in derived
                derivative = new DerivativeEntity
                    {FamilyId = invFamily.InternalName, LibraryId = invFamily.Library, Prototype = prototype};
                prototype.Derivatives.Add(derivative);
                await _prototypeRepository.UpdateAsync(prototype);
            }

            // persist part metadata
            var familyTable =
                _inventorService.GetFamilyTableByInternalNames(invFamily.InternalName, invFamily.Library);
            foreach (var row in familyTable.Rows.Rows.Where(row =>
                prototype.Parts.All(e => e.PartId != row.InternalName)))
                prototype.Parts.Add(new PartEntity
                {
                    FamilyId = invFamily.InternalName,
                    PartId = row.InternalName
                });

            await _prototypeRepository.UpdateAsync(prototype);

            if (prototype.Group == null) return _mapper.Map<DerivativeFamilyDto>(derivative);

            // update part number if has group
            _inventorService.EnsurePartNumberColumnCreated(derivative.FamilyId, derivative.LibraryId);
            _inventorService.SynchronizePartNumber(
                prototype.Parts.ToDictionary(x => x.PartId, x => $"{prototype.Group.ShortName}{x.Id:D8}"),
                derivative.FamilyId, derivative.LibraryId);
            derivative.SynchronizedAt = DateTime.Now;
            await _prototypeRepository.UpdateAsync(prototype);

            return _mapper.Map<DerivativeFamilyDto>(derivative);
        }
    }
}