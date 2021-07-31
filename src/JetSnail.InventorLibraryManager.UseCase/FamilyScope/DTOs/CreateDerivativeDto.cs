using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace JetSnail.InventorLibraryManager.UseCase.FamilyScope.DTOs
{
    public class CreateDerivativeDto
    {
        [JsonPropertyName("to_library_id")]
        [JsonProperty("to_library_id")]
        public string ToLibraryId { get; set; }
    }
}