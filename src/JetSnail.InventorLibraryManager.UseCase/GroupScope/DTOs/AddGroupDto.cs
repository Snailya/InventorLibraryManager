using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace JetSnail.InventorLibraryManager.UseCase.GroupScope.DTOs
{
    public class AddGroupDto
    {
        [JsonPropertyName("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonPropertyName("shortname")]
        [JsonProperty("shortname")]
        public string ShortName { get; set; }
    }
}