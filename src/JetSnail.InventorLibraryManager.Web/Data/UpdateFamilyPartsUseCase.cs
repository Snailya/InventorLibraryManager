using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using AntDesign;
using JetSnail.InventorLibraryManager.Core.DTOs;
using JetSnail.InventorLibraryManager.UseCase.UseCases;

namespace JetSnail.InventorLibraryManager.Web.Data
{
    public class UpdateFamilyPartsUseCase : IUpdateFamilyPartsUseCase
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly NotificationService _notice;

        public UpdateFamilyPartsUseCase(IHttpClientFactory clientFactory, NotificationService notice)
        {
            _clientFactory = clientFactory;
            _notice = notice;
        }

        public async Task<PartDto[]> Execute(string familyId, string libraryId)
        {
            using var client = _clientFactory.CreateClient("inventor");
            var requestContent = new StringContent(string.Empty, Encoding.UTF8, "application/json-patch+json");
            var response =
                await client.PutAsync($"families/{familyId}/parts?libraryId={libraryId}", requestContent);

            if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<PartDto[]>();

            await _notice.Error(new NotificationConfig
            {
                Message = response.ReasonPhrase,
                Description = await response.Content.ReadAsStringAsync(),
                Duration = 0,
                NotificationType = NotificationType.Error
            });
            return null;
        }
    }
}