using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace JetSnail.InventorLibraryManager.UseCase.FamilyScope.DTOs
{
    public class SaveDerivativeDto
    {
        [JsonPropertyName("library_id")]
        [JsonProperty("library_id")]
        public string LibraryId { get; set; }
    }
}