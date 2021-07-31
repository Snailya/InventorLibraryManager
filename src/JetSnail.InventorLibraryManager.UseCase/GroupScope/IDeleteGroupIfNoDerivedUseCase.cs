using System.Threading.Tasks;

namespace JetSnail.InventorLibraryManager.UseCase.GroupScope
{
    public interface IDeleteGroupIfNoDerivedUseCase
    {
        Task Execute(int id);
    }
}