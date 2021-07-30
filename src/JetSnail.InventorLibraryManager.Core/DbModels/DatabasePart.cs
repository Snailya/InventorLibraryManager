using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JetSnail.InventorLibraryManager.Core.DbModels
{
    public class DatabasePart:EntityBase
    {
        [Key] public int Id { get; set; }

        /// <summary>
        /// Inventor ContentTableRow Id
        /// </summary>
        public string InternalName { get; set; }

        /// <summary>
        /// 存储在数据库内的关联的族信息
        /// </summary>
        public virtual DatabaseFamily Family { get; set; }

        /// <summary>
        /// 上一次同步至Inventor的时间
        /// </summary>
        public DateTime SynchronizedAt  { get; set; }

        /// <summary>
        ///  用于写入Inventor的识别码
        /// </summary>
        [NotMapped] public string PartNumber => $"{Family.Group.ShortName}{Id:D8}";
    }
}