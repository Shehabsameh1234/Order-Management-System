using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order_Management_System.Errors;

namespace Order_Management_System.Controllers
{
    [Route("errors/{code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {
        public ActionResult Error(int code)
        {
            if (code == 404)
                return NotFound(new ApisResponse(code));
            else if (code == 401)
                return Unauthorized(new ApisResponse(code));
            else if (code == 400)
                return BadRequest(new ApisResponse(code));

            else return StatusCode(code);
        }
    }
}
