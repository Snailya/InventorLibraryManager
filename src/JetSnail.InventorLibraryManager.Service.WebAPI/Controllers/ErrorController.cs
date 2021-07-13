using Microsoft.AspNetCore.Mvc;

namespace JetSnail.InventorLibraryManager.Service.WebAPI.Controllers
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