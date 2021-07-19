using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.Core.DTOs;

namespace JetSnail.InventorLibraryManager.UseCase.UseCases
{
    public interface IGetFamilyUseCase
    {
        Task<FamilyDto> Execute(string id);
    }
}