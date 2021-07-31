using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.Core.Entities;

namespace JetSnail.InventorLibraryManager.UseCase.Plugins.DataStores
{
    public interface IPrototypeRepository
    {
        Task<PrototypeFamilyEntity[]> GetAllAsync();
        Task<PrototypeFamilyEntity> GetByIdAsync(int prototypeId);
        Task<PrototypeFamilyEntity> GetByInventorIdentifier(string familyId);
        Task<PrototypeFamilyEntity> InsertAsync(PrototypeFamilyEntity proto);
        Task UpdateAsync(PrototypeFamilyEntity prototypeEntity);
    }
}