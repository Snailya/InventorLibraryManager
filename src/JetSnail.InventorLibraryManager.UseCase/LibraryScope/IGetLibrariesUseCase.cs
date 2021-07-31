using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.UseCase.LibraryScope.DTOs;

namespace JetSnail.InventorLibraryManager.UseCase.LibraryScope
{
    public interface IGetLibrariesUseCase
    {
        Task<LibraryDto[]> Execute();
    }
}