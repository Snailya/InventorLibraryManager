using System.Text.Json.Serialization;

namespace JetSnail.InventorLibraryManager.Core.DTOs
{
    public class PartDto
    {
	    /// <summary>
	    ///     Part Id in database
	    /// </summary>
	    [JsonPropertyName("part_database_id")]
        public int? Id { get; set; }

	    /// <summary>
	    ///     Part RowId in inventor
	    /// </summary>
	    [JsonPropertyName("part_id")]
        public string PartInternalName { get; set; }

	    /// <summary>
	    ///     Part FILENAME in inventor
	    /// </summary>
	    [JsonPropertyName("file_name")]
        public string InventorFileName { get; set; }

	    /// <summary>
	    ///     Part PARTNUMBER in inventor
	    /// </summary>
	    [JsonPropertyName("actual_part_number")]
        public string InventorPartNumber { get; set; }

	    /// <summary>
	    ///     Part PARTNUMBER in Database
	    /// </summary>
	    [JsonPropertyName("excepted_part_number")]
        public string DatabasePartNumber { get; set; }
    }
}