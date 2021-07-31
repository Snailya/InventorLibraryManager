using System;

namespace JetSnail.InventorLibraryManager.Core.Entities
{
    public abstract class BaseEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}