using System;
using System.Linq;
using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.Core.DbModels;
using JetSnail.InventorLibraryManager.Core.DTOs;
using JetSnail.InventorLibraryManager.UseCase.DataStores;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JetSnail.InventorLibraryManager.Service.WebAPI.Controllers
{
    /// <inheritdoc />
    [ApiController]
    [ApiVersion("1.0")]
    // [Route("api/v{version:apiVersion}/[controller]")]
    [Route("api/[controller]")]
    public class FamiliesController : ControllerBase
    {
        private readonly IFamilyRepository _familyRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly ILibraryRepository _libraryRepository;
        private readonly ILogger<FamiliesController> _logger;

        /// <inheritdoc />
        public FamiliesController(ILogger<FamiliesController> logger, ILibraryRepository libraryRepository,
            IFamilyRepository familyRepository,
            IGroupRepository groupRepository)
        {
            _logger = logger;
            _libraryRepository = libraryRepository;
            _familyRepository = familyRepository;
            _groupRepository = groupRepository;
        }

        /// <summary>
        ///     获取当前Inventor中加载的族信息。
        ///     族信息由Inventor模型信息和数据库模型信息两部分构成。
        /// </summary>
        /// <returns>A list of <see cref="FamilyDto" />.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAsync()
        {
            try
            {
                var families = (await _familyRepository.GetAllAsync()).ToArray();

                if (!families.Any()) return Ok(Array.Empty<object>());

                return Ok(families.Select(x => new FamilyDto
                {
                    DisplayName = x.InventorModel.DisplayName,
                    FamilyInternalName = x.InventorModel.InternalName,
                    Group = x.DatabaseModel != null
                        ? new GroupDto
                        {
                            DisplayName = x.DatabaseModel?.Group?.DisplayName,
                            Id = x.DatabaseModel?.Group?.Id,
                            ShortName = x.DatabaseModel?.Group?.ShortName
                        }
                        : null,
                    Id = x.DatabaseModel?.Id,
                    LibraryInternalName = x.InventorModel.Library
                }));
            }
            catch (Exception e)
            {
                return e.HResult switch
                {
                    -2147221021 => StatusCode(500, "Unable to get Inventor from ROT."), // Inventor没有启动
                    _ => UnprocessableEntity()
                };
            }
        }

        /// <summary>
        ///     获取指定族的信息，族必须已加载至Inventor中。
        /// </summary>
        /// <param name="id">族在Inventor中的标识符，GUID字符串</param>
        /// <param name="libraryId">库的标识符，当未指定时，将防返回RevisionTime最新的族</param>
        /// <returns>
        ///     <see cref="FamilyDto" />
        /// </returns>
        [HttpGet("{id}", Name = nameof(GetFamilyByIdAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFamilyByIdAsync(string id, [FromQuery] string libraryId = null)
        {
            try
            {
                var family = await _familyRepository.GetByIdAsync(id, libraryId);

                if (family == null) return NotFound();

                return Ok(new FamilyDto
                {
                    DisplayName = family.InventorModel.DisplayName,
                    FamilyInternalName = family.InventorModel.InternalName,
                    Group = family.DatabaseModel != null
                        ? new GroupDto
                        {
                            DisplayName = family.DatabaseModel?.Group?.DisplayName,
                            Id = family.DatabaseModel?.Group?.Id,
                            ShortName = family.DatabaseModel?.Group?.ShortName
                        }
                        : null,
                    Id = family.DatabaseModel?.Id,
                    LibraryInternalName = family.InventorModel.Library
                });
            }
            catch (Exception e)
            {
                return e.HResult switch
                {
                    -2147221021 => StatusCode(500, "Unable to get Inventor from ROT."), // Inventor没有启动
                    _ => UnprocessableEntity()
                };
            }
        }

        /// <summary>
        ///     将族从标准库中复制到自定义库，或从自定义库移动至新的自定义库。
        ///     复制或移动族不会存储族的Group信息，因而也不会更新族零件的PARTNUMBER。
        ///     需要手动调用<see cref="UpdateAsync" />方法和<see cref="UpdateFamilyPartNumbersAsync" />方法更新。
        /// </summary>
        /// <param name="dto">
        ///     <see cref="CopyOrMoveFamilyDto" />
        /// </param>
        /// <returns>
        ///     <see cref="FamilyDto" />
        /// </returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> CopyOrMoveAsync([FromBody] CopyOrMoveFamilyDto dto)
        {
            try
            {
                var family = await _familyRepository.MoveAsync(dto.FamilyInternalName, dto.FromLibraryInternalName,
                    dto.ToLibraryInternalName, dto.Associative);

                return AcceptedAtRoute(nameof(GetFamilyByIdAsync), new { id = family.InventorModel.InternalName });
            }
            catch (Exception e)
            {
                return e.HResult switch
                {
                    -2146233079 => UnprocessableEntity(e.Message),
                    -2147221021 => StatusCode(500, "Unable to get Inventor from ROT."),
                    _ => UnprocessableEntity()
                };
            }
        }

        /// <summary>
        ///     更新族的分组信息。
        ///     更新族的分组信息并不会更新族零件的PARTNUMBER，需要手动调用<see cref="UpdateFamilyPartNumbersAsync" />方法更新。
        /// </summary>
        /// <param name="dto">
        ///     <see cref="UpdateFamilyDto" />
        /// </param>
        /// <returns>
        ///     <see cref="FamilyDto" />
        /// </returns>
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateFamilyDto dto)
        {
            // todo: start here to test
            try
            {
                if (dto.GroupId == null) return UnprocessableEntity();
                var group = await _groupRepository.GetByIdAsync((int)dto.GroupId);
                if (group.DatabaseModel == null)
                    return UnprocessableEntity("Group not exist.");

                var family = await _familyRepository.GetByIdAsync(dto.FamilyInternalName);

                // if this family is never added to the Database, means it's either a read-only library which can't perform update action or user didn't follow the correct workflow to create it in custom library
                if (family.DatabaseModel == null)
                {
                    var library = await _libraryRepository.GetByIdAsync(family.InventorModel.Library);

                    // not-allowed action
                    if (library.InventorModel.ReadOnly)
                        return UnprocessableEntity("Edit action in a read-only library is not allowed.");

                    // user not follow correct workflow
                    family.DatabaseModel = new DatabaseFamily
                    {
                        InternalName = dto.FamilyInternalName
                    };
                }

                family.DatabaseModel.Group = group.DatabaseModel;

                await _familyRepository.UpdateAsync(family);

                return Ok(await GetFamilyByIdAsync(dto.FamilyInternalName));
            }
            catch (Exception e)
            {
                return e.HResult switch
                {
                    -2147024809 => NotFound(),
                    -2147221021 => StatusCode(500, "Unable to get Inventor from ROT."),
                    _ => UnprocessableEntity()
                };
            }
        }

        /// <summary>
        ///     获取指定族的零件信息。
        ///     零件信息由Inventor模型信息和数据库模型信息两部分构成。注意Inventor信息中和数据库信息中的PARTNUMBER可能不一致，可以使用<see cref="UpdateFamilyPartNumbersAsync" />
        ///     或
        ///     <see cref="UpdatePartNumberByIdAsync" />更新Inventor模型中的PARTNUMBER。
        /// </summary>
        /// <param name="id">族在Inventor中的标识符，GUID字符串</param>
        /// <returns>A list of <see cref="PartDto" />.</returns>
        [HttpGet("{id}/parts", Name = nameof(GetFamilyPartNumbersAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFamilyPartNumbersAsync(string id)
        {
            try
            {
                var family = await _familyRepository.GetByIdAsync(id);
                var parts = await _familyRepository.GetPartNumbers(family);

                return Ok(parts.Select(x => new PartDto
                {
                    InventorFileName = x.InventorModel.Cells.SingleOrDefault(c => c.ColumnId == "FILENAME")?.Text,
                    Id = x.DatabaseModel?.Id,
                    PartInternalName = x.InventorModel.InternalName,
                    InventorPartNumber =
                        x.InventorModel.Cells.SingleOrDefault(c => c.ColumnId == "AE_PARTNUMBER")?.Text,
                    DatabasePartNumber = x.DatabaseModel?.PartNumber
                }));
            }
            catch (Exception e)
            {
                return e.HResult switch
                {
                    -2147024809 => NotFound(),
                    -2147221021 => StatusCode(500, "Unable to get Inventor from ROT."),
                    -2147467259 => UnprocessableEntity($"Parameter {nameof(id)} property must be Guid string."),
                    _ => UnprocessableEntity()
                };
            }
        }

        /// <summary>
        ///     更新族零件的PARTNUMBER，要求族的Group不为空。
        ///     执行该操作会检查族表是否包含名为AE_PARTNUMBER的列，若不存在，创建该列，并将数据库信息中的PARTNUMBER值写入如该列。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("{id}/parts", Name = nameof(UpdateFamilyPartNumbersAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateFamilyPartNumbersAsync(string id)
        {
            try
            {
                var family = await _familyRepository.GetByIdAsync(id);
                var parts = await _familyRepository.UpdatePartNumbers(family);

                return Ok(parts.Select(x => new PartDto
                {
                    InventorFileName = x.InventorModel.Cells.SingleOrDefault(c => c.ColumnId == "FILENAME")?.Text,
                    Id = x.DatabaseModel.Id,
                    PartInternalName = x.InventorModel.InternalName,
                    InventorPartNumber =
                        x.InventorModel.Cells.SingleOrDefault(c => c.ColumnId == "AE_PARTNUMBER")?.Text,
                    DatabasePartNumber = x.DatabaseModel.PartNumber
                }));
            }
            catch (Exception e)
            {
                return e.HResult switch
                {
                    -2147024809 => NotFound(),
                    -2147221021 => StatusCode(500, "Unable to get Inventor from ROT."),
                    -2147467259 => UnprocessableEntity($"Parameter {nameof(id)} property must be Guid string."),
                    _ => UnprocessableEntity()
                };
            }
        }

        /// <summary>
        ///     更新指定族零件的PARTNUMBER，要求族的Group不为空。
        ///     执行该操作会检查族表是否包含名为AE_PARTNUMBER的列，若不存在，创建该列，并将数据库信息中的PARTNUMBER值写入如该列。
        /// </summary>
        /// <param name="familyId"></param>
        /// <param name="partId"></param>
        /// <returns></returns>
        [HttpPatch("{familyId}/parts/{partId}", Name = nameof(UpdatePartNumberByIdAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePartNumberByIdAsync(string familyId, string partId)
        {
            try
            {
                var family = await _familyRepository.GetByIdAsync(familyId);
                var part = await _familyRepository.UpdatePartNumber(family, partId);

                return Ok(new PartDto
                {
                    InventorFileName = part.InventorModel.Cells.SingleOrDefault(c => c.ColumnId == "FILENAME")?.Text,
                    Id = part.DatabaseModel.Id,
                    PartInternalName = part.InventorModel.InternalName,
                    InventorPartNumber = part.DatabaseModel.PartNumber
                });
            }
            catch (Exception e)
            {
                return e.HResult switch
                {
                    -2147024809 => NotFound(),
                    -2147221021 => StatusCode(500, "Unable to get Inventor from ROT."),
                    _ => UnprocessableEntity()
                };
            }
        }
    }
}