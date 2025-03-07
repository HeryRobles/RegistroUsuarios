using Microsoft.AspNetCore.Mvc;
using Registro.API.Utilities;
using Registro.BLL.Services.ServicesContracts;
using Registro.DTO;

namespace Registro.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

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
                response.status = true;
                response.value = await _usuarioService.ValidarCredenciales(login.Correo, login.Clave);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.HttpResponseMessage = ex.Message;
                return Unauthorized(response);
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
