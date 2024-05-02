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

        [HttpGet("catalogo-permisos-laborales")]
        public async Task<IActionResult> CatalogoPermisosLaborales()
        {
            var req = new CatalogoPermisosLaborales.Request();
            return Ok(await _mediator.Send(req));
        }

        [HttpGet("corte-tiempo")]
        public async Task<IActionResult> CortedeTiempo()
        {
            var req = new CorteTiempo.Request();
            return Ok(await _mediator.Send(req));
        }

        [HttpPost("crear-corte-tiempo")]
        public async Task<IActionResult> CrearCortedeTiempo([FromBody] CorteTiempoAgregar.Request request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}
