//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Registro.BLL.Services.ServicesContracts;
//using Registro.DTO;

//namespace Registro.API.Controllers.Registro
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class LoginController : ControllerBase
//    {
//        private readonly IRegistroService _registroService;
//        private readonly IJwtService _tokenService;

//        public LoginController(IUsuarioService usuarioService, IRegistroService registroService, IJwtService tokenService)
//        {
//            _registroService = registroService;
//            _tokenService = tokenService;
//        }

//        [HttpPost]
//        public async Task<ActionResult<RegistroUsuarioDTO>> Login([FromBody] UsuarioDTO usuario)
//        {
//            try
//            {
//                var usuarioRegistrado = await _registroService.Login(usuario);
//                if (usuarioRegistrado == null)
//                    return Unauthorized("Usuario o contraseña incorrectos");
//                var token = _tokenService.GenerarToken(usuarioRegistrado);
//                return Ok(new { token });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
//            }
//        }
//    }
//}
