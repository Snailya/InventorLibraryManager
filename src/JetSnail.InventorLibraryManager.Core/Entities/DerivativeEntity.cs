using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace JetSnail.InventorLibraryManager.Core.Entities
{
    /// <summary>
    ///     用来记录衍生的族，通过复制操作生成
    /// </summary>
    [Table("Derivative")]
    public class DerivativeEntity : BaseEntity, IFamily
    {
        public DateTime? SynchronizedAt { get; set; }
        public PrototypeFamilyEntity Prototype { get; set; }
        public string FamilyId { get; set; }
        public string LibraryId { get; set; }
    }
}