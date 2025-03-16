using Microsoft.AspNetCore.Mvc;
using Registro.BLL.Services;
using Registro.BLL.Services.ServicesContracts;
using Registro.DTO;
using System;

namespace Registro.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly AuthService _authService;

        public LoginController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> IniciarSesion([FromBody] LoginDTO loginDTO)
        {
            try
            {
                var token = await _authService.Login(loginDTO);
                return Ok(token);
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