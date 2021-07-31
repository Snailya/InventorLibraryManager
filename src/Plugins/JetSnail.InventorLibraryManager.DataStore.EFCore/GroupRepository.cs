using System.Linq;
using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.Core.Entities;
using JetSnail.InventorLibraryManager.Data;
using JetSnail.InventorLibraryManager.UseCase.Plugins.DataStores;
using Microsoft.EntityFrameworkCore;

namespace JetSnail.InventorLibraryManager.DataStore.EFCore
{
    public class GroupRepository : IGroupRepository
    {
        private readonly ApplicationDbContext _context;

        public GroupRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public GroupEntity[] GetAll()
        {
            return _context.Groups.Include(x => x.Prototypes).ToArray();
        }

        public async Task<GroupEntity[]> GetAllAsync()
        {
            return await _context.Groups.Include(x => x.Prototypes).ToArrayAsync();
        }


        public async Task<GroupEntity> GetByIdAsync(int id)
        {
            return await _context.Groups.Include(x => x.Prototypes).ThenInclude(x => x.Derivatives)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<GroupEntity> InsertAsync(GroupEntity groupEntity)
        {
            await _context.AddAsync(groupEntity);
            await _context.SaveChangesAsync();
            return groupEntity;
        }

        public async Task UpdateAsync(GroupEntity groupEntity)
        {
            _context.Update(groupEntity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var group = await _context.Groups.FindAsync(id);
            _context.Remove(group);
            await _context.SaveChangesAsync();
        }
    }
}