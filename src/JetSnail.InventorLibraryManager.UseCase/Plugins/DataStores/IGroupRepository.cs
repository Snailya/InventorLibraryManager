using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.Core.Entities;

namespace JetSnail.InventorLibraryManager.UseCase.Plugins.DataStores
{
    public interface IGroupRepository
    {
        GroupEntity[] GetAll();
        Task<GroupEntity[]> GetAllAsync();

        Task<GroupEntity> GetByIdAsync(int id);

        Task<GroupEntity> InsertAsync(GroupEntity groupEntity);

        Task UpdateAsync(GroupEntity obj);

        Task DeleteAsync(int id);
    }
}