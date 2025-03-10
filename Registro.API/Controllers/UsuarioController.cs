using Microsoft.AspNetCore.Mvc;
using Registro.API.Utilities;
using Registro.BLL.Services.ServicesContracts;
using Registro.DTO;
using Microsoft.AspNetCore.Authorization;

namespace Registro.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IJwtService _jwtService;

        public UsuarioController(IUsuarioService usuarioService, IJwtService jwtService)
        {
            _usuarioService = usuarioService;
            _jwtService = jwtService;
        }

        [Authorize(Roles = "Administrador, Supervisor")]
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

        [HttpPost("login")]
        public async Task<ActionResult<SesionDTO>> IniciarSesion([FromBody] LoginDTO login)
        {
            var response = new HttpResponseWrapper<SesionDTO>();

            try
            {
                var sesion = await _usuarioService.Login(login.Correo, login.Clave);

                if (sesion == null)
                {
                    response.status = false;
                    response.HttpResponseMessage = "Credenciales inválidas";
                    return Unauthorized(response);
                }

                var token = _jwtService.GenerarToken(sesion);

                response.status = true;
                response.value = new SesionDTO
                {
                    IdUsuario = sesion.IdUsuario,
                    NombreCompleto = sesion.NombreCompleto,
                    Correo = sesion.Correo,
                    RolDescripcion = sesion.RolDescripcion,
                    Token = token
                };

                return Ok(response);
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

        [HttpPost("registroCliente")]
        public async Task<ActionResult<UsuarioDTO>> Registrar([FromBody] RegistroUsuariosDTO registro)
        {
            var response = new HttpResponseWrapper<UsuarioDTO>();

            try
            {
                var usuario = new UsuarioDTO
                {
                    NombreCompleto = registro.NombreCompleto,
                    Correo = registro.Correo,
                    Clave = registro.Clave,
                    IdRol = 4 // Rol de cliente por defecto
                };

                response.status = true;
                response.value = await _usuarioService.Registrar(usuario);
                return CreatedAtAction(nameof(Registrar), new { id = response.value.IdUsuario }, response);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.HttpResponseMessage = ex.Message;
                return BadRequest(response);
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost("activar")]
        public async Task<ActionResult<UsuarioDTO>> ActivarUsuario([FromBody] UsuarioDTO usuario)
        {
            var response = new HttpResponseWrapper<UsuarioDTO>();

            try
            {
                response.status = true;
                response.value = await _usuarioService.DarDeAlta(usuario);
                return CreatedAtAction(nameof(ActivarUsuario), new { id = response.value.IdUsuario }, response);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.HttpResponseMessage = ex.Message;
                return BadRequest(response);
            }
        }

        [Authorize]
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

        [Authorize(Roles = "Administrador")]
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

        [Authorize(Roles = "Administrador")]
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
