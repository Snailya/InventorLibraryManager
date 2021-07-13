using JetSnail.InventorLibraryManager.Core.DbModels;
using Microsoft.EntityFrameworkCore;

namespace JetSnail.InventorLibraryManager.Data
{
    public class ContentCenterContext : DbContext
    {
        public ContentCenterContext(DbContextOptions<ContentCenterContext> options)
            : base(options)
        {
        }

        public DbSet<DatabaseGroup> Groups { get; set; }
        public DbSet<DatabaseFamily> Families { get; set; }
        public DbSet<DatabasePart> Parts { get; set; }
    }
}