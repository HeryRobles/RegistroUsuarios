using AutoMapper;
using Registro.BLL.Services.ServicesContracts;
using Registro.BLL.Repository.IRepository;
using Registro.DTO;
using Registro.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<PeliculaDTO> Obtener(int id)
        {
            try
            {
                var pelicula = await _peliculaRepository.Obtener(p => p.IdPelicula == id);
                if (pelicula == null)
                    throw new InvalidOperationException("La película no existe.");

                return _mapper.Map<PeliculaDTO>(pelicula);
            }
            catch
            {
                throw;
            }
        }

        public async Task<PeliculaDTO> Crear(PeliculaDTO modelo)
        {
            try
            {
                if (modelo == null)
                    throw new ArgumentNullException(nameof(modelo), "El modelo no puede ser nulo.");

                var peliculaCreada = await _peliculaRepository.Crear(_mapper.Map<Pelicula>(modelo));

                if (peliculaCreada.IdPelicula == 0)
                    throw new InvalidOperationException("No se pudo crear la película.");

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
                if (modelo == null)
                    throw new ArgumentNullException(nameof(modelo), "El modelo no puede ser nulo.");

                var peliculaModelo = _mapper.Map<Pelicula>(modelo);
                var peliculaEncontrada = await _peliculaRepository.Obtener(p => p.IdPelicula == peliculaModelo.IdPelicula);

                if (peliculaEncontrada == null)
                    throw new InvalidOperationException("La película no existe.");

                peliculaEncontrada.Titulo = peliculaModelo.Titulo;
                peliculaEncontrada.Sinopsis = peliculaModelo.Sinopsis;
                peliculaEncontrada.Reseña = peliculaModelo.Reseña;

                bool respuesta = await _peliculaRepository.Editar(peliculaEncontrada);

                if (!respuesta)
                    throw new InvalidOperationException("No se pudo editar la película.");

                return respuesta;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var peliculaEncontrada = await _peliculaRepository.Obtener(p => p.IdPelicula == id);

                if (peliculaEncontrada == null)
                    throw new InvalidOperationException("La película no existe.");

                bool respuesta = await _peliculaRepository.Eliminar(peliculaEncontrada);

                if (!respuesta)
                    throw new InvalidOperationException("No se pudo eliminar la película.");

                return respuesta;
            }
            catch
            {
                throw;
            }
        }

        public async Task<PeliculaDTO> Calificar(int id, double calificacion)
        {
            try
            {
                if (calificacion < 0 || calificacion > 10)
                    throw new ArgumentOutOfRangeException(nameof(calificacion), "La calificación debe estar entre 0 y 10.");

                var peliculaEncontrada = await _peliculaRepository.Obtener(p => p.IdPelicula == id);
                if (peliculaEncontrada == null)
                    throw new InvalidOperationException("La película no existe.");

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
                if (comentarioDTO == null)
                    throw new ArgumentNullException(nameof(comentarioDTO), "El comentario no puede ser nulo.");

                var peliculaEncontrada = await _peliculaRepository.Obtener(p => p.IdPelicula == id);

                if (peliculaEncontrada == null)
                    throw new InvalidOperationException("La película no existe.");

                var comentario = _mapper.Map<Comentario>(comentarioDTO);
                comentario.IdPelicula = id;

                var comentarioCreado = await _comentarioRepository.Crear(comentario);

                if (comentarioCreado.IdComentario == 0)
                    throw new InvalidOperationException("No se pudo agregar el comentario.");

                return _mapper.Map<PeliculaDTO>(peliculaEncontrada);
            }
            catch
            {
                throw;
            }
        }
    }
}