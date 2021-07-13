using System.Collections.Generic;
using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.Core.Domains;

namespace JetSnail.InventorLibraryManager.UseCase.DataStores
{
    public interface IGroupRepository
    {
        Task<IEnumerable<Group>> GetAllAsync();

        Task<Group> GetByIdAsync(int id);

        Task<Group> InsertAsync(Group obj);

        Task UpdateAsync(Group obj);

        Task DeleteAsync(int id);
    }
}