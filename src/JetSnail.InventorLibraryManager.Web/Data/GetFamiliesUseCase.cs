using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AntDesign;
using JetSnail.InventorLibraryManager.Core.DTOs;
using JetSnail.InventorLibraryManager.UseCase.UseCases;

namespace JetSnail.InventorLibraryManager.Web.Data
{
    public class GetFamiliesUseCase : IGetFamiliesUseCase
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly NotificationService _notice;

        public GetFamiliesUseCase(IHttpClientFactory clientFactory, NotificationService notice)
        {
            _clientFactory = clientFactory;
            _notice = notice;
        }

        public async Task<FamilyDto[]> Execute(string libraryId)
        {
            using var client = _clientFactory.CreateClient("inventor");
            var response =
                await client.GetAsync(string.IsNullOrEmpty(libraryId) ? "families" : $"families?libraryId={libraryId}");

            if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<FamilyDto[]>();

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