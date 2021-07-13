using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.Core.DTOs;

namespace JetSnail.InventorLibraryManager.UseCase.UseCases
{
    public interface IGetGroupsUseCase
    {
        Task<GroupDto[]> Execute();
    }
}