using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.UseCase.GroupScope;
using JetSnail.InventorLibraryManager.UseCase.GroupScope.DTOs;

namespace JetSnail.InventorLibraryManager.Client.UseCases.GroupScope
{
    public class AddGroupUseCase : IAddGroupUseCase
    {
        private readonly IHttpClientFactory _clientFactory;

        public AddGroupUseCase(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<GroupDto> Execute(string name, string shortName)
        {
            using var client = _clientFactory.CreateClient("inventor");
            var response = await client.PostAsJsonAsync("api/v2.0/groups",
                new AddGroupDto {Name = name, ShortName = shortName});

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<GroupDto>();
        }
    }
}