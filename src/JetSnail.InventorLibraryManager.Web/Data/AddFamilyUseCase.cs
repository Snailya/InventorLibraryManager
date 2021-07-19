﻿using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AntDesign;
using JetSnail.InventorLibraryManager.Core.DTOs;
using JetSnail.InventorLibraryManager.UseCase.UseCases;

namespace JetSnail.InventorLibraryManager.Web.Data
{
    public class AddFamilyUseCase : IAddFamilyUseCase
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly NotificationService _notice;

        public AddFamilyUseCase(IHttpClientFactory clientFactory, NotificationService notice)
        {
            _clientFactory = clientFactory;
            _notice = notice;
        }

        public async Task<FamilyDto> Execute(string familyId, string fromLibraryId, string toLibraryId)
        {
            using var client = _clientFactory.CreateClient("inventor");
            var response = await client.PostAsJsonAsync("families",
                new CopyOrMoveFamilyDto
                {
                    FamilyInternalName = familyId, FromLibraryInternalName = fromLibraryId,
                    ToLibraryInternalName = toLibraryId
                });

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