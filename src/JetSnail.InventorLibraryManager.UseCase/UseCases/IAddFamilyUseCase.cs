﻿using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.Core.DTOs;

namespace JetSnail.InventorLibraryManager.UseCase.UseCases
{
    public interface IAddFamilyUseCase
    {
        Task<FamilyDto> Execute(string familyId, string fromLibraryId, string toLibraryId);
    }
}