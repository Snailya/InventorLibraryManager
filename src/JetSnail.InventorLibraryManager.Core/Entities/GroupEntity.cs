using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JetSnail.InventorLibraryManager.Core.Entities
{
    /// <summary>
    ///     附加分组信息
    /// </summary>
    [Table("Group")]
    public class GroupEntity : BaseEntity
    {
        [Key] public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public virtual ICollection<PrototypeFamilyEntity> Prototypes { get; set; } = new List<PrototypeFamilyEntity>();
    }
}