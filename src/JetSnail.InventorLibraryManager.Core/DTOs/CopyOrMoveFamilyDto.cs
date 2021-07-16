using System.Text.Json.Serialization;

namespace JetSnail.InventorLibraryManager.Core.DTOs
{
    public class CopyOrMoveFamilyDto
    {
	    /// <summary>
	    ///     族Id
	    /// </summary>
	    [JsonPropertyName("family_id")]
        public string FamilyInternalName { get; set; }

	    /// <summary>
	    ///     移动前的库Id。
	    /// </summary>
	    [JsonPropertyName("from_library_id")]
        public string FromLibraryInternalName { get; set; }

	    /// <summary>
	    ///     移动后的库Id。
	    /// </summary>
	    [JsonPropertyName("to_library_id")]
        public string ToLibraryInternalName { get; set; }

	    /// <summary>
	    ///     是否与原族关联。
	    /// </summary>
	    [JsonPropertyName("associative")]
        public bool Associative { get; set; } = true;
    }
}