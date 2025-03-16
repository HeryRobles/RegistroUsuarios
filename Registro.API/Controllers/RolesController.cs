using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Registro.BLL.Services.ServicesContracts;
using Registro.DTO;
using Registro.API.Utilities;
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
        public async Task<ActionResult<HttpResponseWrapper<List<RolDTO>>>> Lista()
        {
            var response = new HttpResponseWrapper<List<RolDTO>>();

            try
            {
                var listaRoles = await _rolService.Lista();
                response.status = true;
                response.value = listaRoles;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.HttpResponseMessage = ex.Message;
                return BadRequest(response);
            }
        }
    }
}
