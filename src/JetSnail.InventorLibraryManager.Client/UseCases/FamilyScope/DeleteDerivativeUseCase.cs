using System.Net.Http;
using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.UseCase.FamilyScope;

namespace JetSnail.InventorLibraryManager.Client.UseCases.FamilyScope
{
    public class DeleteDerivativeUseCase : IDeleteDerivativeUseCase
    {
        private readonly IHttpClientFactory _clientFactory;

        public DeleteDerivativeUseCase(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task Execute(int prototypeId, string libraryId)
        {
            using var client = _clientFactory.CreateClient("inventor");
            var response =
                await client.DeleteAsync($"api/v2.0/prototypes/{prototypeId}/derivatives?libraryId={libraryId}");

            response.EnsureSuccessStatusCode();
        }
    }
}