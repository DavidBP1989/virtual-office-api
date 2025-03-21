using cobach_api.Features.Common.Enums;
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

        [HttpGet("descargar-corte-tiempo")]
        [AllowAnonymous]
        public async Task<IActionResult> DescargarCorteTiempo(int permissionId)
        {
            var req = new Descargar.Request(permissionId, TipoPermisosLaborales.CorteTiempo);
            var file = await _mediator.Send(req);

            return File(file.Data.File, "application/pdf");
        }

        [HttpGet("descargar-permiso-economico")]
        [AllowAnonymous]
        public async Task<IActionResult> DescargarPermisoEconomico(int permissionId)
        {
            var req = new Descargar.Request(permissionId, TipoPermisosLaborales.PermisoEconomico);
            var file = await _mediator.Send(req);

            return File(file.Data.File, "application/pdf");
        }

        [HttpGet("permisos-por-usuario")]
        public async Task<IActionResult> PermisoPorCliente(string empleadoId)
        {
            var req = new PermisoPorEmpleado.Request(empleadoId);
            return Ok(await _mediator.Send(req));
        }
    }
}
