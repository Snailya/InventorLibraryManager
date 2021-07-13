using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.Core.DTOs;

namespace JetSnail.InventorLibraryManager.UseCase.UseCases
{
    public interface IAddGroupUseCase
    {
        Task<GroupDto> Execute(string displayName, string shortName);
    }
}