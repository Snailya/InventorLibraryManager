using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JetSnail.InventorLibraryManager.Core.DbModels
{
    public class DatabasePart
    {
        [Key] public int Id { get; set; }
        public string InternalName { get; set; }
        public virtual DatabaseFamily Family { get; set; }

        [NotMapped] public string PartNumber => $"{Family.Group.ShortName}{Id:D8}";
    }
}