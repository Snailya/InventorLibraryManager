using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using JetSnail.InventorLibraryManager.Core.Entities;
using JetSnail.InventorLibraryManager.UseCase;
using JetSnail.InventorLibraryManager.UseCase.FamilyScope;
using JetSnail.InventorLibraryManager.UseCase.FamilyScope.DTOs;
using JetSnail.InventorLibraryManager.UseCase.Plugins.DataStores;
using JetSnail.InventorLibraryManager.UseCase.Plugins.Software;
using Microsoft.AspNetCore.SignalR;

namespace JetSnail.InventorLibraryManager.Server.Hubs
{
    public class DashboardHub : Hub<IDashboardHub>, IScanPrototypesUseCase
    {
        private readonly IInventorService _inventorService;
        private readonly IPrototypeRepository _prototypeRepository;

        public DashboardHub(IInventorService inventorService, IPrototypeRepository prototypeRepository)
        {
            _inventorService = inventorService;
            _prototypeRepository = prototypeRepository;
        }

        [HubMethodName("scan-prototypes")]
        public async IAsyncEnumerable<ProgressDto> Execute([EnumeratorCancellation] CancellationToken token)
        {
            var result = new ProgressDto();
            var index = 0;

            var prototypes = _inventorService.GetAllFamilies().Where(x => string.IsNullOrEmpty(x.FromSource)).ToArray();
            foreach (var prototype in prototypes)
            {
                if (token.IsCancellationRequested)
                {
                    result.Message = "取消操作。";
                    result.Status = ProgressStatus.Canceled;
                    yield return result;
                    yield break;
                }

                index++;
                result.Message = $"正在处理{prototype.DisplayName}({index}/{prototypes.Length})...";
                result.Percent = index * 100 / prototypes.Length;
                result.Status = ProgressStatus.Processing;
                // return a start message
                yield return result;

                // persist
                try
                {
                    if (await _prototypeRepository.GetByInventorIdentifier(prototype.InternalName) == null)
                        await _prototypeRepository.InsertAsync(
                            new PrototypeFamilyEntity
                            {
                                FamilyId = prototype.InternalName,
                                LibraryId = prototype.Library
                            });
                }
                catch (Exception e)
                {
                    result.Message = $"处理{prototype.DisplayName}失败: {e.Message}({index}/{prototypes.Length})...";
                    result.Status = ProgressStatus.OnError;
                }

                // return errro log if on error
                if (result.Status == ProgressStatus.OnError) yield return result;
            }

            yield return new ProgressDto
            {
                Message = "完成。",
                Percent = 100,
                Status = ProgressStatus.Finished
            };
        }
    }
}