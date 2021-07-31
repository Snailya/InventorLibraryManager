using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.UseCase.GroupScope;
using JetSnail.InventorLibraryManager.UseCase.GroupScope.DTOs;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;

namespace JetSnail.InventorLibraryManager.Client.UseCases.GroupScope
{
    public class
        UpdateGroupThenUpdatePartNumberIfShortNameChangeUseCase :
            IUpdateGroupThenUpdatePartNumberIfShortNameChangeUseCase
    {
        private readonly IHttpClientFactory _clientFactory;

        public UpdateGroupThenUpdatePartNumberIfShortNameChangeUseCase(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task Execute(int id, JsonPatchDocument<GroupDto> doc)
        {
            using var client = _clientFactory.CreateClient("inventor");
            var content = new StringContent(JsonConvert.SerializeObject(doc), Encoding.UTF8,
                "application/json-patch+json");
            var response = await client.PatchAsync($"api/v2.0/groups/{id}", content);

            response.EnsureSuccessStatusCode();
        }
    }
}