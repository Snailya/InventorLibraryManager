using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JetSnail.InventorLibraryManager.Core.DbModels
{
    public class DatabaseFamily
    {
        [Key] public int Id { get; set; }

        /// <summary>
        ///     Inventor ContentFamily Id
        /// </summary>
        public string InternalName { get; set; }

        /// <summary>
        ///     所属的类别，用于确定PartNumber的前缀。与Inventor Category无关
        /// </summary>
        public virtual DatabaseGroup Group { get; set; }

        /// <summary>
        ///     存储在数据库内的关联的零件信息
        /// </summary>
        public virtual ICollection<DatabasePart> Parts { get; set; } = new List<DatabasePart>();
    }
}