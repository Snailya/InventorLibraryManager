using System;
using System.Linq;
using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.Core.DTOs;
using JetSnail.InventorLibraryManager.UseCase.DataStores;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JetSnail.InventorLibraryManager.Service.WebAPI.Controllers
{
    /// <inheritdoc />
    [ApiController]
    [Route("api/[controller]")]
    public class LibrariesController : ControllerBase
    {
        private readonly ILibraryRepository _libraryRepository;
        private readonly ILogger<LibrariesController> _logger;


        /// <inheritdoc />
        public LibrariesController(ILogger<LibrariesController> logger, ILibraryRepository libraryRepository)
        {
            _logger = logger;
            _libraryRepository = libraryRepository;
        }

        /// <summary>
        ///     获取当前Inventor中加载的库信息。
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAsync()
        {
            try
            {
                var libraries = await _libraryRepository.GetAllAsync();
                return Ok(libraries.Select(x => new LibraryDto
                {
                    DisplayName = x.InventorModel.DisplayName, LibraryInternalName = x.InventorModel.InternalName,
                    ReadOnly = x.InventorModel.ReadOnly
                }));
            }
            catch (Exception e)
            {
                return e.HResult switch
                {
                    -2147221021 => StatusCode(500, "Unable to get Inventor from ROT."), // Inventor没有启动
                    _ => UnprocessableEntity(e.Message)
                };
            }
        }
    }
}