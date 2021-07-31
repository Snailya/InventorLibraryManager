using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.UseCase.FamilyScope;
using JetSnail.InventorLibraryManager.UseCase.FamilyScope.DTOs;

namespace JetSnail.InventorLibraryManager.Client.UseCases.FamilyScope
{
    public class GetPrototypesUseCase : IGetPrototypesUseCase
    {
        private readonly IHttpClientFactory _clientFactory;

        public GetPrototypesUseCase(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<PrototypeFamilyDto[]> Execute()
        {
            using var client = _clientFactory.CreateClient("inventor");
            var response = await client.GetAsync("api/v2.0/prototypes");

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<PrototypeFamilyDto[]>();
        }
    }
}