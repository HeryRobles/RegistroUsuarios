using Microsoft.AspNetCore.Mvc;
using Registro.BLL.Services.ServicesContracts;
using Registro.DTO;
using Microsoft.AspNetCore.Authorization;
using Registro.BLL.Services;
using System.Security.Claims;

namespace Registro.API.Controllers.Usuarios
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        //[Authorize(Roles = "Administrador")]
        [HttpGet("lista")]
        public async Task<ActionResult<List<UsuarioDTO>>> Lista()
        {
            var response = new List<UsuarioDTO>();

            var usuarios = await _usuarioService.Lista();
            return Ok(response);
       
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDTO>> ObtenerUsuario(int id)
        {
            try
            {
                var usuario = await _usuarioService.Obtener(id);
                if(usuario == null)
                    return NotFound(new { message = "Usuario no encontrado." });
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //[Authorize(Roles = "Administrador")]
        [HttpPost("crear")]
        public async Task<ActionResult<UsuarioDTO>> Crear([FromBody] UsuarioDTO usuario)
        {

            try
            {
                var usuarioCreado = await _usuarioService.Crear(usuario);
                return CreatedAtAction(nameof(ObtenerUsuario), new { id = usuarioCreado.IdUsuario }, usuarioCreado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //[Authorize]
        [HttpPut("editar/{id}")]
        public async Task<ActionResult<bool>> Editar(int id, [FromBody] UsuarioDTO usuario)
        {

            if (id != usuario.IdUsuario)
                return BadRequest(new { message = "El ID del usuario no coincide." });

            try
            {
                var idUsuarioActual = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var rolUsuarioActual = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;

                var resultado = await _usuarioService.Editar(usuario, idUsuarioActual, rolUsuarioActual);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
           
        }
        
        //[Authorize(Roles = "Administrador")]
        [HttpDelete("eliminar/{id}")]
        public async Task<ActionResult<bool>> Eliminar(int id)
        {

            try
            {
                var resultado = await _usuarioService.Eliminar(id);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
