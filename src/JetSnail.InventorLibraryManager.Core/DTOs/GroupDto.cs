using System.Text.Json.Serialization;

namespace JetSnail.InventorLibraryManager.Core.DTOs
{
    public class GroupDto
    {
	    /// <summary>
	    ///     分组Id
	    /// </summary>
	    [JsonPropertyName("group_database_id")]
        public int? Id { get; set; }

	    /// <summary>
	    ///     分组名称
	    /// </summary>
	    [JsonPropertyName("name")]
        public string DisplayName { get; set; }

	    /// <summary>
	    ///     缩写
	    /// </summary>
	    [JsonPropertyName("code")]
        public string ShortName { get; set; }
    }
}