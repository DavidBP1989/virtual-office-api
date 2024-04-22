using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cobach_api.Features.Documentos
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DocumentosController : ControllerBase
    {
        private readonly IMediator _mediator;
        public DocumentosController(IMediator mediator)
        {
            _mediator = mediator;
        }
        

        [HttpGet("informacion-carpetas")]
        public async Task<IActionResult> InformacionCarpetas()
        {
            var req = new InformacionCarpetas.Request();
            return Ok(await _mediator.Send(req));
        }

        [HttpGet("archivos-por-folder")]
        public async Task<IActionResult> ArchivosPorFolder(int folderId)
        {
            var req = new ArchivosPorFolder.Request(folderId);
            return Ok(await _mediator.Send(req));
        }
    }
}
