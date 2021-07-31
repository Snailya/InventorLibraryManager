using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.UseCase.FamilyScope.DTOs;

namespace JetSnail.InventorLibraryManager.UseCase.FamilyScope
{
    /// <summary>
    ///     从原型生成衍生族到指定的库
    /// </summary>
    public interface ICreateDerivativeThenSynPartNumberUseCase
    {
        Task<DerivativeFamilyDto> Execute(int prototypeId, string toLibraryId);
    }
}