using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.UseCase.FamilyScope;
using JetSnail.InventorLibraryManager.UseCase.FamilyScope.DTOs;

namespace JetSnail.InventorLibraryManager.Client.UseCases.FamilyScope
{
    public class CreateDerivativeThenSynPartNumberUseCase : ICreateDerivativeThenSynPartNumberUseCase
    {
        private readonly IHttpClientFactory _clientFactory;

        public CreateDerivativeThenSynPartNumberUseCase(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<DerivativeFamilyDto> Execute(int prototypeId, string toLibraryId)
        {
            using var client = _clientFactory.CreateClient("inventor");
            var response = await client.PostAsJsonAsync($"api/v2.0/prototypes/{prototypeId}/derivatives",
                new CreateDerivativeDto {ToLibraryId = toLibraryId});

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<DerivativeFamilyDto>();
        }
    }
}