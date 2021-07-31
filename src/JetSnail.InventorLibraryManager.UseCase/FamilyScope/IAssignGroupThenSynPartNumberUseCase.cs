using System.Threading.Tasks;

namespace JetSnail.InventorLibraryManager.UseCase.FamilyScope
{
    /// <summary>
    ///     为原型指定GroupMeta,并更新衍生族的PartMeta
    /// </summary>
    public interface IAssignGroupThenSynPartNumberUseCase
    {
        Task Execute(int prototypeId, int groupId);
    }
}