using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetSnail.InventorLibraryManager.Core.DbModels
{
	public class EntityBase
	{
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime CreatedAt { get; set; }

		/// <summary>
		/// 修改时间
		/// </summary>
		public DateTime ModifiedAt { get; set; }
	}
}
