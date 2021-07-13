using System.Text.Json.Serialization;

namespace JetSnail.InventorLibraryManager.Core.DTOs
{
    public class UpdateFamilyDto
    {
		/// <summary>
		///     族Id
		/// </summary>
		[JsonPropertyName("family_id")]
	    public string FamilyInternalName { get; set; }

		/// <summary>
		///     分组Id。
		/// </summary>
		[JsonPropertyName("group_database_id")]
	    public int? GroupId { get; set; }
    }
}