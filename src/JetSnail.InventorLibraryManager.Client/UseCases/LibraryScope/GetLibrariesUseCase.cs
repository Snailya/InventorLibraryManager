using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.UseCase.LibraryScope;
using JetSnail.InventorLibraryManager.UseCase.LibraryScope.DTOs;

namespace JetSnail.InventorLibraryManager.Client.UseCases.LibraryScope
{
    public class GetLibrariesUseCase : IGetLibrariesUseCase
    {
        private readonly IHttpClientFactory _clientFactory;

        public GetLibrariesUseCase(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<LibraryDto[]> Execute()
        {
            using var client = _clientFactory.CreateClient("inventor");
            var response = await client.GetAsync("api/v2.0/libraries");

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<LibraryDto[]>();
        }
    }
}