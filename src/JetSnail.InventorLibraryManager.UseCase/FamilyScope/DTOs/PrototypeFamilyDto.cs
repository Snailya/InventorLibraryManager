using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace JetSnail.InventorLibraryManager.UseCase.FamilyScope.DTOs
{
    public class PrototypeFamilyDto : BaseFamilyDto
    {
        [JsonPropertyName("id")]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonPropertyName("group")]
        [JsonProperty("group")]
        public PrototypeGroupDto Group { get; set; }

        [JsonPropertyName("group_modified_at")]
        [JsonProperty("group_modified_at")]
        public DateTime? GroupModifiedAt { get; set; }

        [JsonPropertyName("derivatives")]
        [JsonProperty("derivatives")]
        public DerivativeFamilyDto[] Derivatives { get; set; }
    }
}