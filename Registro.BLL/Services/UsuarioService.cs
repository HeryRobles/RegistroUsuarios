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
        private readonly IGenericRepository<Rol> _rolRepository;
        private readonly IMapper _mapper;

        public UsuarioService(IGenericRepository<Usuario> usuarioRepository, IGenericRepository<Rol> rolRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _rolRepository = rolRepository;
            _mapper = mapper;
        }

        public async Task<List<UsuarioDTO>> Lista()
        {
            try
            {
                var query = await _usuarioRepository.Consultar();

                var usuarios = await query
                    .Include(u => u.IdRolNavigation)
                    .ToListAsync();

                return _mapper.Map<List<UsuarioDTO>>(usuarios);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de usuarios", ex);
            }
        }

        public async Task<SesionDTO> ValidarCredenciales(string correo, string clave)
        {
            try
            {
                var query = await _usuarioRepository.Consultar(u => u.Correo == correo && u.Clave == clave);

                var usuario = await query
                    .Include(u => u.IdRolNavigation)
                    .FirstOrDefaultAsync();

                return _mapper.Map<SesionDTO>(usuario);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al validar credenciales", ex);
            }
        }
        public async Task<bool> AsignarRol(int usuarioId, int nuevoRolId)
        {
            try
            {
                var usuario = await _usuarioRepository.Obtener(u => u.IdUsuario == usuarioId);
                if (usuario == null)
                    throw new TaskCanceledException("El usuario no existe");

                var rolExiste = await _rolRepository.Obtener(r => r.IdRol == nuevoRolId);
                if (rolExiste == null)
                    throw new TaskCanceledException("El rol especificado no existe");

                usuario.IdRol = nuevoRolId;

                bool respuesta = await _usuarioRepository.Editar(usuario);


                return respuesta;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la asignación de roles", ex);
            }
        }


        public async Task<UsuarioDTO> Crear(UsuarioDTO modelo)
        {
            try
            {
                var usuarioModelo = _mapper.Map<Usuario>(modelo);
                var usuarioCreado = await _usuarioRepository.Crear(usuarioModelo);


                if (usuarioCreado.IdUsuario == 0)
                    throw new TaskCanceledException("Error al crear el usuario");


                var usuarioConRol = await _usuarioRepository
                    .Consultar(u => u.IdUsuario == usuarioCreado.IdUsuario);

                usuarioCreado = usuarioConRol.Include(rol => rol.IdRolNavigation).First();

                return _mapper.Map<UsuarioDTO>(usuarioCreado);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear el usuario", ex);
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


                return respuesta;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al editar el usuario", ex);
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

                return respuesta;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el usuario", ex);
            }
        }
    }
}
