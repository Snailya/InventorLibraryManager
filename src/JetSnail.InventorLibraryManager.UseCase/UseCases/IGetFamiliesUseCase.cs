using System.Collections.Generic;
using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.Core.DTOs;

namespace JetSnail.InventorLibraryManager.UseCase.UseCases
{
    public interface IGetFamiliesUseCase
    {
        Task<FamilyDto[]> Execute();
    }
}