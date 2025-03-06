using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Registro.BLL.Services.ServicesContracts;
using Registro.DAL.Repository.IRepository;
using Registro.DTO;
using Registro.Model.Entities;

namespace Registro.BLL.Services
{
    public class RolService : IRolService
    {
        private readonly IGenericRepository<Rol> _rolRepository;
        private readonly IMapper _mapper;

        public RolService(IGenericRepository<Rol> rolRepository, IMapper mapper)
        {
            _rolRepository = rolRepository;
            _mapper = mapper;
        }

        public async Task<List<RolDTO>> Lista()
        {
            try
            {
                var listaRoles = await _rolRepository.Consultar();
                return _mapper.Map<List<RolDTO>>(listaRoles);
            }
            catch 
            {
                throw;
            }
        }
    }
}
