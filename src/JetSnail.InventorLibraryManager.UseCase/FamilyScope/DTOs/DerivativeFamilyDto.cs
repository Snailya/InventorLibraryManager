using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace JetSnail.InventorLibraryManager.UseCase.FamilyScope.DTOs
{
    public class DerivativeFamilyDto : BaseFamilyDto
    {
        public DerivativeFamilyDto()
        {
            Type = FamilyType.Derivative;
        }

        [JsonPropertyName("created_at")]
        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("synchronized_at")]
        [JsonProperty("synchronized_at")]
        public DateTime? SynchronizedAt { get; set; }
    }
}