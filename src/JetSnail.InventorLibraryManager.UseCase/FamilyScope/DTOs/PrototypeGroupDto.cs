using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace JetSnail.InventorLibraryManager.UseCase.FamilyScope.DTOs
{
    public class PrototypeGroupDto
    {
        [JsonPropertyName("id")]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}