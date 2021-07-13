using System.Collections.Generic;
using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.Core.Domains;

namespace JetSnail.InventorLibraryManager.UseCase.DataStores
{
    public interface ILibraryRepository
    {
        Task<IEnumerable<Library>> GetAllAsync();
        Task<Library> GetByIdAsync(string id);
    }
}