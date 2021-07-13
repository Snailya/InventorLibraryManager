using System.Threading.Tasks;

namespace JetSnail.InventorLibraryManager.UseCase.UseCases
{
    public interface IDeleteGroupUseCase
    {
        Task<bool> Execute(int id);
    }
}