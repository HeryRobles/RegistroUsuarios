using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Registro.BLL.Services.ServicesContracts;
using Registro.BLL.Repository.IRepository;
using Registro.DTO;
using Registro.Model.Entities;
using System.ComponentModel.Design;

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

        /*Metodo para obtener la lista de usuarios
         * Esta lista de usuarios solo puede acceder el Administrador
         */
        public async Task<List<UsuarioDTO>> Lista()
        {
            try
            {
                var query = await _usuarioRepository.Consultar();

                var listaUsuarios = await query.ToListAsync();

                return _mapper.Map<List<UsuarioDTO>>(listaUsuarios);

            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de usuarios", ex);
            }
        }

        public async Task<UsuarioDTO> Obtener(int id)
        {
            try
            {
                var usuario = await _usuarioRepository.Obtener(p => p.IdUsuario == id);
                if (usuario == null)
                    throw new InvalidOperationException("El usuario no existe.");

                return _mapper.Map<UsuarioDTO>(usuario);
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
                if (modelo == null)
                    throw new ArgumentNullException(nameof(modelo), "El modelo no puede ser nulo.");

                var usuarioCreado = await _usuarioRepository.Crear(_mapper.Map<Usuario>(modelo));
                if (usuarioCreado.IdUsuario == 0)
                    throw new Exception("Error al crear el usuario");

                var query = await _usuarioRepository.Consultar(u =>
                u.IdUsuario == usuarioCreado.IdUsuario);

                usuarioCreado = query.Include(u => u.IdRolNavigation)
                    .FirstOrDefault();

                return _mapper.Map<UsuarioDTO>(usuarioCreado);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> Editar(UsuarioDTO modelo, int idUsuarioActual, string rolUsuarioActual)
        {
            try
            {
                if(modelo == null)
                    throw new ArgumentNullException(nameof(modelo), "El modelo no puede ser nulo.");

                var usuarioModelo = _mapper.Map<Usuario>(modelo);

                var usuarioEncontrado = await _usuarioRepository.Obtener(u => 
                u.IdUsuario == usuarioModelo.IdUsuario);

                if (usuarioEncontrado == null)
                    throw new TaskCanceledException("El usuario no existe");

                usuarioEncontrado.NombreCompleto = usuarioModelo.NombreCompleto;
                usuarioEncontrado.Correo = usuarioModelo.Correo;
                usuarioEncontrado.IdRol = usuarioModelo.IdRol;
                usuarioEncontrado.Clave = usuarioModelo.Clave;

                bool respuesta = await _usuarioRepository.Editar(usuarioEncontrado);
                if(!respuesta)
                    throw new Exception("Error al editar el usuario");

                return respuesta;
            }
            catch (Exception ex)
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
                    throw new Exception("Error al eliminar el usuario");

                return respuesta;
            }
            catch 
            {
                throw;
            }
        }

        
    }
}
