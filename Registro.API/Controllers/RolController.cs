using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Registro.BLL.Services.ServicesContracts;
using Registro.DTO;
using Registro.API.Utilities;


namespace Registro.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {
        private readonly IRolService _rolService;

        public RolController(IRolService rolService)
        {
            _rolService = rolService;
        }

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
