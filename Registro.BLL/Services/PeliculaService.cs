using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Registro.BLL.Services.ServicesContracts;
using Registro.DAL.Repository.IRepository;
using Registro.DTO;
using Registro.Model.Entities;

namespace Registro.BLL.Services
{
    public class PeliculaService : IPeliculaServices
    {
        private readonly IGenericRepository<Pelicula> _peliculaRepository;
        private readonly IMapper _mapper;

        public PeliculaService(IGenericRepository<Pelicula> peliculaRepository, IMapper mapper)
        {
            _peliculaRepository = peliculaRepository;
            _mapper = mapper;
        }

        public Task<PeliculaDTO> Crear(PeliculaDTO modelo)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Editar(PeliculaDTO modelo)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<PeliculaDTO>> Lista()
        {
            throw new NotImplementedException();
        }

        public Task<PeliculaDTO> Obtener(int id)
        {
            throw new NotImplementedException();
        }
    }
}
