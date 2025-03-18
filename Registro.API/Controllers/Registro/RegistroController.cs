using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Registro.BLL.Services;
using Registro.BLL.Services.ServicesContracts;
using Registro.DTO;

namespace Registro.API.Controllers.Registro
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistroController_ : ControllerBase
    {
        private readonly AuthService _authService;

        public RegistroController_(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<ActionResult> RegistrarUsuario([FromBody] UsuarioDTO model)
        {
            try
            {
                model.IdRol = 4;

                var usuarioCreado = await _authService.Registro(model);
                return Ok(usuarioCreado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Mensaje = ex.Message });
            }
            
        }
    }


}
