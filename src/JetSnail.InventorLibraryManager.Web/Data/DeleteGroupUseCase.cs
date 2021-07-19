using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AntDesign;
using JetSnail.InventorLibraryManager.UseCase.UseCases;

namespace JetSnail.InventorLibraryManager.Web.Data
{
    public class DeleteGroupUseCase : IDeleteGroupUseCase
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly NotificationService _notice;

        public DeleteGroupUseCase(IHttpClientFactory clientFactory, NotificationService notice)
        {
            _clientFactory = clientFactory;
            _notice = notice;
        }

        public async Task<bool> Execute(int id)
        {
            using var client = _clientFactory.CreateClient("inventor");
            var response = await client.DeleteAsync($"groups/{id}");

            if (response.StatusCode == HttpStatusCode.NoContent) return true;

            await _notice.Error(new NotificationConfig
            {
                Message = response.ReasonPhrase,
                Description = await response.Content.ReadAsStringAsync(),
                Duration = 0,
                NotificationType = NotificationType.Error
            });

            return false;
        }
    }
}