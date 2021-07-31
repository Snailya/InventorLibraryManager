using System.Threading.Tasks;

namespace JetSnail.InventorLibraryManager.UseCase.FamilyScope
{
    public interface ISyncDerivativePartNumberUseCase
    {
        Task Execute(int prototypeId, string libraryId);
    }
}