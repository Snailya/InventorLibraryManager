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
    public class GetFamiliesUseCase : IGetFamiliesUseCase
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly NotificationService _notice;

        public GetFamiliesUseCase(IHttpClientFactory clientFactory, NotificationService notice)
        {
            _clientFactory = clientFactory;
            _notice = notice;
        }

        public async Task<FamilyDto[]> Execute()
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                "http://10.25.16.149:5000/api/families");
            using var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);
            
            if (response.IsSuccessStatusCode)
            {
                await using var responseStream = await response.Content.ReadAsStreamAsync();
                return (await JsonSerializer.DeserializeAsync
                    <FamilyDto[]>(responseStream))!.ToArray();
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