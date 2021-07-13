using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Inventor;

namespace JetSnail.InventorLibraryManager.Service
{
    public class InventorService : IInventorService
    {
        private readonly ContentCenterContext _context;
        private readonly IPartNumberService _partNumber;

        public InventorService(ContentCenterContext context, IPartNumberService partNumber)
        {
            _context = context;
            _partNumber = partNumber;
        }

        #region Group

        public async Task<ResultDto> UpdatePartNumbersByGroup(UpdatePartNumbersByGroupDto dto)
        {
            try
            {
                var app = InventorHelper.GetAppReference();
                var actionResult = new List<(string, int)>();

                // update inventor content
                app.ContentCenter.ForceRefreshCache();

                // get all family InternalNames that under this group
                var familyEntities =
                    await _context.Families.Include(e => e.Group).AsNoTracking().Where(e => e.Group.Id == dto.GroupId)
                        .ToListAsync();

                foreach (var xmlFamily in familyEntities.Select(e => app.ContentCenter.FamilyManager
                    .GetFamily(e.InternalName)
                    .ToObject<XmlFamily>()))
                    actionResult.Add(new ValueTuple<string, int>(xmlFamily.InternalName,
                        (await UpdateFamilyPartNumbersAsync(xmlFamily.InternalName, xmlFamily.Library, dto.CheckMode))
                        .Item1));

                if (actionResult.All(x => x.Item2 == 1))
                {
                    (await _context.Groups.FindAsync(dto.GroupId)).HasSynchronized = true;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new ResultDto {Status = 0, Message = e.Message};
            }

            return new ResultDto {Status = 1};
        }

        #endregion

        private async Task<(int, string)> UpdateFamilyPartNumbersAsync(string familyInternalName,
            string libraryInternalName,
            bool checkMode = false)
        {
            try
            {
                var app = InventorHelper.GetAppReference();

                // update inventor content
                app.ContentCenter.ForceRefreshCache();

                // get family table through xml
                var xmlFamilyTable =
                    app.ContentCenter.FamilyManager.GetFamilyTable(familyInternalName).ToObject<XmlFamilyTable>();

                // get family entity by InternalName
                var familyEntity = _context.Families.Include(e => e.Group).Include(e => e.Parts)
                    .Single(e => e.InternalName == familyInternalName);

                foreach (var partEntity in familyEntity.Parts)
                {
                    if (checkMode)
                        if (partEntity.PartNumber.Substring(0, 3) != familyEntity.Group.ShortName)
                            partEntity.PartNumber = _partNumber.GeneratePartNumber(familyEntity.Group.Id);
                    xmlFamilyTable.Rows.Rows.Single(r => r.InternalName == partEntity.InternalName).Text =
                        partEntity.PartNumber;
                }

                // save changes to inventor
                app.ContentCenter.FamilyManager.EditFamilyTable(familyInternalName, libraryInternalName,
                    xmlFamilyTable.ToXml());

                // save changes to database
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new ValueTuple<int, string>(0, e.Message);
            }

            return new ValueTuple<int, string>(1, string.Empty);
        }


        #region Part

        public async Task<ResultDto> DeletePartAsync(DeletePartDto dto)
        {
            try
            {
                var app = InventorHelper.GetAppReference();

                // update inventor content
                app.ContentCenter.ForceRefreshCache();

                // get family through xml
                var xmlFamilyTable =
                    app.ContentCenter.FamilyManager.GetFamilyTable(dto.FamilyInternalName).ToObject<XmlFamilyTable>();
                var xmlPart = xmlFamilyTable.Rows.Rows.SingleOrDefault(r => r.InternalName == dto.InternalName);
                if (xmlPart != null)
                {
                    xmlFamilyTable.Rows.Rows.Remove(xmlPart);
                    // save changes to inventor
                    app.ContentCenter.FamilyManager.EditFamilyTable(dto.FamilyInternalName, dto.LibraryInternalName,
                        xmlFamilyTable.ToXml());
                }

                // save changes to database
                var partEntity = _context.Parts.SingleOrDefault(e => e.InternalName == dto.InternalName);
                if (partEntity != null)
                {
                    _context.Parts.Remove(partEntity);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new ResultDto {Status = 0, Message = e.Message};
            }

            return new ResultDto {Status = 1};
        }

        #endregion

        #region Family

        public async Task<IEnumerable<FamilyDto>> GetFamiliesAsync(string filter, string libraryInternalName)
        {

        }

        public async Task<FamilyDto> GetFamilyAsync(string familyInternalName)
        {
            // update inventor content
            var app = InventorHelper.GetAppReference();
            app.ContentCenter.ForceRefreshCache();

            // get library dictionary
            var libraries = (await GetLibrariesAsync()).ToDictionary(x => x.InternalName, x => x.DisplayName);

            // get family through xml
            var xmlFamily = app.ContentCenter.FamilyManager.GetFamily(familyInternalName).ToObject<XmlFamily>();
            if (xmlFamily == null) return null;

            // construct dto
            var familyDto = new FamilyDto
            {
                // database data
                Id = _context.Families.AsNoTracking().SingleOrDefault(e => e.InternalName == xmlFamily.InternalName)
                    ?.Id,
                Group = _context.Families.AsNoTracking()
                    .SingleOrDefault(e => e.InternalName == xmlFamily.InternalName)?.Group
                    .ToDto(),

                // inventor data
                InternalName = xmlFamily.InternalName,
                DisplayName = xmlFamily.DisplayName,
                LibraryInternalName = xmlFamily.Library,
                LibraryDisplayName = libraries[xmlFamily.Library]
            };

            return familyDto;
        }

        public async Task<FamilyDetailDto> GetFamilyDetailAsync(string familyInternalName)
        {
            // update inventor content
            var app = InventorHelper.GetAppReference();
            app.ContentCenter.ForceRefreshCache();

            // get library dictionary
            var libraries = (await GetLibrariesAsync()).ToDictionary(x => x.InternalName, x => x.DisplayName);

            // get family through xml
            var xmlFamily = app.ContentCenter.FamilyManager.GetFamily(familyInternalName).ToObject<XmlFamily>();
            var xmlFamilyTable =
                app.ContentCenter.FamilyManager.GetFamilyTable(familyInternalName, FullyPopulated: true)
                    .ToObject<XmlFamilyTable>();

            // construct dto
            var familyDto = new FamilyDetailDto
            {
                // database data
                Id = _context.Families.AsNoTracking().SingleOrDefault(e => e.InternalName == xmlFamily.InternalName)
                    ?.Id,
                Group = _context.Families.Include(e => e.Group).AsNoTracking()
                    .SingleOrDefault(e => e.InternalName == xmlFamily.InternalName)
                    ?.Group
                    .ToDto(),

                // inventor data
                InternalName = xmlFamily.InternalName,
                DisplayName = xmlFamily.DisplayName,
                LibraryInternalName = xmlFamily.Library,
                LibraryDisplayName = libraries[xmlFamily.Library],

                Parts = xmlFamilyTable.Rows.Rows.Select(r => new PartDto
                {
                    // database data
                    Id = _context.Parts.AsNoTracking()
                        .SingleOrDefault(entity => entity.InternalName == r.InternalName)?.Id,
                    PartNumber = _context.Parts.AsNoTracking()
                        .SingleOrDefault(entity => entity.InternalName == r.InternalName)
                        ?.PartNumber,

                    // inventor data
                    InternalName = r.InternalName,
                    FileName = r.Cells.Single(c => c.ColumnId == "FILENAME").Text
                }).ToArray()
            };
            return familyDto;
        }

        public async Task<ResultDto> SaveFamilyCopyAsAsync(SaveFamilyCopyAsDto dto)
        {
            var hasCopied = false;

            try
            {
                var app = InventorHelper.GetAppReference();

                // update inventor content
                app.ContentCenter.ForceRefreshCache();

                // execute copy action, save to inventor
                app.ContentCenter.FamilyManager.CopyFamily(dto.InternalName, dto.FromLibraryInternalName,
                    dto.ToLibraryInternalName,
                    dto.Associative);
                hasCopied = true;

                // get family through xml
                var xmlFamilyTable =
                    app.ContentCenter.FamilyManager.GetFamilyTable(dto.InternalName, FullyPopulated: true)
                        .ToObject<XmlFamilyTable>();

                // loop the xmlFamilyTable to write PartNumber
                foreach (var xmlRow in xmlFamilyTable.Rows.Rows)
                    xmlRow.Cells.Single(c => c.ColumnId == "PARTNUMBER").Text =
                        _partNumber.GeneratePartNumber(dto.GroupId);

                // save to Inventor
                app.ContentCenter.FamilyManager.EditFamilyTable(dto.InternalName, dto.ToLibraryInternalName,
                    xmlFamilyTable.ToXml());

                // save to database
                var familyEntity =
                    new FamilyEntity
                    {
                        Group = await _context.Groups.FindAsync(dto.GroupId),
                        InternalName = dto.InternalName,
                        Parts = xmlFamilyTable.Rows.Rows.Select(r => new PartEntity
                        {
                            InternalName = r.InternalName,
                            PartNumber = r.Cells.Single(c => c.ColumnId == "PARTNUMBER").Text
                        }).ToList()
                    };
                _context.Families.Add(familyEntity);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                if (hasCopied)
                    InventorHelper.GetAppReference().ContentCenter.FamilyManager
                        .DeleteFamily(dto.InternalName, dto.ToLibraryInternalName);
                return new ResultDto {Status = 0, Message = e.Message};
            }

            return new ResultDto {Status = 1};
        }

        public Task<ResultDto> MoveFamilyToAsync(MoveFamilyToDto dto)
        {
            try
            {
                var app = InventorHelper.GetAppReference();

                // update inventor content
                app.ContentCenter.ForceRefreshCache();

                // execute copy action, save to inventor
                app.ContentCenter.FamilyManager.CopyFamily(dto.InternalName, dto.FromLibraryInternalName,
                    dto.ToLibraryInternalName);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return Task.FromResult(new ResultDto {Status = 0, Message = e.Message});
            }

            return Task.FromResult(new ResultDto {Status = 1});
        }

        public async Task<ResultDto> DeleteFamilyAsync(DeleteFamilyDto dto)
        {
            try
            {
                // update inventor content
                var app = InventorHelper.GetAppReference();
                app.ContentCenter.ForceRefreshCache();

                // execute delete action
                app.ContentCenter.FamilyManager.DeleteFamily(dto.InternalName, dto.LibraryInternalName);

                // save to database
                var familyEntityToRemove = _context.Families.SingleOrDefault(e => e.InternalName == dto.InternalName);
                if (familyEntityToRemove != null) _context.Families.Remove(familyEntityToRemove);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new ResultDto {Status = 0, Message = e.Message};
            }

            return new ResultDto {Status = 1};
        }

        public async Task<ResultDto> UpdateFamilyPartNumbers(UpdateFamilyPartNumbersDto dto)
        {
            var (item1, item2) =
                await UpdateFamilyPartNumbersAsync(dto.FamilyInternalName, dto.LibraryInternalName, dto.CheckMode);
            return new ResultDto {Status = item1, Message = item2};
        }

        #endregion
    }
}