using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.UseCase.GroupScope.DTOs;

namespace JetSnail.InventorLibraryManager.UseCase.GroupScope
{
    public interface IGetGroupByIdUseCase
    {
        Task<GroupDto> Execute(int id);
    }
}