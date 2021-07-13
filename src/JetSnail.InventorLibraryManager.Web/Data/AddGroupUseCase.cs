using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using AntDesign;
using JetSnail.InventorLibraryManager.Core.DTOs;
using JetSnail.InventorLibraryManager.UseCase.UseCases;

namespace JetSnail.InventorLibraryManager.Web.Data
{
    public class AddGroupUseCase : IAddGroupUseCase
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly NotificationService _notice;

        public AddGroupUseCase(IHttpClientFactory clientFactory, NotificationService notice)
        {
            _clientFactory = clientFactory;
            _notice = notice;
        }

        public async Task<GroupDto> Execute(string displayName, string shortName)
        {

            using var client = _clientFactory.CreateClient();

            var response = await client.PostAsJsonAsync("http://10.25.16.149:5000/api/groups",new GroupDto(){DisplayName = displayName,ShortName = shortName});
            
            if (response.IsSuccessStatusCode)
            {
                await using var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync
                    <GroupDto>(responseStream);
            }

            await _notice.Error(new NotificationConfig
            {
                Message = response.ReasonPhrase,
                NotificationType = NotificationType.Error
            });
            return null;
        }
    }
}