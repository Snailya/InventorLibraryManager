using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
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

        public override int SaveChanges()
        {
	        return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
	        foreach (var entityEntry in ChangeTracker.Entries())
	        {
		        if (entityEntry.Entity is EntityBase entityBase)
		        {
					switch (entityEntry.State)
					{
						case EntityState.Detached:
							break;
						case EntityState.Unchanged:
							break;
						case EntityState.Deleted:
							break;
						case EntityState.Modified:
							entityBase.ModifiedAt = DateTime.Now;
							break;
						case EntityState.Added:
							entityBase.CreatedAt = DateTime.Now;
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
				}
	        }

	        return base.SaveChangesAsync(cancellationToken);
        }
    }
}