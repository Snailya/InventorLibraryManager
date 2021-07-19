using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.Core.DTOs;

namespace JetSnail.InventorLibraryManager.UseCase.UseCases
{
    public interface IUpdateFamilyPartUseCase
    {
        Task<PartDto> Execute(string partId, string familyId, string libraryId);
    }
}