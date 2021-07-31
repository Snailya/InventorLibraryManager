using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.UseCase.GroupScope;
using JetSnail.InventorLibraryManager.UseCase.GroupScope.DTOs;

namespace JetSnail.InventorLibraryManager.Client.UseCases.GroupScope
{
    public class GetGroupsUseCase : IGetGroupsUseCase
    {
        private readonly IHttpClientFactory _clientFactory;

        public GetGroupsUseCase(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<GroupDto[]> Execute()
        {
            using var client = _clientFactory.CreateClient("inventor");
            var response = await client.GetAsync("api/v2.0/groups");

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<GroupDto[]>();
        }
    }
}