using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Registro.BLL.Services.ServicesContracts;
using Registro.DTO;
using Registro.API.Utilities;
using Registro.BLL.Services;

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

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            var response = new HttpResponseWrapper<List<UsuarioDTO>>();

            try
            {
                response.status = true;
                response.value = await _usuarioService.Lista();
            }
            catch (Exception ex)
            {
                response.status = false;
                response.HttpResponseMessage = ex.Message;
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("IniciarSesion")]
        public async Task<IActionResult> IniciarSesion([FromBody] LoginDTO login)
        {
            var response = new HttpResponseWrapper<SesionDTO>();

            try
            {
                response.status = true;
                response.value = await _usuarioService.ValidarCredenciales(login.Correo, login.Clave);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.HttpResponseMessage = ex.Message;
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("Guardar")]
        public async Task<IActionResult> Guardar([FromBody] UsuarioDTO usuario)
        {
            var response = new HttpResponseWrapper<UsuarioDTO>();

            try
            {
                response.status = true;
                response.value = await _usuarioService.Crear(usuario);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.HttpResponseMessage = ex.Message;
            }
            return Ok(response);
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] UsuarioDTO usuario)
        {
            var response = new HttpResponseWrapper<bool>();

            try
            {
                response.status = true;
                response.value = await _usuarioService.Editar(usuario);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.HttpResponseMessage = ex.Message;
            }
            return Ok(response);
        }

        [HttpDelete]
        [Route("Eliminar")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var response = new HttpResponseWrapper<bool>();

            try
            {
                response.status = true;
                response.value = await _usuarioService.Eliminar(id);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.HttpResponseMessage = ex.Message;
            }
            return Ok(response);
        }
    }
}
