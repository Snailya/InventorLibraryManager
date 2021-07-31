using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace JetSnail.InventorLibraryManager.UseCase.GroupScope.DTOs
{
    public class CheckNameDto
    {
        [JsonPropertyName("value")]
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}