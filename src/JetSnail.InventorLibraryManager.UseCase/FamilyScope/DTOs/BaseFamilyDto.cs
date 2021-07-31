using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace JetSnail.InventorLibraryManager.UseCase.FamilyScope.DTOs
{
    public class BaseFamilyDto
    {
        [JsonPropertyName("familyId")]
        [JsonProperty("familyId")]
        public string FamilyId { get; set; }

        [JsonPropertyName("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonPropertyName("type")]
        [JsonProperty("type")]
        public FamilyType Type { get; set; }

        [JsonPropertyName("library")]
        [JsonProperty("library")]
        public FamilyLibraryDto Library { get; set; }
    }

    public enum FamilyType
    {
        Prototype,
        Derivative
    }
}