using Microsoft.AspNetCore.Mvc;
using Registro.API.Utilities;
using Registro.BLL.Services.ServicesContracts;
using Registro.DTO;
using Microsoft.AspNetCore.Authorization;
using Registro.BLL.Services;

namespace Registro.API.Controllers.Usuarios
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly JwtService _jwtService;

        public UsuariosController(IUsuarioService usuarioService, JwtService jwtService)
        {
            _usuarioService = usuarioService;
            _jwtService = jwtService;
        }

        //[Authorize(Roles = "Administrador")]
        [HttpGet("lista")]
        public async Task<ActionResult<List<UsuarioDTO>>> Lista()
        {
            var response = new HttpResponseWrapper<List<UsuarioDTO>>();

            try
            {
                response.status = true;
                response.value = await _usuarioService.Lista();
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.HttpResponseMessage = ex.Message;
                return BadRequest(response);
            }
        }

        //[Authorize(Roles = "Administrador")]
        [HttpPost("crear")]
        public async Task<ActionResult<UsuarioDTO>> Crear([FromBody] UsuarioDTO usuario)
        {
            var response = new HttpResponseWrapper<UsuarioDTO>();

            try
            {
                response.status = true;
                response.value = await _usuarioService.Crear(usuario);
                return CreatedAtAction(nameof(Crear), new { id = response.value.IdUsuario }, response);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.HttpResponseMessage = ex.Message;
                return BadRequest(response);
            }
        }

        /*Este endpoint es el que se encarga de registrar un usuario
         * este usuario se registra con el rol de cliente por defecto, el cual tendra acceso a el catalogo de peliculas
         * y podrá comentar y calificarlas cuando inicie sesión.
         * */

        //[Authorize]
        [HttpPut("editar/{id}")]
        public async Task<ActionResult<bool>> Editar(int id, [FromBody] UsuarioDTO usuario)
        {
            var response = new HttpResponseWrapper<bool>();

            try
            {
                var idUsuarioActual = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "idUsuario")?.Value);
                var rolUsuarioActual = User.Claims.FirstOrDefault(c => c.Type == "rol")?.Value;

                response.status = true;
                response.value = await _usuarioService.Editar(usuario, idUsuarioActual, rolUsuarioActual);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                response.status = false;
                response.HttpResponseMessage = ex.Message;
                return Unauthorized(response);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.HttpResponseMessage = ex.Message;
                return BadRequest(response);
            }
        }

        //[Authorize(Roles = "Administrador")]
        [HttpPut("{id}/rol")]
        public async Task<ActionResult<bool>> AsignarRol(int id, [FromQuery] int nuevoRolId)
        {
            var response = new HttpResponseWrapper<bool>();

            try
            {
                response.status = true;
                response.value = await _usuarioService.AsignarRol(id, nuevoRolId);

                if (!response.value)
                {
                    response.status = false;
                    response.HttpResponseMessage = "No se pudo asignar el rol al usuario.";
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.HttpResponseMessage = ex.Message;
                return BadRequest(response);
            }
        }

        //[Authorize(Roles = "Administrador")]
        [HttpDelete("editar/{id}")]
        public async Task<ActionResult<bool>> Eliminar(int id)
        {
            var response = new HttpResponseWrapper<bool>();

            try
            {
                response.status = true;
                response.value = await _usuarioService.Eliminar(id);

                if (!response.value)
                    return NotFound(new { status = false, message = "Usuario no encontrado." });

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
