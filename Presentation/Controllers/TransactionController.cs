using Application.Queries.Person;
using Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Base;

namespace Presentation.Controllers
{
    public class TransactionController : BaseApiController
    {
        [HttpPost]
        [AllowAnonymous]
        [Route("Transaction/Save")]
        public async Task<IActionResult> Save([FromBody] IEnumerable<VmTransaction> transactions)
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
                foreach (var item in transactions)
                {
                    var res = await Mediator.Send(item);
                    if (!res.Success)
                        return StatusCode(StatusCodes.Status500InternalServerError, "ثبت برخی داده ها با مشکل موجه شده است");
                }
                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("Transaction/TransactionSumationReport")]
        public async Task<IActionResult> TransactionSumationReport()
        {
            var res = await Mediator.Send(new GetSumTransactionQuery());
            return Ok(res);
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("Transaction/TransactionSumationReportWithTotal")]
        public async Task<IActionResult> TotalTransactionReportWithTotal()
        {
            var res = await Mediator.Send(new GetSumTransactionWithTotalQuery());
            return Ok(res);
        }
    }
}
