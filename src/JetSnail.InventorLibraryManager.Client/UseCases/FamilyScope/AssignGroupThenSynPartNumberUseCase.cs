using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.UseCase.FamilyScope;
using JetSnail.InventorLibraryManager.UseCase.FamilyScope.DTOs;

namespace JetSnail.InventorLibraryManager.Client.UseCases.FamilyScope
{
    public class AssignGroupThenSynPartNumberUseCase : IAssignGroupThenSynPartNumberUseCase
    {
        private readonly IHttpClientFactory _clientFactory;

        public AssignGroupThenSynPartNumberUseCase(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task Execute(int prototypeId, int groupId)
        {
            using var client = _clientFactory.CreateClient("inventor");
            var response = await client.PutAsJsonAsync($"api/v2.0/prototypes/{prototypeId}/group",
                new AssignGroupDto {GroupId = groupId});

            response.EnsureSuccessStatusCode();
        }
    }
}