using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Base;

namespace Presentation.Controllers
{
    public class PersonController : BaseApiController
    {
        [HttpPost]
        [AllowAnonymous]
        [Route("Person/Save")]
        public async Task<IActionResult> Save([FromBody] IEnumerable<Persons> persons)
        {
            if (!ModelState.IsValid)
            {
                string message = "";
                foreach (var item in ModelState.Values.SelectMany(v => v.Errors))
                {
                    message = (string.IsNullOrEmpty(message) ? item.ErrorMessage : message + ", " + item.ErrorMessage);
                }
                return BadRequest(message);
            }
            try
            {
                foreach (var item in persons)
                { 
                    var res = await Mediator.Send(item);
                    if (!res.Success)
                        return StatusCode(StatusCodes.Status500InternalServerError,"ثبت برخی داده ها با مشکل موجه شده است");

                }
                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
