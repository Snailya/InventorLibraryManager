using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.Core.DTOs;

namespace JetSnail.InventorLibraryManager.UseCase.UseCases
{
    public interface IUpdateGroupUseCase
    {
        Task<GroupDto> Execute(int id, string displayName, string shortName);
    }
}