//using Azure;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Registro.API.Utilities;
//using Registro.BLL.Services;
//using Registro.BLL.Services.ServicesContracts;
//using Registro.DTO;

//namespace Registro.API.Controllers.Registro
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class RegistroController_ : ControllerBase
//    {
//        private readonly IRegistroService _registroService;
//        private readonly IAuthService _authService;

//        public RegistroController_(IRegistroService registroService, IAuthService authService)
//        {
//            _registroService = registroService;
//            _authService = authService;
//            _response = new HttpResponseWrapper<object>();

//        }

//        [HttpPost("registro")]
//        public async Task<ActionResult> Registro([FromBody] RegistroUsuarioDTO registroDTO)
//        {
//            try
//            {
//                var usuario = await _authService.Registro(registroDTO);
//                _response.status = true;
//                _response.value = usuario;
//                return Ok(_response);
//            }
//            catch (Exception ex)
//            {
//                _response.status = false;
//                _response.HttpResponseMessage = ex.Message;
//                return BadRequest(_response);
//            }
//        }
//    }


//}
