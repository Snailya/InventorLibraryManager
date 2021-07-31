using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.UseCase.FamilyScope.DTOs;

namespace JetSnail.InventorLibraryManager.UseCase
{
    public interface IDashboardHub
    {
        Task HandleSyncPrototypeProgress(ProgressDto dto);
    }
}