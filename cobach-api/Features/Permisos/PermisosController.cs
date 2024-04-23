using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cobach_api.Features.Permisos
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PermisosController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PermisosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("corte-tiempo")]
        public async Task<IActionResult> CortedeTiempo()
        {
            var req = new CorteTiempo.Request();
            return Ok(await _mediator.Send(req));
        }
    }
}
