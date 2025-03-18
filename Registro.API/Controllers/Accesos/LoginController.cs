using Microsoft.AspNetCore.Mvc;
using Registro.BLL.Services;
using Registro.BLL.Services.ServicesContracts;
using Registro.DTO;
using System;

namespace Registro.API.Controllers.Accesos
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAuthService _authService;

        public LoginController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("registrarse")]
        public async Task<ActionResult> RegistrarUsuario([FromBody] UsuarioDTO model)
        {
            try
            {
                var usuarioCreado = await _authService.Registro(model);
                return Ok(usuarioCreado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Mensaje = ex.Message });
            }

        }

        [HttpPost("iniciosesion")]
        public async Task<ActionResult> IniciarSesion([FromBody] LoginDTO loginDTO)
        {
            try
            {
                var token = await _authService.Login(loginDTO);
                return Ok(new { Token = token });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Mensaje = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Mensaje = "Error interno del servidor" });
            }
        }
    }
}