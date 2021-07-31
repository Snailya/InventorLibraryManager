using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.UseCase.FamilyScope.DTOs;

namespace JetSnail.InventorLibraryManager.UseCase.FamilyScope
{
    /// <summary>
    ///     将离线产生的衍生族记录至数据库，并更新PartMeta。
    /// </summary>
    public interface ISaveDerivativeThenSynPartNumberUseCase
    {
        Task<DerivativeFamilyDto> Execute(int prototypeId, string libraryId);
    }
}