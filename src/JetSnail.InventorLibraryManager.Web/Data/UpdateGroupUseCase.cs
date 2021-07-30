using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using AntDesign;
using JetSnail.InventorLibraryManager.Core.DTOs;
using JetSnail.InventorLibraryManager.UseCase.UseCases;
using Newtonsoft.Json;

namespace JetSnail.InventorLibraryManager.Web.Data
{
    public class UpdateGroupUseCase : IUpdateGroupUseCase
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly NotificationService _notice;

        public UpdateGroupUseCase(IHttpClientFactory clientFactory, NotificationService notice)
        {
            _clientFactory = clientFactory;
            _notice = notice;
        }

        public async Task<GroupDto> Execute(int id, string displayName, string shortName)
        {
            using var client = _clientFactory.CreateClient("inventor");
            var content = JsonContent.Create(new GroupDto { Id = id, DisplayName = displayName, ShortName = shortName }, mediaType: null, null);
            var response = await client.PatchAsync($"groups", content);

            if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<GroupDto>();

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