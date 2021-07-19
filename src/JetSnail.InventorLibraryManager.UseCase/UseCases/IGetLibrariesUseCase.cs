using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.Core.DTOs;

namespace JetSnail.InventorLibraryManager.UseCase.UseCases
{
    public interface IGetLibrariesUseCase
    {
        Task<LibraryDto[]> Execute();
    }
}