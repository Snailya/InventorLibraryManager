using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace JetSnail.InventorLibraryManager.UseCase.LibraryScope.DTOs
{
    public class LibraryDto
    {
        [JsonPropertyName("library_id")]
        [JsonProperty("library_id")]
        public string LibraryId { get; set; }

        [JsonPropertyName("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonPropertyName("read_only")]
        [JsonProperty("read_only")]
        public bool IsReadOnly { get; set; }
    }
}