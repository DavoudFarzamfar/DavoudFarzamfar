using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers.Base
{

    public class BaseApiController : ControllerBase
    {
        #region Property
        protected IMediator _mediator;
        #endregion
        protected IMediator Mediator
        {
            get
            {
                return _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
            }
        }
    }

}
