using System.Text.Json.Serialization;
using JetSnail.InventorLibraryManager.UseCase.FamilyScope.DTOs;
using Newtonsoft.Json;

namespace JetSnail.InventorLibraryManager.UseCase.GroupScope.DTOs
{
    public class GroupDto
    {
        /// <summary>
        ///     Identifier used in <see cref="AssignGroupDto" />.
        /// </summary>
        [JsonPropertyName("id")]
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        ///     One of user toward identifier about group.
        /// </summary>
        [JsonPropertyName("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        ///     One of user toward identifier about group.
        /// </summary>
        [JsonPropertyName("shortname")]
        [JsonProperty("shortname")]
        public string ShortName { get; set; }

        [JsonPropertyName("prototypes")]
        [JsonProperty("prototypes")]
        public GroupPrototypeDto[] Prototypes { get; set; }
    }
}