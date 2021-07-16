using System.Text.Json.Serialization;

namespace JetSnail.InventorLibraryManager.Core.DTOs
{
    public class LibraryDto
    {
	    /// <summary>
	    ///     库名称
	    /// </summary>
	    [JsonPropertyName("name")]
        public string DisplayName { get; set; }

	    /// <summary>
	    ///     库Id。
	    /// </summary>
	    [JsonPropertyName("library_id")]
        public string LibraryInternalName { get; set; }

	    /// <summary>
	    ///     是否只读。
	    /// </summary>
	    [JsonPropertyName("read_only")]
        public bool ReadOnly { get; set; }
    }
}