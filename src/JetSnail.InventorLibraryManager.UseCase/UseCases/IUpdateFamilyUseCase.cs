using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.Core.DTOs;

namespace JetSnail.InventorLibraryManager.UseCase.UseCases
{
    public interface IUpdateFamilyUseCase
    {
        Task<FamilyDto> Execute(string familyId, string libraryId, int groupId);
    }
}