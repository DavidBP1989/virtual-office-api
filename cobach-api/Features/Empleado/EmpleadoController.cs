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

        [HttpGet("fondo-ahorro-historial")]
        public async Task<IActionResult> FondoAhorroHistorial(int idRegistro)
        {
            var req = new FondoAhorroHistorial.Request(idRegistro);
            return Ok(await _mediator.Send(req));
        }

        [HttpGet("centros-trabajo")]
        public async Task<IActionResult> CentrosTrabajo()
        {
            var req = new CentrosTrabajo.Request();
            return Ok(await _mediator.Send(req));
        }

        [HttpPost("cambiar-contrasena")]
        public async Task<IActionResult> CambiarContrasena([FromBody] CambiarContrasena.Request request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpGet("busqueda-empleado")]
        public async Task<IActionResult> BusquedaEmpleado(string filtro)
        {
            return Ok(await _mediator.Send(new BusquedaEmpleado.Request(filtro)));
        }

        [HttpGet("empleados")]
        public async Task<IActionResult> Empleados()
        {
            return Ok(await _mediator.Send(new Empleados.Request()));
        }
    }
}
