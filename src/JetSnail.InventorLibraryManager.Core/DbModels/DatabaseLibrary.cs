using System.ComponentModel.DataAnnotations;

namespace JetSnail.InventorLibraryManager.Core.DbModels
{
	public class DatabaseLibrary:EntityBase
	{
		[Key] public int Id { get; set; }

		/// <summary>
		///     Inventor Library Id
		/// </summary>
		public string InternalName { get; set; }
	}
}