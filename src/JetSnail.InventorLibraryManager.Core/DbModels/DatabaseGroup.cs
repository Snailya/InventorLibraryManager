using System.ComponentModel.DataAnnotations;

namespace JetSnail.InventorLibraryManager.Core.DbModels
{
    public class DatabaseGroup
    {
        [Key] public int Id { get; set; }

        /// <summary>
        ///     类别名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        ///     类别代号
        /// </summary>
        [StringLength(3)]
        public string ShortName { get; set; }

        /// <summary>
        ///     Indicates whether need to update Inventor due to ShortName change.
        /// </summary>
        public bool HasSynchronized { get; set; } = false;
    }
}