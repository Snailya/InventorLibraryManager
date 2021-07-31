using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.UseCase.FamilyScope;
using JetSnail.InventorLibraryManager.UseCase.FamilyScope.DTOs;

namespace JetSnail.InventorLibraryManager.Client.UseCases.FamilyScope
{
    public class GetDerivativesUseCase : IGetDerivativesUseCase
    {
        private readonly IHttpClientFactory _clientFactory;

        public GetDerivativesUseCase(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<DerivativeFamilyDto[]> Execute(int prototypeId)
        {
            using var client = _clientFactory.CreateClient("inventor");
            var response = await client.GetAsync($"api/v2.0/prototypes/{prototypeId}/derivatives");

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<DerivativeFamilyDto[]>();
        }
    }
}