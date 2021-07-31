using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace JetSnail.InventorLibraryManager.UseCase.FamilyScope.DTOs
{
    public class AssignGroupDto
    {
        [JsonPropertyName("group_id")]
        [JsonProperty("group_id")]
        public int GroupId { get; set; }
    }
}