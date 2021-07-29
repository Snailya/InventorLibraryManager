using System;
using System.Linq;
using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.Core.DbModels;
using JetSnail.InventorLibraryManager.Core.Domains;
using JetSnail.InventorLibraryManager.Core.DTOs;
using JetSnail.InventorLibraryManager.UseCase.DataStores;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JetSnail.InventorLibraryManager.Service.WebAPI.Controllers
{
	[ApiController]
	[ApiVersion("1.0")] // [Route("api/v{version:apiVersion}/[controller]")]
	[Route("api/[controller]")]
	public class GroupsController : ControllerBase
	{
		private readonly IGroupRepository _groupRepository;
		private readonly ILogger<GroupsController> _logger;

		public GroupsController(ILogger<GroupsController> logger, IGroupRepository groupRepository)
		{
			_logger = logger;
			_groupRepository = groupRepository;
		}

		/// <summary>
		///     获取数据库中存储的分组信息。
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult> GetAsync()
		{
			var groups = (await _groupRepository.GetAllAsync()).ToArray();

			if (!groups.Any()) return Ok(Array.Empty<object>());

			return Ok(groups.Select(x => new GroupDto
			{
				DisplayName = x.DatabaseModel.DisplayName,
				Id = x.DatabaseModel?.Id,
				ShortName = x.DatabaseModel?.ShortName
			}));
		}

		[HttpGet("{id:int}", Name = nameof(GetGroupByIdAsync))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetGroupByIdAsync(int id)
		{
			var group = await _groupRepository.GetByIdAsync(id);
			if (group.DatabaseModel == null) return NotFound();

			return Ok(new GroupDto
			{
				DisplayName = group.DatabaseModel.DisplayName, Id = group.DatabaseModel.Id,
				ShortName = group.DatabaseModel.ShortName
			});
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
		public async Task<IActionResult> AddAsync([FromBody] GroupDto dto)
		{
			try
			{
				var group = await _groupRepository.InsertAsync(new Group
				{
					DatabaseModel = new DatabaseGroup
					{
						DisplayName = dto.DisplayName, HasSynchronized = true, Id = dto.Id ?? default(int),
						ShortName = dto.ShortName
					}
				});
				return CreatedAtRoute(nameof(GetGroupByIdAsync), new {id = group.DatabaseModel.Id},
					new GroupDto
					{
						DisplayName = group.DatabaseModel.DisplayName, Id = group.DatabaseModel.Id,
						ShortName = group.DatabaseModel.ShortName
					});
			}
			catch (DbUpdateException dbUpdateException)
			{
				if (dbUpdateException.InnerException is not SqlException sqlException) return UnprocessableEntity();

				if (sqlException.Number == 8152)
					return UnprocessableEntity("Length of the ShortName property must be 3.");
				return UnprocessableEntity(dbUpdateException.Message);
			}
		}

		[HttpPatch]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
		public async Task<IActionResult> UpdateAsync([FromBody] GroupDto dto)
		{
			try
			{
				await _groupRepository.UpdateAsync(new Group
				{
					DatabaseModel = new DatabaseGroup
					{
						DisplayName = dto.DisplayName, HasSynchronized = true, Id = dto.Id ?? default(int),
						ShortName = dto.ShortName
					}
				});
				return await GetGroupByIdAsync(dto.Id ?? default(int));
			}
			catch (DbUpdateException dbUpdateException)
			{
				if (dbUpdateException.InnerException is not SqlException sqlException) return NotFound();

				if (sqlException.Number == 8152)
					return UnprocessableEntity("Length of the ShortName property must be 3.");
				return UnprocessableEntity(dbUpdateException.Message);
			}
		}

		[HttpDelete("{id:int}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> DeleteAsync(int id)
		{
			try
			{
				await _groupRepository.DeleteAsync(id);
				return NoContent();
			}
			catch (ArgumentNullException)
			{
				return NotFound();
			}
			catch (Exception e)
			{
				return UnprocessableEntity(e.Message);
			}
		}
	}
}