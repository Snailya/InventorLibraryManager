using JetSnail.InventorLibraryManager.Core.DbModels;
using JetSnail.InventorLibraryManager.Core.InventorModels;

namespace JetSnail.InventorLibraryManager.Core.Domains
{
    public class Part
    {
        public DatabasePart DatabaseModel { get; set; }
        public InventorRow InventorModel { get; set; }
    }
}