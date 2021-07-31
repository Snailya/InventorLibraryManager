using Microsoft.AspNetCore.Mvc;

namespace JetSnail.InventorLibraryManager.Server.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        [NonAction]
        public IActionResult Error()
        {
            return Problem();
        }
    }
}