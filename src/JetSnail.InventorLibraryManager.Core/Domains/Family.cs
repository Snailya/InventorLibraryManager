using JetSnail.InventorLibraryManager.Core.DbModels;
using JetSnail.InventorLibraryManager.Core.InventorModels;

namespace JetSnail.InventorLibraryManager.Core.Domains
{
    public class Family
    {
        public DatabaseFamily DatabaseModel { get; set; }
        public InventorFamily InventorModel { get; set; }
    }
}