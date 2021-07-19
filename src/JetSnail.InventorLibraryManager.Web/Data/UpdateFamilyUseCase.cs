using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using AntDesign;
using JetSnail.InventorLibraryManager.Core.DTOs;
using JetSnail.InventorLibraryManager.UseCase.UseCases;

namespace JetSnail.InventorLibraryManager.Web.Data
{
    public class UpdateFamilyUseCase : IUpdateFamilyUseCase
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly NotificationService _notice;

        public UpdateFamilyUseCase(IHttpClientFactory clientFactory, NotificationService notice)
        {
            _clientFactory = clientFactory;
            _notice = notice;
        }

        public async Task<FamilyDto> Execute(string familyId, string libraryId, int groupId)
        {
            using var client = _clientFactory.CreateClient("inventor");
            var requestContent = new StringContent(groupId.ToString(), Encoding.UTF8, "application/json-patch+json");

            var response = await client.PatchAsync($"families/{familyId}?libraryId={libraryId}", requestContent);

            if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<FamilyDto>();

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