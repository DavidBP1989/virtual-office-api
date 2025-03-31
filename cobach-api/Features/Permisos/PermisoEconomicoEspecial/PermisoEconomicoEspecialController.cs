using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace cobach_api.Features.Permisos.PermisoEconomicoEspecial
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermisoEconomicoEspecialController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PermisoEconomicoEspecialController(IMediator mediator) => _mediator = mediator;

        [HttpPost("crear-permiso-economico-especial")]
        public async Task<IActionResult> CrearPermisoEconomico([FromBody] PermisoEconomicoEspecialAgregar.Request request) =>
            Ok(await _mediator.Send(request));
    }
}
