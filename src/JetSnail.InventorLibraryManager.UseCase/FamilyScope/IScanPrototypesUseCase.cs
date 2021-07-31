using System.Collections.Generic;
using System.Threading;
using JetSnail.InventorLibraryManager.UseCase.FamilyScope.DTOs;

namespace JetSnail.InventorLibraryManager.UseCase.FamilyScope
{
    public interface IScanPrototypesUseCase
    {
        IAsyncEnumerable<ProgressDto> Execute(CancellationToken token);
    }
}