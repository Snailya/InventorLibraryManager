using System.Text.Json.Serialization;

namespace JetSnail.InventorLibraryManager.Core.DTOs
{
	/// <summary>
	///     族的详细信息，包含用于在ViewFamily页面显示Family的详细信息。
	/// </summary>
	public class FamilyDetailDto : FamilyDto
    {
		/// <summary>
		///     Family description.
		/// </summary>
		[JsonPropertyName("description")]
	    public string Description { get; set; }

		/// <summary>
		///     Array of part info.
		/// </summary>
		[JsonPropertyName("parts")]
	    public PartDto[] Parts { get; set; }
    }
}