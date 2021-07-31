using System.Threading.Tasks;

namespace JetSnail.InventorLibraryManager.UseCase.GroupScope
{
    /// <summary>
    ///     检查ShortName
    /// </summary>
    public interface ICheckNameUseCase
    {
        Task<(bool, string)> Execute(string newValue, string oldValue = null);
    }
}