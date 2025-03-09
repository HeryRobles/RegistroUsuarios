using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Registro.BLL.Services.ServicesContracts;
using Registro.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Registro.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeliculaController : ControllerBase
    {
        private readonly IPeliculaService _peliculaService;

        public PeliculaController(IPeliculaService peliculaService)
        {
            _peliculaService = peliculaService;
        }

        [HttpGet("Lista")]
        public async Task<ActionResult<List<PeliculaDTO>>> Lista()
        {
            var peliculas = await _peliculaService.Lista();
            return Ok(peliculas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PeliculaDTO>> Obtener(int id)
        {
            var pelicula = await _peliculaService.Obtener(id);
            if (pelicula == null)
                return NotFound();

            return Ok(pelicula);
        }

        // Crear una nueva película (solo Administrador)
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<ActionResult<PeliculaDTO>> Crear([FromBody] PeliculaDTO peliculaDTO)
        {
            var peliculaCreada = await _peliculaService.Crear(peliculaDTO);
            return CreatedAtAction(nameof(Obtener), new { id = peliculaCreada.IdPelicula }, peliculaCreada);
        }

        [Authorize(Roles = "Supervisor, Empleado, Administrador")]
        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> Editar(int id, [FromBody] PeliculaDTO peliculaDTO)
        {
            if (id != peliculaDTO.IdPelicula)
                return BadRequest();

            var resultado = await _peliculaService.Editar(peliculaDTO);
            return Ok(resultado);
        }

        [Authorize(Roles = "Administrador")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Eliminar(int id)
        {
            var resultado = await _peliculaService.Eliminar(id);
            return Ok(resultado);
        }

        [Authorize(Roles = "Cliente")]
        [HttpPost("{id}/calificar")]
        public async Task<ActionResult<PeliculaDTO>> Calificar(int id, [FromQuery] double calificacion)
        {
            var peliculaCalificada = await _peliculaService.Calificar(id, calificacion);
            return Ok(peliculaCalificada);
        }

        [Authorize(Roles = "Cliente")]
        [HttpPost("{id}/comentar")]
        public async Task<ActionResult<PeliculaDTO>> Comentar(int id, [FromBody] ComentarioDTO comentarioDTO)
        {
            var peliculaComentada = await _peliculaService.Comentar(id, comentarioDTO);
            return Ok(peliculaComentada);
        }
    }
}