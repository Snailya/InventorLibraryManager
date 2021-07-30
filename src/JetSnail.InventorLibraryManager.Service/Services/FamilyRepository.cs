using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventor;
using JetSnail.InventorLibraryManager.Core.DbModels;
using JetSnail.InventorLibraryManager.Core.Domains;
using JetSnail.InventorLibraryManager.Core.InventorModels;
using JetSnail.InventorLibraryManager.Data;
using JetSnail.InventorLibraryManager.UseCase.DataStores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JetSnail.InventorLibraryManager.Service.Services
{
    public class FamilyRepository : IFamilyRepository
    {
        private readonly ContentCenterContext _context;
        private readonly IGroupRepository _groupRepository;
        private readonly ILibraryRepository _libraryRepository;
        private readonly ILogger<FamilyRepository> _logger;

        public FamilyRepository(ILogger<FamilyRepository> logger, ContentCenterContext context,
            ILibraryRepository libraryRepository, IGroupRepository groupRepository)
        {
            _logger = logger;
            _context = context;
            _libraryRepository = libraryRepository;
            _groupRepository = groupRepository;
        }

        public Task<IEnumerable<Family>> GetAllAsync(string libraryId)
        {
            // update inventor content
            var app = InventorHelper.GetAppReference();
            app.ContentCenter.ForceRefreshCache();

            // get families through xml
            var xmlTableOfContents =
                ((string)app.ContentCenter.GetTableOfContents(GeneralDataTypeEnum.kVTableGeneralData, libraryId))
                ?.ToObject<InventorTableOfContents>();
            if (xmlTableOfContents == null) return Task.FromResult<IEnumerable<Family>>(null);

            return Task.FromResult(xmlTableOfContents.Families.Families.Select(x => new Family
            {
                DatabaseModel = _context.Families.Include(e => e.Group).Include(e => e.Parts).AsNoTracking()
                    .SingleOrDefault(e => e.InternalName == x.InternalName),
                InventorModel = x
            }));
        }

        public async Task<Family> GetByIdAsync(string id, string libraryId)
        {
            // update inventor content
            var app = InventorHelper.GetAppReference();
            app.ContentCenter.ForceRefreshCache();

            // XXX: if family is copy or moved from other library, must specified libraryId. therefore use get all method and select from it.
            return (await GetAllAsync(libraryId)).SingleOrDefault(x => x.InventorModel.InternalName == id);
        }

        public async Task<Family> CopyOrMoveAsync(string id, string fromLibraryId, string toLibraryId, bool associative)
        {
            // because CopyFamily method would hack the readonly 
            // check if this is in readonly library
            var toLibrary = await _libraryRepository.GetByIdAsync(toLibraryId);
            if (toLibrary.InventorModel == null)
                throw new InvalidOperationException(
                    "Can't find the target library.");
            if (toLibrary.InventorModel.ReadOnly)
                throw new InvalidOperationException("Copy or move family into a read-only library is not allowed.");

            // because CopyFamily method could hack even the from library or to library not loaded
            // check if the both libraries are loaded
            var family = await GetByIdAsync(id, fromLibraryId);
            if (family == null)
                throw new InvalidOperationException(
                    "Can't find family in the library.");

            var app = InventorHelper.GetAppReference();
            app.ContentCenter.ForceRefreshCache();

            app.ContentCenter.FamilyManager.CopyFamily(id, family.InventorModel.Library,
                toLibraryId);
            // app.ContentCenter.FamilyManager.PublishFamily(app.ContentCenter.FamilyManager.GetFamily(id, toLibraryId),toLibraryId);

            // if the origin library is not read-only, delete origin family from Inventor
            var fromLibrary = await _libraryRepository.GetByIdAsync(fromLibraryId);
            if (!fromLibrary.InventorModel.ReadOnly) app.ContentCenter.FamilyManager.DeleteFamily(id, fromLibraryId);

            family = await GetByIdAsync(id, toLibraryId);
            if (family.DatabaseModel != null) return family;

            // record into database, pay attention that the CreatedAt property is auto set when create the instance
            family.DatabaseModel = new DatabaseFamily
            {
                InternalName = id
            };
            _context.Families.Add(family.DatabaseModel);
            await _context.SaveChangesAsync();

            return family;
        }

        public async Task<IEnumerable<Part>> UpdatePartNumbers(Family obj)
        {
            if (obj.DatabaseModel.Group == null)
                throw new InvalidOperationException("Unable to update PARTNUMBER if no group the family belongs to.");

            var app = InventorHelper.GetAppReference();
            app.ContentCenter.ForceRefreshCache();

            // get family table through xml
            var xmlFamilyTable =
                app.ContentCenter.FamilyManager.GetFamilyTable(obj.InventorModel.InternalName, FullyPopulated: true)
                    .ToObject<InventorFamilyTable>();

            // check if PARTNUMBER column exist
            if (xmlFamilyTable.Columns.Columns.All(x => x.ColumnId != "AE_PARTNUMBER"))
            {
                xmlFamilyTable.Columns.Columns.Add(new InventorColumn
                {
                    ColumnId = "AE_PARTNUMBER",
                    KeyWeight = 1,
                    Title = "AE_PARTNUMBER",
                    ValueType = "string"
                });
                foreach (var inventorRow in xmlFamilyTable.Rows.Rows)
                    inventorRow.Cells.Add(new InventorCell { ColumnId = "AE_PARTNUMBER" });
            }

            // loop to persist part into database, pay attention that the CreatedAt property is auto generated
            foreach (var part in from inventorRow in xmlFamilyTable.Rows.Rows
                let part = obj.DatabaseModel.Parts.SingleOrDefault(x => x.InternalName == inventorRow.InternalName)
                where part == null
                select new DatabasePart { InternalName = inventorRow.InternalName }) obj.DatabaseModel.Parts.Add(part);

            _context.Families.Update(obj.DatabaseModel);
            await _context.SaveChangesAsync();

            // loop to write AE_PARTNUMBER into inventor
            foreach (var inventorRow in xmlFamilyTable.Rows.Rows)
                inventorRow.Cells.Single(x => x.ColumnId == "AE_PARTNUMBER").Text = obj.DatabaseModel.Parts
                    .Single(x => x.InternalName == inventorRow.InternalName).PartNumber;

            app.ContentCenter.FamilyManager.EditFamilyTable(obj.InventorModel.InternalName, obj.InventorModel.Library,
                xmlFamilyTable.ToXmlString());

            return xmlFamilyTable.Rows.Rows.Select(x => new Part
            {
                DatabaseModel = obj.DatabaseModel.Parts
                    .Single(e => e.InternalName == x.InternalName),
                InventorModel = x
            });
        }

        public async Task<Part> UpdatePartNumber(Family obj, string partId)
        {
            if (obj.DatabaseModel.Group == null)
                throw new InvalidOperationException("Unable to update PARTNUMBER if no group the family belongs to.");

            var app = InventorHelper.GetAppReference();
            app.ContentCenter.ForceRefreshCache();

            // get family table through xml
            var xmlFamilyTable =
                app.ContentCenter.FamilyManager.GetFamilyTable(obj.InventorModel.InternalName, FullyPopulated: true)
                    .ToObject<InventorFamilyTable>();

            var xmlRow = xmlFamilyTable.Rows.Rows.SingleOrDefault(r => r.InternalName == partId);
            if (xmlRow == null)
                throw new InvalidOperationException(
                    $"Part {partId} either not exist or not under family {obj.InventorModel.DisplayName}.");

            // check if PARTNUMBER column exist
            if (xmlFamilyTable.Columns.Columns.All(x => x.ColumnId != "AE_PARTNUMBER"))
            {
                xmlFamilyTable.Columns.Columns.Add(new InventorColumn
                {
                    ColumnId = "AE_PARTNUMBER",
                    KeyWeight = 1,
                    Title = "AE_PARTNUMBER",
                    ValueType = "string"
                });
                xmlRow.Cells.Add(new InventorCell { ColumnId = "AE_PARTNUMBER" });
            }

            // persist part into database
            var part = obj.DatabaseModel.Parts.SingleOrDefault(x => x.InternalName == xmlRow.InternalName);
            if (part == null)
            {
                part = new DatabasePart { InternalName = partId };  // create new part
                obj.DatabaseModel.Parts.Add(part);
            }

            _context.Families.Update(obj.DatabaseModel);
            await _context.SaveChangesAsync();

            // persist part into Inventor
            xmlRow.Cells.Single(x => x.ColumnId == "AE_PARTNUMBER").Text = part.PartNumber;
            app.ContentCenter.FamilyManager.EditFamilyTable(obj.InventorModel.InternalName, obj.InventorModel.Library,
                xmlFamilyTable.ToXmlString());

            return new Part
            {
                DatabaseModel = obj.DatabaseModel.Parts
                    .Single(e => e.InternalName == partId),
                InventorModel = xmlRow
            };
        }

        public Task<IEnumerable<Part>> GetPartNumbers(Family obj)
        {
            var app = InventorHelper.GetAppReference();
            app.ContentCenter.ForceRefreshCache();

            // get family table through xml
            var xmlFamilyTable =
                app.ContentCenter.FamilyManager.GetFamilyTable(obj.InventorModel.InternalName, FullyPopulated: true)
                    .ToObject<InventorFamilyTable>();

            return Task.FromResult(xmlFamilyTable.Rows.Rows.Select(x => new Part
            {
                DatabaseModel = obj.DatabaseModel?.Parts
                    .SingleOrDefault(e => e.InternalName == x.InternalName),
                InventorModel = x
            }));
        }

        public async Task UpdateAsync(string familyId, string libraryId, int groupId)
        {
            var group = await _groupRepository.GetByIdAsync(groupId);
            if (group.DatabaseModel == null)
                throw new InvalidOperationException("Group not exist.");

            var family = await GetByIdAsync(familyId, libraryId);
            if (family.InventorModel == null)
                throw new InvalidOperationException("Family not found.");

            var library = await _libraryRepository.GetByIdAsync(family.InventorModel.Library);
            if (library.InventorModel.ReadOnly)
                throw new InvalidOperationException("Update action in a read-only library is not allowed.");

            // if the user not follow the desire workflow, which means the family is in read/write library, but no record is in database
            family.DatabaseModel ??= new DatabaseFamily
            {
                InternalName = familyId
            };

            family.DatabaseModel.Group = group.DatabaseModel;

            // bug: unable to modify family property through FamilyManger.EditFamily, always return 0x80004005 Unspecified error
            var app = InventorHelper.GetAppReference();
            app.ContentCenter.ForceRefreshCache();

            _context.Families.Update(family.DatabaseModel);
            await _context.SaveChangesAsync();
        }
    }
}