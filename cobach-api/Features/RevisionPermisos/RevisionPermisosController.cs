using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cobach_api.Features.RevisionPermisos
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RevisionPermisosController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RevisionPermisosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("permisos-revisar")]
        public async Task<IActionResult> PermisosLaboralesPorRevisar()
        {
            var req = new PermisosLaboralesPorRevisar.Request();
            return  Ok(await _mediator.Send(req)); 
        }

        [HttpPost("actualizar-estatus-permiso")]
        public async Task<IActionResult> ActualizarPermiso(ActualizarPermiso.Request request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost("confirmar-permiso")]
        public async Task<IActionResult> ConfirmarPermiso(ConfirmarPermiso.Request request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}
