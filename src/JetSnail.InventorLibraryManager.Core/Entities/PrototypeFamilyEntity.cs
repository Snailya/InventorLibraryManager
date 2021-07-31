using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JetSnail.InventorLibraryManager.Core.Entities
{
    /// <summary>
    ///     族的样本，用来记录Meta信息
    /// </summary>
    [Table("Prototype")]
    public class PrototypeFamilyEntity : BaseEntity, IFamily
    {
        [Key] public int Id { get; set; }
        public string FamilyId { get; set; }
        public string LibraryId { get; set; }
        public GroupEntity Group { get; set; }
        public DateTime? GroupModifiedAt { get; set; }
        public virtual ICollection<PartEntity> Parts { get; set; } = new HashSet<PartEntity>();
        public virtual ICollection<DerivativeEntity> Derivatives { get; set; } =
            new HashSet<DerivativeEntity>();
    }
}