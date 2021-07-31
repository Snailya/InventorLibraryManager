using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.UseCase.GroupScope.DTOs;

namespace JetSnail.InventorLibraryManager.UseCase.GroupScope
{
    /// <summary>
    ///     创建分组
    /// </summary>
    public interface IAddGroupUseCase
    {
        Task<GroupDto> Execute(string name, string shortName);
    }
}