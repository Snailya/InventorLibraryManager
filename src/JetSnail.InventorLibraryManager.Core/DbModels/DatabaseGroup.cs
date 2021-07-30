using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JetSnail.InventorLibraryManager.Core.DbModels
{
    public class DatabaseGroup:EntityBase
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
        ///     存储在数据库内的关联的零件信息
        /// </summary>
        // public virtual ICollection<DatabaseFamily> Families { get; set; } = new List<DatabaseFamily>();
    }
}