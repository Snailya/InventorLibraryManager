using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.UseCase.GroupScope.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace JetSnail.InventorLibraryManager.UseCase.GroupScope
{
    /// <summary>
    ///     修改GroupMeta
    /// </summary>
    public interface IUpdateGroupThenUpdatePartNumberIfShortNameChangeUseCase
    {
        Task Execute(int id, JsonPatchDocument<GroupDto> doc);
    }
}