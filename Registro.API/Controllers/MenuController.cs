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
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<ActionResult<HttpResponseWrapper<List<MenuDTO>>>> Lista(int idUsuario)
        {
            var response = new HttpResponseWrapper<List<MenuDTO>>();

            try
            {
                var listaMenus = await _menuService.Lista(idUsuario);

                response.status = true;
                response.value = listaMenus;

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
