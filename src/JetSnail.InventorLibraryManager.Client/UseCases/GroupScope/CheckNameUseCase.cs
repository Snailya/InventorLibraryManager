using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.UseCase.GroupScope;
using JetSnail.InventorLibraryManager.UseCase.GroupScope.DTOs;

namespace JetSnail.InventorLibraryManager.Client.UseCases.GroupScope
{
    public class CheckNameUseCase : ICheckNameUseCase
    {
        private readonly IHttpClientFactory _clientFactory;

        public CheckNameUseCase(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<(bool, string)> Execute(string newValue, string oldValue = null)
        {
            using var client = _clientFactory.CreateClient("inventor");
            var response = await client.PostAsJsonAsync($"api/v2.0/groups/check-name?current={oldValue}",
                new CheckNameDto {Value = newValue});

            return response.IsSuccessStatusCode
                ? (true, await response.Content.ReadAsStringAsync())
                : (false, await response.Content.ReadAsStringAsync());
        }
    }
}