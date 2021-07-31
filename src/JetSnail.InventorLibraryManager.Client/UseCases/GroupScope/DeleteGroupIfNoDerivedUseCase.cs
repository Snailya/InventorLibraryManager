using System.Net.Http;
using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.UseCase.GroupScope;

namespace JetSnail.InventorLibraryManager.Client.UseCases.GroupScope
{
    public class DeleteGroupIfNoDerivedUseCase : IDeleteGroupIfNoDerivedUseCase
    {
        private readonly IHttpClientFactory _clientFactory;

        public DeleteGroupIfNoDerivedUseCase(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task Execute(int id)
        {
            using var client = _clientFactory.CreateClient("inventor");
            var response = await client.DeleteAsync($"api/v2.0/groups/{id}");

            response.EnsureSuccessStatusCode();
        }
    }
}