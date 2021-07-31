using System.Collections.Generic;
using JetSnail.InventorLibraryManager.Core.InventorModels;

namespace JetSnail.InventorLibraryManager.UseCase.Plugins.Software
{
    public interface IInventorService
    {
        IEnumerable<InventorLibrary> GetServerLibraries();
        InventorLibrary GetLibraryByInternalName(string libraryId);

        IEnumerable<InventorFamily> GetAllFamilies();
        IEnumerable<InventorFamily> GetFamiliesFromLibrary(string libraryId);
        IEnumerable<InventorFamily> GetFamiliesByInternalName(string familyId);
        InventorFamily GetFamilyByInternalNames(string familyId, string libraryId);
        InventorFamilyTable GetFamilyTableByInternalNames(string familyId, string libraryId);
        InventorFamily CopyFamily(string familyId, string libraryId, string toLibraryId);
        void DeleteFamily(string familyId, string libraryId);
        bool EnsurePartNumberColumnCreated(string familyId, string libraryId);
        void SynchronizePartNumber(Dictionary<string, string> data, string familyId, string libraryId);
    }
}