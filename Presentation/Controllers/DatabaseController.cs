using Application.Commands.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Base;

namespace Presentation.Controllers
{
    public class DatabaseController : BaseApiController
    {
        [HttpPost]
        [AllowAnonymous]
        [Route("Database/create")]
        public async Task<IActionResult> create()
        {
            var res = await Mediator.Send(new CreateDatabaseCommand());
            if (res.Success)
                return Ok(res.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
