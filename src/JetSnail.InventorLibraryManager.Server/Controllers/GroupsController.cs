using System;
using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.Server.Exceptions;
using JetSnail.InventorLibraryManager.UseCase.GroupScope;
using JetSnail.InventorLibraryManager.UseCase.GroupScope.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace JetSnail.InventorLibraryManager.Server.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class GroupsController : ControllerBase
    {
        private readonly IAddGroupUseCase _addGroupUseCase;
        private readonly ICheckNameUseCase _checkNameUseCase;
        private readonly IDeleteGroupIfNoDerivedUseCase _deleteGroupUseCase;
        private readonly IGetGroupByIdUseCase _getGroupByIdUseCase;
        private readonly IGetGroupsUseCase _getGroupsUseCase;

        private readonly IUpdateGroupThenUpdatePartNumberIfShortNameChangeUseCase
            _updateGroupUseCase;

        public GroupsController(
            IGetGroupsUseCase getGroupsUseCase,
            IGetGroupByIdUseCase getGroupByIdUseCase,
            ICheckNameUseCase checkNameUseCase,
            IAddGroupUseCase addGroupUseCase,
            IUpdateGroupThenUpdatePartNumberIfShortNameChangeUseCase
                updateGroupUseCase,
            IDeleteGroupIfNoDerivedUseCase deleteGroupUseCase
        )
        {
            _addGroupUseCase = addGroupUseCase;
            _checkNameUseCase = checkNameUseCase;
            _deleteGroupUseCase = deleteGroupUseCase;
            _getGroupsUseCase = getGroupsUseCase;
            _getGroupByIdUseCase = getGroupByIdUseCase;
            _updateGroupUseCase =
                updateGroupUseCase;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllGroupsAsync()
        {
            return Ok(await _getGroupsUseCase.Execute());
        }

        [HttpGet("{id:int}", Name = nameof(GetGroupByIdAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetGroupByIdAsync(int id)
        {
            try
            {
                return Ok(await _getGroupByIdUseCase.Execute(id));
            }
            catch (ResourceNotFoundException resourceNotFoundException)
            {
                return NotFound(resourceNotFoundException.Message);
            }
        }

        [HttpPost("check-name")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> CheckName([FromBody] CheckNameDto dto, [FromQuery] string current)
        {
            var (result, message) = await _checkNameUseCase.Execute(dto.Value, current);
            return result ? Ok(message) : UnprocessableEntity(message);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> AddGroupAsync([FromBody] AddGroupDto dto)
        {
            try
            {
                var group = await _addGroupUseCase.Execute(dto.Name, dto.ShortName);
                return CreatedAtRoute(nameof(GetGroupByIdAsync), new {group.Id},
                    group);
            }
            catch (InvalidOperationException invalidOperationException)
            {
                return UnprocessableEntity(invalidOperationException.Message);
            }
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdateGroupAsync(int id, [FromBody] JsonPatchDocument<GroupDto> doc)
        {
            try
            {
                await _updateGroupUseCase.Execute(id, doc);
                return AcceptedAtRoute(nameof(GetGroupByIdAsync), new {id});
            }
            catch (ResourceNotFoundException resourceNotFoundException)
            {
                return UnprocessableEntity(resourceNotFoundException.Message);
            }
            catch (InvalidOperationException invalidOperationException)
            {
                return UnprocessableEntity(invalidOperationException.Message);
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> DeleteGroupAsync(int id)
        {
            try
            {
                await _deleteGroupUseCase.Execute(id);
                return NoContent();
            }
            catch (ResourceNotFoundException resourceNotFoundException)
            {
                return UnprocessableEntity(resourceNotFoundException.Message);
            }
            catch (InvalidOperationException invalidOperationException)
            {
                return UnprocessableEntity(invalidOperationException.Message);
            }
        }
    }
}