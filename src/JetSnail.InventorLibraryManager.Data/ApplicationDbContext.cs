using System;
using System.Threading;
using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace JetSnail.InventorLibraryManager.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<GroupEntity> Groups { get; set; }
        public DbSet<PrototypeFamilyEntity> Prototypes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<PrototypeFamilyEntity>()
                .HasAlternateKey(e => e.FamilyId);
            builder.Entity<DerivativeEntity>()
                .HasKey(e => new {e.FamilyId, e.LibraryId});
            builder.Entity<GroupEntity>()
                .HasIndex(e => e.ShortName)
                .IsUnique();
            builder.Entity<PartEntity>()
                .HasAlternateKey(e => new {e.PartId, e.FamilyId});
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            foreach (var entityEntry in ChangeTracker.Entries())
                if (entityEntry.Entity is BaseEntity entityBase)
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

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}