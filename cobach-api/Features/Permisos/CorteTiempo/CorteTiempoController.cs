using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cobach_api.Features.Permisos.CorteTiempo
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CorteTiempoController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CorteTiempoController(IMediator mediator) => _mediator = mediator;

        [HttpGet("corte-tiempo-list")]
        public async Task<IActionResult> CorteTiempoList()
        {
            var req = new CorteTiempoList.Request();
            return Ok(await _mediator.Send(req));
        }

        [HttpPost("crear-corte-tiempo")]
        public async Task<IActionResult> CrearCortedeTiempo([FromBody] CorteTiempoAgregar.Request request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost("actualizar-corte-tiempo")]
        public async Task<IActionResult> ActualizarCortedeTiempo([FromBody] CorteTiempoActualizar.Request request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}
