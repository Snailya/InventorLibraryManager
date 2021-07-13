using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using AntDesign;
using JetSnail.InventorLibraryManager.Core.DTOs;
using JetSnail.InventorLibraryManager.UseCase.UseCases;

namespace JetSnail.InventorLibraryManager.Web.Data
{
    public class GetGroupsUseCase : IGetGroupsUseCase
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly NotificationService _notice;

        public GetGroupsUseCase(IHttpClientFactory clientFactory, NotificationService notice)
        {
            _clientFactory = clientFactory;
            _notice = notice;
        }

        public async Task<GroupDto[]> Execute()
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                "http://10.25.16.149:5000/api/groups");
            using var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);
            
            if (response.IsSuccessStatusCode)
            {
                await using var responseStream = await response.Content.ReadAsStreamAsync();
                return (await JsonSerializer.DeserializeAsync
                    <IEnumerable<GroupDto>>(responseStream))!.ToArray();
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