using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.UseCase.LibraryScope;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JetSnail.InventorLibraryManager.Server.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class LibrariesController : ControllerBase
    {
        private readonly IGetLibrariesUseCase _getLibrariesUseCase;

        public LibrariesController(IGetLibrariesUseCase getLibrariesUseCase)
        {
            _getLibrariesUseCase = getLibrariesUseCase;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllGroupsAsync()
        {
            return Ok(await _getLibrariesUseCase.Execute());
        }
    }
}