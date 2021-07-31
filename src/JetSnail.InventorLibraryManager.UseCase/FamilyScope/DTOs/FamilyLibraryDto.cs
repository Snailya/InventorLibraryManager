using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace JetSnail.InventorLibraryManager.UseCase.FamilyScope.DTOs
{
    public class FamilyLibraryDto
    {
        [JsonPropertyName("library_id")]
        [JsonProperty("library_id")]
        public string LibraryId { get; set; }

        [JsonPropertyName("name")]
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}