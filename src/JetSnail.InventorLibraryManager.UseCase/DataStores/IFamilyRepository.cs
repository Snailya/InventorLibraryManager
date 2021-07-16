using System.Collections.Generic;
using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.Core.Domains;

namespace JetSnail.InventorLibraryManager.UseCase.DataStores
{
    public interface IFamilyRepository
    {
        Task<IEnumerable<Family>> GetAllAsync(string libraryId = null);
        Task<Family> GetByIdAsync(string id, string libraryId = null);
        Task UpdateAsync(string familyId, string libraryId, int groupId);
        Task<Family> CopyOrMoveAsync(string id, string fromLibraryId, string toLibraryId, bool associative = true);
        Task<IEnumerable<Part>> GetPartNumbers(Family obj);
        Task<IEnumerable<Part>> UpdatePartNumbers(Family obj);
        Task<Part> UpdatePartNumber(Family obj, string partId);
    }
}