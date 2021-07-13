using System.Text.Json.Serialization;

namespace JetSnail.InventorLibraryManager.Core.DTOs
{
    public class UpdateFamilyPartNumbersDto
    {
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
		///     是否使用检查模式。
		/// </summary>
		[JsonPropertyName("check")]
	    public bool CheckMode { get; set; } = false;
    }
}