using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.Core.Domains;
using JetSnail.InventorLibraryManager.Data;
using JetSnail.InventorLibraryManager.UseCase.DataStores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JetSnail.InventorLibraryManager.Service.Services
{
    public class GroupRepository : IGroupRepository
    {
        private readonly ContentCenterContext _context;
        private readonly ILogger<GroupRepository> _logger;

        public GroupRepository(ILogger<GroupRepository> logger, ContentCenterContext context)
        {
            _logger = logger;
            _context = context;
        }

        public Task<IEnumerable<Group>> GetAllAsync()
        {
            return Task.FromResult(_context.Groups.AsNoTracking().Select(x => new Group { DatabaseModel = x })
                .AsEnumerable());
        }

        public async Task<Group> GetByIdAsync(int id)
        {
            return new Group { DatabaseModel = await _context.Groups.FindAsync(id) };
        }

        public async Task<Group> InsertAsync(Group obj)
        {
            _context.Groups.Add(obj.DatabaseModel);
            await _context.SaveChangesAsync();

            return obj;
        }

        public async Task UpdateAsync(int id, string displayName, string shortName)
        {
	        var group =  await _context.Groups.FindAsync(id);
	        group.DisplayName = displayName;
	        group.ShortName = shortName;
            _context.Groups.Update(group);
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