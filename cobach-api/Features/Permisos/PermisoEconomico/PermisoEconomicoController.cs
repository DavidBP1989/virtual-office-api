using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace cobach_api.Features.Permisos.PermisoEconomico
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermisoEconomicoController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PermisoEconomicoController(IMediator mediator) => _mediator = mediator;

        [HttpGet("permiso-economico-list")]
        public async Task<IActionResult> PermisoEconomico()
        {
            var req = new PermisoEconomicoList.Request();
            return Ok(await _mediator.Send(req));
        }

        [HttpPost("crear-permiso-economico")]
        public async Task<IActionResult> CrearPermisoEconomico([FromBody] PermisoEconomicoAgregar.Request request) =>
            Ok(await _mediator.Send(request));

        [HttpPost("actualizar-permiso-economico")]
        public async Task<IActionResult> ActualizarPermisoEconomico([FromBody] PermisoEconomicoActualizar.Request request) =>
            Ok(await _mediator.Send(request));
    }
}
