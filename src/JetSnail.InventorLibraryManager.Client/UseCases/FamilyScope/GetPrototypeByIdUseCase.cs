using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.UseCase.FamilyScope;
using JetSnail.InventorLibraryManager.UseCase.FamilyScope.DTOs;

namespace JetSnail.InventorLibraryManager.Client.UseCases.FamilyScope
{
    public class GetPrototypeByIdUseCase : IGetPrototypeByIdUseCase
    {
        private readonly IHttpClientFactory _clientFactory;

        public GetPrototypeByIdUseCase(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<PrototypeFamilyDto> Execute(int id)
        {
            using var client = _clientFactory.CreateClient("inventor");
            var response = await client.GetAsync($"api/v2.0/prototypes/{id}");

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<PrototypeFamilyDto>();
        }
    }
}