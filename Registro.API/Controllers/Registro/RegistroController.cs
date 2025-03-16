using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Registro.API.Utilities;
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

        [HttpPost("registro")]
        public async Task<IActionResult> RegistrarUsuario([FromBody] RegistroUsuarioDTO registroDTO)
        {
            try
            {
                var usuarioCreado = await _authService.Registro(registroDTO);
                return Ok(usuarioCreado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Mensaje = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Mensaje = "Error interno del servidor" });
            }
        }
    }


}
