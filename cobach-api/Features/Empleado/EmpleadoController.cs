using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cobach_api.Features.Empleado
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmpleadoController : ControllerBase
    {
        private readonly IMediator _mediator;
        public EmpleadoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("informacion-basica")]
        public async Task<IActionResult> InformacionBasica()
        {
            var req = new InformacionBasica.Request();
            return Ok(await _mediator.Send(req));
        }

        [HttpGet("informacion-general")]
        public async Task<IActionResult> InformacionGeneral()
        {
            var req = new InformacionGeneral.Request();
            return Ok(await _mediator.Send(req));
        }

        [HttpGet("fondo-ahorro")]
        public async Task<IActionResult> FondoAhorro()
        {
            var req = new FondoAhorro.Request();
            return Ok(await _mediator.Send(req));
        }

        [HttpGet("centros-trabajo")]
        public async Task<IActionResult> CentrosTrabajo()
        {
            var req = new CentrosTrabajo.Request();
            return Ok(await _mediator.Send(req));
        }
    }
}
