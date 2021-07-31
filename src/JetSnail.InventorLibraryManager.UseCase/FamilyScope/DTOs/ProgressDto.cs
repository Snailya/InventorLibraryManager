using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace JetSnail.InventorLibraryManager.UseCase.FamilyScope.DTOs
{
    public class ProgressDto
    {
        [JsonPropertyName("status")]
        [JsonProperty("status")]
        public ProgressStatus Status { get; set; }

        [JsonPropertyName("percent")]
        [JsonProperty("percent")]
        public int Percent { get; set; }

        [JsonPropertyName("message")]
        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public enum ProgressStatus
    {
        Processing,
        Finished,
        OnError,
        Canceled
    }
}