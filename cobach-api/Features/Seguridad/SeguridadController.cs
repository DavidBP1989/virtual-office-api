using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace cobach_api.Features.Seguridad
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeguridadController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SeguridadController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] CreateToken.Request request) => Ok(await _mediator.Send(request));
    }
}
