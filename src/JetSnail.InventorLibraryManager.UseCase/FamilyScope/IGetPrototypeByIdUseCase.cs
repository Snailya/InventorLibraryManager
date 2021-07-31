using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.UseCase.FamilyScope.DTOs;

namespace JetSnail.InventorLibraryManager.UseCase.FamilyScope
{
    public interface IGetPrototypeByIdUseCase
    {
        Task<PrototypeFamilyDto> Execute(int id);
    }
}