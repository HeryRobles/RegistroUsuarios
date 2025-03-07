using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Registro.BLL.Services.ServicesContracts;
using Registro.DAL.Repository.IRepository;
using Registro.DTO;
using Registro.Model.Entities;

namespace Registro.BLL.Services
{
    public class ComentarioService : IComentarioService
    {
        private readonly IGenericRepository<Comentario> _comentarioRepository;
        private readonly IMapper _mapper;

        public ComentarioService(IGenericRepository<Comentario> comentarioRepository, IMapper mapper)
        {
            _comentarioRepository = comentarioRepository;
            _mapper = mapper;
        }

        public async Task<List<ComentarioDTO>> Lista()
        {
            try
            {
                var queryComentario = await _comentarioRepository.Consultar();
                var listaComentario = queryComentario.ToList();
                return _mapper.Map<List<ComentarioDTO>>(listaComentario);
            }
            catch
            {
                throw;
            }
        }

        public async Task<ComentarioDTO> Crear(ComentarioDTO modelo)
        {
            try
            {
                var comentarioCreado = await _comentarioRepository.Crear(_mapper.Map<Comentario>(modelo));
                if (comentarioCreado.IdComentario == 0)
                    throw new TaskCanceledException("No se pudo crear el comentario");
                return _mapper.Map<ComentarioDTO>(comentarioCreado);
            }
            catch
            {
                throw;
            }

        }
        public async Task<bool> Eliminar(int id)
        {
            var comentarioEncontrado = await _comentarioRepository.Obtener(c => c.IdComentario == id);

            if (comentarioEncontrado == null)
                throw new TaskCanceledException("El Comentario no existe");
            bool respuesta = await _comentarioRepository.Eliminar(comentarioEncontrado);

            if (respuesta)
                throw new TaskCanceledException("No se pudo eliminar");

            return respuesta;
        }

        public Task<ComentarioDTO> Obtener(int id)
        {
            throw new NotImplementedException();
        }
    }
}
