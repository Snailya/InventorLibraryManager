using System.Threading.Tasks;

namespace JetSnail.InventorLibraryManager.UseCase.FamilyScope
{
    /// <summary>
    ///     删除衍生族
    /// </summary>
    public interface IDeleteDerivativeUseCase
    {
        Task Execute(int prototypeId, string libraryId);
    }
}