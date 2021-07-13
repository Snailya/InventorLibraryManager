using System.Text.Json.Serialization;

namespace JetSnail.InventorLibraryManager.Core.DTOs
{
    public class AllocatePartNumberDto
    {
		/// <summary>
		///     零件Id
		/// </summary>
		[JsonPropertyName("part_id")]
		public string PartInternalName { get; set; }

		/// <summary>
		///     族Id。
		/// </summary>
		[JsonPropertyName("family_id")]

		public string FamilyInternalName { get; set; }

		/// <summary>
		///     库Id。
		/// </summary>
		[JsonPropertyName("library_id")]

		public string LibraryInternalName { get; set; }

		/// <summary>
		///     分组Id。
		/// </summary>
		[JsonPropertyName("group_database_id")]
		public int GroupId { get; set; }
    }
}