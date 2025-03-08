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
        [HttpGet("Lista")]
        public async Task<ActionResult<HttpResponseWrapper<List<UsuarioDTO>>>> Lista()
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

        [HttpPost("IniciarSesion")]
        public async Task<ActionResult<HttpResponseWrapper<SesionDTO>>> IniciarSesion([FromBody] LoginDTO login)
        {
            var response = new HttpResponseWrapper<SesionDTO>();

            try
            {
                var usuario = await _usuarioService.ValidarCredenciales(login.Correo, login.Clave);

                if (usuario == null)
                {
                    response.status = false;
                    response.HttpResponseMessage = "Credenciales inválidas";
                    return Unauthorized(response);
                }

                var token = _jwtService.GenerarToken(usuario);

                response.status = true;
                response.value = new SesionDTO
                {
                    IdUsuario = usuario.IdUsuario,
                    NombreCompleto = usuario.NombreCompleto,
                    Correo = usuario.Correo,
                    RolDescripcion = usuario.RolDescripcion,
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
        
        [HttpPost("Registrar")]
        public async Task<ActionResult<HttpResponseWrapper<UsuarioDTO>>> Registrar([FromBody] RegistroUsuariosDTO registro)
        {
            var response = new HttpResponseWrapper<UsuarioDTO>();

            try
            {
                // Asignar el rol de "Cliente" por defecto
                var usuario = new UsuarioDTO
                {
                    NombreCompleto = registro.NombreCompleto,
                    Correo = registro.Correo,
                    Clave = registro.Clave,
                    IdRol = 4 
                };

                response.status = true;
                response.value = await _usuarioService.Crear(usuario);
                return CreatedAtAction(nameof(Registrar), new { id = response.value.IdUsuario }, response);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.HttpResponseMessage = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpPost("Guardar")]
        public async Task<ActionResult<HttpResponseWrapper<UsuarioDTO>>> Guardar([FromBody] UsuarioDTO usuario)
        {
            var response = new HttpResponseWrapper<UsuarioDTO>();

            try
            {
                response.status = true;
                response.value = await _usuarioService.Crear(usuario);
                return CreatedAtAction(nameof(Guardar), new { id = response.value.IdUsuario }, response);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.HttpResponseMessage = ex.Message;
                return BadRequest(response);
            }
        }
        [Authorize(Roles = "Administrador")]
        [HttpPut("Editar")]
        public async Task<ActionResult<HttpResponseWrapper<bool>>> Editar([FromBody] UsuarioDTO usuario)
        {
            var response = new HttpResponseWrapper<bool>();

            try
            {
                response.status = true;
                response.value = await _usuarioService.Editar(usuario);
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
        [HttpPost("AsignarRol")]
        public async Task<ActionResult<HttpResponseWrapper<bool>>> AsignarRol([FromQuery] int usuarioId, [FromQuery] int nuevoRolId)
        {
            var response = new HttpResponseWrapper<bool>();

            try
            {
                response.status = true;
                response.value = await _usuarioService.AsignarRol(usuarioId, nuevoRolId);

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
        [HttpDelete("Eliminar/{id}")]
        public async Task<ActionResult<HttpResponseWrapper<bool>>> Eliminar(int id)
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
