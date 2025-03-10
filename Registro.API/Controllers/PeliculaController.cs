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

        [HttpGet("lista")]
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
                return NotFound(new { message = "Película no encontrada." });

            return Ok(pelicula);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost("crear")]
        public async Task<ActionResult<PeliculaDTO>> Crear([FromBody] PeliculaDTO peliculaDTO)
        {
            try
            {
                var peliculaCreada = await _peliculaService.Crear(peliculaDTO);
                return CreatedAtAction(nameof(Obtener), new { id = peliculaCreada.IdPelicula }, peliculaCreada);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Supervisor, Empleado, Administrador")]
        [HttpPut("editar/{id}")]
        public async Task<ActionResult<bool>> Editar(int id, [FromBody] PeliculaDTO peliculaDTO)
        {
            if (id != peliculaDTO.IdPelicula)
                return BadRequest(new { message = "El ID de la película no coincide." });

            try
            {
                var resultado = await _peliculaService.Editar(peliculaDTO);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpDelete("eliminar/{id}")]
        public async Task<ActionResult<bool>> Eliminar(int id)
        {
            try
            {
                var resultado = await _peliculaService.Eliminar(id);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Cliente")]
        [HttpPost("{id}/calificar")]
        public async Task<ActionResult<PeliculaDTO>> Calificar(int id, [FromBody] double calificacion)
        {
            try
            {
                var peliculaCalificada = await _peliculaService.Calificar(id, calificacion);
                return Ok(peliculaCalificada);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Cliente")]
        [HttpPost("{id}/comentar")]
        public async Task<ActionResult<PeliculaDTO>> Comentar(int id, [FromBody] ComentarioDTO comentarioDTO)
        {
            try
            {
                var peliculaComentada = await _peliculaService.Comentar(id, comentarioDTO);
                return Ok(peliculaComentada);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}