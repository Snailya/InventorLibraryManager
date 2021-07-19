using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AntDesign;
using JetSnail.InventorLibraryManager.Core.DTOs;
using JetSnail.InventorLibraryManager.UseCase.UseCases;

namespace JetSnail.InventorLibraryManager.Web.Data
{
    public class GetFamilyPartsUseCase : IGetFamilyPartsUseCase
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly NotificationService _notice;

        public GetFamilyPartsUseCase(IHttpClientFactory clientFactory, NotificationService notice)
        {
            _clientFactory = clientFactory;
            _notice = notice;
        }

        public async Task<PartDto[]> Execute(string familyId, string libraryId)
        {
            using var client = _clientFactory.CreateClient("inventor");
            var response =
                await client.GetAsync($"families/{familyId}/parts?libraryId={libraryId}");

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