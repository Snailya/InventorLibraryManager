using System.Text.Json.Serialization;

namespace JetSnail.InventorLibraryManager.Core.DTOs
{
    public class DeletePartDto
    {
		/// <summary>
		///     零件Id。
		/// </summary>
		[JsonPropertyName("part_id")]
	    public string PartInternalName { get; set; }

		/// <summary>
		///     族Id
		/// </summary>
		[JsonPropertyName("family_id")]
	    public string FamilyInternalName { get; set; }

		/// <summary>
		///     库Id。
		/// </summary>
		[JsonPropertyName("library_id")]
	    public string LibraryInternalName { get; set; }
    }
}