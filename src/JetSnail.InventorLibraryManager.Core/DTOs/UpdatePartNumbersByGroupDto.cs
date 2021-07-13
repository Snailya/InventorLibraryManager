using System.Text.Json.Serialization;

namespace JetSnail.InventorLibraryManager.Core.DTOs
{
    public class UpdatePartNumbersByGroupDto
    {
		/// <summary>
		///     分组Id。
		/// </summary>
		[JsonPropertyName("group_database_id")]
	    public int GroupId { get; set; }

		/// <summary>
		///     是否使用检查模式。
		/// </summary>
		[JsonPropertyName("check")]
	    public bool CheckMode { get; set; } = false;
    }
}