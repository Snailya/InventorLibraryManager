using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.Core.Entities;
using JetSnail.InventorLibraryManager.Data;
using JetSnail.InventorLibraryManager.UseCase.Plugins.DataStores;
using Microsoft.EntityFrameworkCore;

namespace JetSnail.InventorLibraryManager.DataStore.EFCore
{
    public class PrototypeRepository : IPrototypeRepository
    {
        private readonly ApplicationDbContext _context;

        public PrototypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<PrototypeFamilyEntity[]> GetAllAsync()
        {
            return _context.Prototypes.Include(x => x.Derivatives).Include(x => x.Group)
                .ToArrayAsync();
        }

        public async Task<PrototypeFamilyEntity> GetByIdAsync(int prototypeId)
        {
            return await _context.Prototypes.Include(x => x.Derivatives).Include(x => x.Group).Include(x => x.Parts)
                .SingleOrDefaultAsync(x => x.Id == prototypeId);
        }

        public async Task<PrototypeFamilyEntity> GetByInventorIdentifier(string familyId)
        {
            return await _context.Prototypes.SingleOrDefaultAsync(x => x.FamilyId == familyId);
        }

        public async Task<PrototypeFamilyEntity> InsertAsync(PrototypeFamilyEntity prototypeEntity)
        {
            await _context.Prototypes.AddAsync(prototypeEntity);
            await _context.SaveChangesAsync();
            return prototypeEntity;
        }

        public async Task UpdateAsync(PrototypeFamilyEntity prototypeEntity)
        {
            _context.Prototypes.Update(prototypeEntity);
            await _context.SaveChangesAsync();
        }
    }
}