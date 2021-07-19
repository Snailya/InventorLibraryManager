using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.Core.DTOs;

namespace JetSnail.InventorLibraryManager.UseCase.UseCases
{
    public interface IGetFamilyPartsUseCase
    {
        Task<PartDto[]> Execute(string familyId, string libraryId);
    }
}