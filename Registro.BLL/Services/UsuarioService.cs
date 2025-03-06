using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Registro.BLL.Services.ServicesContracts;
using Registro.DAL.Repository.IRepository;
using Registro.DTO;
using Registro.Model.Entities;

namespace Registro.BLL.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IGenericRepository<Usuario> _usuarioRepository;
        private readonly IMapper _mapper;

        public UsuarioService(IGenericRepository<Usuario> usuarioRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
        }

        public async Task<List<UsuarioDTO>> Lista()
        {
            try
            {
                var queryUsuario = await _usuarioRepository.Consultar();
                var listaUsuario = queryUsuario.Include(rol => rol.IdRolNavigation).ToList();
                return _mapper.Map<List<UsuarioDTO>>(listaUsuario);
            }
            catch
            {
                throw;
            }
        }

        public async Task<SesionDTO> ValidarCredenciales(string correo, string clave)
        {
            try
            {
                var queryUsuario = await _usuarioRepository.Consultar(u => u.Correo == correo
                && u.Clave == clave
                );

                if (queryUsuario.FirstOrDefault() == null)
                    throw new TaskCanceledException("El Usuario no existe");

                Usuario returnUsuario = queryUsuario.Include(rol => rol.IdRolNavigation).First();

                return _mapper.Map<SesionDTO>(returnUsuario);
            }
            catch
            {
                throw;
            }
        }
        public async Task<UsuarioDTO> Crear(UsuarioDTO modelo)
        {
            try
            {
                var usuarioCreado = await _usuarioRepository.Crear(_mapper.Map<Usuario>(modelo));


                if (usuarioCreado.IdUsuario == 0)
                    throw new TaskCanceledException("Error al crear el usuario");

                var query = await _usuarioRepository.Consultar(u => u.IdUsuario == usuarioCreado.IdUsuario);

                usuarioCreado = query.Include(rol => rol.IdRolNavigation).First();

                return _mapper.Map<UsuarioDTO>(usuarioCreado);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(UsuarioDTO modelo)
        {
            try
            {
                var usuaioModelo = _mapper.Map<Usuario>(modelo);
                var usuarioEncontrado = await _usuarioRepository.Obtener(u => u.IdUsuario == usuaioModelo.IdUsuario);

                if (usuarioEncontrado == null)
                    throw new TaskCanceledException("El usuario no existe");

                usuarioEncontrado.NombreCompleto = usuaioModelo.NombreCompleto;
                usuarioEncontrado.Correo = usuaioModelo.Correo;
                usuarioEncontrado.IdRol = usuaioModelo.IdRol;
                usuarioEncontrado.Clave = usuaioModelo.Clave;
                usuarioEncontrado.EsActivo = usuaioModelo.EsActivo;

                bool respuesta = await _usuarioRepository.Editar(usuarioEncontrado);

                if (!respuesta)
                    throw new TaskCanceledException("Error al editar el usuario");

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
                var usuarioEncontrado = await _usuarioRepository.Obtener(u => u.IdUsuario == id);

                if (usuarioEncontrado == null)
                    throw new TaskCanceledException("El usuario no existe");

                bool respuesta = await _usuarioRepository.Eliminar(usuarioEncontrado);
                if (!respuesta)
                    throw new TaskCanceledException("Error al eliminar el usuario");

                return respuesta;
            }
            catch
            {
                throw;
            }
        }
    }
}
