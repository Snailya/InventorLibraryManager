using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.UseCase.FamilyScope;
using JetSnail.InventorLibraryManager.UseCase.FamilyScope.DTOs;

namespace JetSnail.InventorLibraryManager.Client.UseCases.FamilyScope
{
    public class SaveDerivativeThenSynPartNumberUseCase : ISaveDerivativeThenSynPartNumberUseCase
    {
        private readonly IHttpClientFactory _clientFactory;

        public SaveDerivativeThenSynPartNumberUseCase(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<DerivativeFamilyDto> Execute(int prototypeId, string libraryId)
        {
            using var client = _clientFactory.CreateClient("inventor");
            var response = await client.PatchAsync($"api/v2.0/prototypes/{prototypeId}/derivatives",
                JsonContent.Create(new SaveDerivativeDto {LibraryId = libraryId}));

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<DerivativeFamilyDto>();
        }
    }
}