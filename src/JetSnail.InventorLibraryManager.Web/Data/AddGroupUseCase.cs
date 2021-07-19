﻿using System.Net.Http;
using System.Net.Http.Json;
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
            using var client = _clientFactory.CreateClient("inventor");
            var response = await client.PostAsJsonAsync("groups",
                new GroupDto { DisplayName = displayName, ShortName = shortName });

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