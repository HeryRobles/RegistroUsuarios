using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Registro.BLL.Services.ServicesContracts;
using Registro.DAL.Repository.IRepository;
using Registro.DTO;
using Registro.Model.Entities;

namespace Registro.BLL.Services
{
    public class PeliculaService : IPeliculaService
    {
        private readonly IGenericRepository<Pelicula> _peliculaRepository;
        private readonly IGenericRepository<Comentario> _comentarioRepository;
        private readonly IMapper _mapper;

        public PeliculaService(IGenericRepository<Pelicula> peliculaRepository, IGenericRepository<Comentario> comentarioRepository, IMapper mapper)
        {
            _peliculaRepository = peliculaRepository;
            _comentarioRepository = comentarioRepository;
            _mapper = mapper;
        }

        public async Task<List<PeliculaDTO>> Lista()
        {
            try
            {
                var queryPelicula = await _peliculaRepository.Consultar();
                var listaPelicula = queryPelicula.ToList();
                return _mapper.Map<List<PeliculaDTO>>(listaPelicula);
            }
            catch
            {
                throw;
            }
        }

        public Task<PeliculaDTO> Obtener(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PeliculaDTO> Crear(PeliculaDTO modelo)
        {
            try
            {
                var peliculaCreada = await _peliculaRepository.Crear(_mapper.Map<Pelicula>(modelo));

                if(peliculaCreada.IdPelicula == 0)
                    throw new TaskCanceledException("No se pudo crear la pelicula");

                return _mapper.Map<PeliculaDTO>(peliculaCreada);

            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(PeliculaDTO modelo)
        {
            try
            {
                var peliculaModelo = _mapper.Map<Pelicula>(modelo);
                var peliculaEncontrada = await _peliculaRepository.Obtener(p => 
                p.IdPelicula == peliculaModelo.IdPelicula);

                if(peliculaEncontrada.IdPelicula == 0)
                    throw new TaskCanceledException("La pelicula no existe");

                peliculaEncontrada.Titulo = peliculaModelo.Titulo;
                peliculaEncontrada.Sinopsis = peliculaModelo.Sinopsis;
                peliculaEncontrada.Reseña = peliculaModelo.Reseña;

                bool respuesta = await _peliculaRepository.Editar(peliculaEncontrada);

                if(respuesta)
                    throw new TaskCanceledException("No se pudo editar la pelicula");

                return respuesta;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> Eliminar(int id)
        {
            var peliculaEncontrada = await _peliculaRepository.Obtener(p => p.IdPelicula == id);

            if(peliculaEncontrada == null)
                throw new TaskCanceledException("La pelicula no existe");
            bool respuesta = await _peliculaRepository.Eliminar(peliculaEncontrada);

            if (respuesta)
                throw new TaskCanceledException("No se pudo eliminar");

            return respuesta;
        }

        public async Task<PeliculaDTO> Calificar(int id, double calificacion)
        {
            try
            {
                var peliculaEncontrada = await _peliculaRepository.Obtener(p => p.IdPelicula == id);
                if(peliculaEncontrada == null)
                    throw new TaskCanceledException("La pelicula no existe");

                peliculaEncontrada.Calificacion = calificacion;

                await _peliculaRepository.Editar(peliculaEncontrada);

                return _mapper.Map<PeliculaDTO>(peliculaEncontrada);
            }
            catch
            {
                throw;
            }
        }

        public async Task<PeliculaDTO> Comentar(int id, ComentarioDTO comentarioDTO)
        {
            try
            {
                var peliculaEncontrada = await _peliculaRepository.Obtener(p => p.IdPelicula == id);

                if (peliculaEncontrada == null)
                    throw new TaskCanceledException("La película no existe");

                var comentario = _mapper.Map<Comentario>(comentarioDTO);
                comentario.IdPelicula = id;

                var comentarioCreado = await _comentarioRepository.Crear(comentario);

                if (comentarioCreado.IdComentario == 0)
                    throw new TaskCanceledException("No se pudo agregar el comentario");

                return _mapper.Map<PeliculaDTO>(peliculaEncontrada);
            }
            catch
            {
                throw;
            }
        }

    }
}
