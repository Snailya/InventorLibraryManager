using System.Collections.Generic;
using System.Linq;
using Inventor;
using JetSnail.InventorLibraryManager.Core.InventorModels;
using JetSnail.InventorLibraryManager.UseCase.Plugins.Software;

namespace JetSnail.InventorLibraryManager.Software.Inventor
{
    /// <summary>
    ///     Assume that the operation in this class is always success, so please validate operation in use cases
    /// </summary>
    public class InventorService : IInventorService
    {
        public IEnumerable<InventorLibrary> GetServerLibraries()
        {
            var app = InventorHelper.GetAppReference();
            return app.ContentCenter.LibraryManager.GetServerLibraries().ToObject<InventorLibraries>()
                .Libraries;
        }

        public IEnumerable<InventorFamily> GetAllFamilies()
        {
            var app = InventorHelper.GetAppReference();
            return app.ContentCenter.LibraryManager.GetServerLibraries().ToObject<InventorLibraries>()
                .Libraries.SelectMany(x => ((string) app.ContentCenter.GetTableOfContents(
                    GeneralDataTypeEnum.kVTableGeneralData,
                    x.InternalName)).ToObject<InventorTableOfContents>().Families.Families);
        }

        public IEnumerable<InventorFamily> GetFamiliesFromLibrary(string libraryId)
        {
            var app = InventorHelper.GetAppReference();
            return ((string) app.ContentCenter.GetTableOfContents(GeneralDataTypeEnum.kVTableGeneralData, libraryId))
                .ToObject<InventorTableOfContents>().Families.Families;
        }

        public InventorFamily GetFamilyByInternalNames(string familyId, string libraryId)
        {
            return GetFamiliesFromLibrary(libraryId).SingleOrDefault(x => x.InternalName == familyId);
        }

        public InventorFamilyTable GetFamilyTableByInternalNames(string familyId, string libraryId)
        {
            var app = InventorHelper.GetAppReference();
            return app.ContentCenter.FamilyManager.GetFamilyTable(familyId, libraryId, true)
                .ToObject<InventorFamilyTable>();
        }

        public void SynchronizePartNumber(Dictionary<string, string> data, string familyId, string libraryId)
        {
            var app = InventorHelper.GetAppReference();
            var familyTable =
                app.ContentCenter.FamilyManager.GetFamilyTable(familyId, libraryId, true)
                    .ToObject<InventorFamilyTable>();

            foreach (var row in familyTable.Rows.Rows)
                if (data.TryGetValue(row.InternalName, out var partNumber))
                    if (row.Cells.SingleOrDefault(c => c.ColumnId == "AE_PARTNUMBER") is { } cell)
                        cell.Text = partNumber;

            app.ContentCenter.FamilyManager.EditFamilyTable(familyId, libraryId, familyTable.ToXmlString());
        }

        public InventorFamily CopyFamily(string familyId, string libraryId, string toLibraryId)
        {
            var app = InventorHelper.GetAppReference();
            app.ContentCenter.FamilyManager.CopyFamily(familyId, libraryId, toLibraryId);

            return app.ContentCenter.FamilyManager.GetFamily(familyId, toLibraryId).ToObject<InventorFamily>();
        }

        public bool EnsurePartNumberColumnCreated(string familyId, string libraryId)
        {
            var app = InventorHelper.GetAppReference();
            var familyTable =
                app.ContentCenter.FamilyManager.GetFamilyTable(familyId, libraryId, true)
                    .ToObject<InventorFamilyTable>();

            if (familyTable.Columns.Columns.Any(x => x.ColumnId == "AE_PARTNUMBER"))
                return false;

            familyTable.Columns.Columns.Add(new InventorColumn
            {
                ColumnId = "AE_PARTNUMBER",
                ColumnwideValue = "",
                Custom = true,
                Hidden = false,
                KeyWeight = 0,
                Title = "AE_PARTNUMBER",
                UnitType = "",
                ValueType = "string"
            });

            app.ContentCenter.FamilyManager.EditFamilyTable(familyId, libraryId, familyTable.ToXmlString());
            return true;
        }

        public void DeleteFamily(string familyId, string libraryId)
        {
            var app = InventorHelper.GetAppReference();
            app.ContentCenter.FamilyManager.DeleteFamily(familyId, libraryId);
        }

        public InventorLibrary GetLibraryByInternalName(string libraryId)
        {
            return GetServerLibraries().SingleOrDefault(x => x.InternalName == libraryId);
        }

        public IEnumerable<InventorFamily> GetFamiliesByInternalName(string familyId)
        {
            return GetAllFamilies().Where(x => x.InternalName == familyId);
        }
    }
}