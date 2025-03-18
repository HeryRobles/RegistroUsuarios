using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Registro.BLL.Services.ServicesContracts;
using Registro.DTO;
using Microsoft.AspNetCore.Authorization;


namespace Registro.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRolService _rolService;

        public RolesController(IRolService rolService)
        {
            _rolService = rolService;
        }

        [Authorize(Roles = "Administrador, Supervisor")]
        [HttpGet]
        [Route("Lista")]
        public async Task<ActionResult<List<RolDTO>>> Lista()
        {
            return Ok(await _rolService.Lista());
        }
    }
}
