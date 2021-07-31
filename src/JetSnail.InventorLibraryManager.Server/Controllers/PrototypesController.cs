using System;
using System.Linq;
using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.Server.Exceptions;
using JetSnail.InventorLibraryManager.UseCase.FamilyScope;
using JetSnail.InventorLibraryManager.UseCase.FamilyScope.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JetSnail.InventorLibraryManager.Server.Controllers
{
    /// <inheritdoc />
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PrototypesController : ControllerBase
    {
        private readonly IAssignGroupThenSynPartNumberUseCase _assignGroupUseCase;
        private readonly ICreateDerivativeThenSynPartNumberUseCase _createDerivativeUseCase;
        private readonly IDeleteDerivativeUseCase _deleteDerivativeUseCase;
        private readonly IGetDerivativesUseCase _getDerivativesUseCase;
        private readonly IGetPrototypeByIdUseCase _getPrototypeByIdUseCase;
        private readonly IGetPrototypesUseCase _getPrototypesUseCase;
        private readonly ISaveDerivativeThenSynPartNumberUseCase _saveDerivativeUseCase;
        private readonly ISyncDerivativePartNumberUseCase _syncDerivativePartNumberUseCase;

        /// <inheritdoc />
        public PrototypesController(
            IGetPrototypesUseCase getPrototypesUseCase,
            IGetPrototypeByIdUseCase getPrototypeByIdUseCase,
            IGetDerivativesUseCase getDerivativesUseCase,
            ICreateDerivativeThenSynPartNumberUseCase createDerivativeUseCase,
            ISaveDerivativeThenSynPartNumberUseCase saveDerivativeUseCase,
            ISyncDerivativePartNumberUseCase syncDerivativePartNumberUseCase,
            IAssignGroupThenSynPartNumberUseCase assignGroupUseCase,
            IDeleteDerivativeUseCase deleteDerivativeUseCase)
        {
            _getPrototypesUseCase = getPrototypesUseCase;
            _getPrototypeByIdUseCase = getPrototypeByIdUseCase;
            _getDerivativesUseCase = getDerivativesUseCase;
            _createDerivativeUseCase = createDerivativeUseCase;
            _saveDerivativeUseCase = saveDerivativeUseCase;
            _syncDerivativePartNumberUseCase = syncDerivativePartNumberUseCase;
            _assignGroupUseCase = assignGroupUseCase;
            _deleteDerivativeUseCase = deleteDerivativeUseCase;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPrototypesAsync()
        {
            return Ok(await _getPrototypesUseCase.Execute());
        }


        [HttpGet("{prototypeId:int}", Name = nameof(GetPrototypeByIdAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPrototypeByIdAsync(int prototypeId)
        {
            try
            {
                return Ok(await _getPrototypeByIdUseCase.Execute(prototypeId));
            }
            catch (ResourceNotFoundException resourceNotFoundException)
            {
                return NotFound(resourceNotFoundException.Message);
            }
        }

        [HttpGet("{prototypeId:int}/derivatives", Name = nameof(GetDerivativesAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDerivativesAsync(int prototypeId, [FromQuery] string libraryId)
        {
            try
            {
                return string.IsNullOrEmpty(libraryId)
                    ? Ok(await _getDerivativesUseCase.Execute(prototypeId))
                    : Ok((await _getDerivativesUseCase.Execute(prototypeId)).Where(x =>
                        x.Library.LibraryId == libraryId));
            }
            catch (ResourceNotFoundException resourceNotFoundException)
            {
                return NotFound(resourceNotFoundException.Message);
            }
        }

        [HttpPost("{prototypeId:int}/derivatives")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> CreateDerivativeAsync(int prototypeId, [FromBody] CreateDerivativeDto dto)
        {
            try
            {
                var derivative = await _createDerivativeUseCase.Execute(prototypeId, dto.ToLibraryId);
                return CreatedAtRoute(nameof(GetDerivativesAsync),
                    new {prototypeId, dto.ToLibraryId},
                    derivative);
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

        [HttpPatch("{prototypeId:int}/derivatives")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> SaveDerivativeAsync(int prototypeId, [FromBody] SaveDerivativeDto dto)
        {
            try
            {
                var derivative = await _saveDerivativeUseCase.Execute(prototypeId, dto.LibraryId);
                return AcceptedAtRoute(nameof(GetDerivativesAsync),
                    new {prototypeId, dto.LibraryId},
                    derivative);
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

        [HttpPut("{prototypeId:int}/derivatives/sync")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> SyncDerivativesAsync(int prototypeId, [FromQuery] string libraryId)
        {
            try
            {
                await _syncDerivativePartNumberUseCase.Execute(prototypeId, libraryId);
                return AcceptedAtRoute(nameof(GetDerivativesAsync),
                    new {prototypeId, libraryId},
                    GetDerivativesAsync(prototypeId, libraryId));
            }
            catch (ArgumentNullException argumentNullException)
            {
                return UnprocessableEntity(argumentNullException.Message);
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

        [HttpPut("{prototypeId:int}/group")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> AssignGroupAsync(int prototypeId, [FromBody] AssignGroupDto dto)
        {
            try
            {
                await _assignGroupUseCase.Execute(prototypeId, dto.GroupId);
                return AcceptedAtRoute(nameof(GetPrototypeByIdAsync), new {prototypeId});
            }
            catch (ResourceNotFoundException resourceNotFoundException)
            {
                return UnprocessableEntity(resourceNotFoundException.Message);
            }
        }

        [HttpDelete("{prototypeId:int}/derivatives")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> DeleteDerivativeAsync(int prototypeId, [FromQuery] string libraryId)
        {
            try
            {
                await _deleteDerivativeUseCase.Execute(prototypeId, libraryId);
                return NoContent();
            }
            catch (ResourceNotFoundException resourceNotFoundException)
            {
                return UnprocessableEntity(resourceNotFoundException.Message);
            }
        }
    }
}