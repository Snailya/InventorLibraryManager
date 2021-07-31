using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.UseCase.FamilyScope.DTOs;

namespace JetSnail.InventorLibraryManager.UseCase.FamilyScope
{
    /// <summary>
    ///     返回平铺的当前加载的全部Family信息，供前端合并后显示。注意此方法会自动在数据库中记录原型,但不记录原型对应的PartMeta。
    /// </summary>
    public interface IGetPrototypesUseCase
    {
        Task<PrototypeFamilyDto[]> Execute();
    }
}