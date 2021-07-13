using Microsoft.AspNetCore.Mvc;

namespace JetSnail.InventorLibraryManager.Service.WebAPI.Controllers
{
    [ApiController]
    public class IndexController : ControllerBase
    {
        [Route("/")]
        [NonAction]
        public IActionResult Welcome()
        {
            return Ok("Welcome!");
        }
    }
}