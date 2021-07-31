using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.UseCase.GroupScope.DTOs;

namespace JetSnail.InventorLibraryManager.UseCase.GroupScope
{
    public interface IGetGroupsUseCase
    {
        Task<GroupDto[]> Execute();
    }
}