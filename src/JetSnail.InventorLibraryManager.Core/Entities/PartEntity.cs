using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JetSnail.InventorLibraryManager.Core.Entities
{
    /// <summary>
    ///     附加的零件信息
    /// </summary>
    [Table("Part")]
    public class PartEntity : BaseEntity
    {
        [Key] public int Id { get; set; }
        public string PartId { get; set; }
        public string FamilyId { get; set; }
    }
}