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
        private readonly IGenericRepository<Rol> _rolRepository;
        private readonly IMapper _mapper;

        public UsuarioService(IGenericRepository<Usuario> usuarioRepository, IGenericRepository<Rol> rolRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _rolRepository = rolRepository;
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

        public async Task<UsuarioDTO> Crear(UsuarioDTO modelo)
        {
            try
            {
                if (modelo.IdRol == 1 || modelo.IdRol == 2 || modelo.IdRol == 3)
                
                    throw new InvalidOperationException("El rol especificado no puede ser dado de alta");
                
                var usuarioModelo = _mapper.Map<Usuario>(modelo);
                    var usuarioCreado = await _usuarioRepository.Crear(usuarioModelo);
                    if (usuarioCreado.IdUsuario == 0)
                    throw new InvalidOperationException("Error al crear el usuario");

                    var usuarioConRol = await _usuarioRepository
                        .Consultar(u => u.IdUsuario == usuarioCreado.IdUsuario);

                usuarioCreado = await usuarioConRol.Include(rol => rol.IdRolNavigation).FirstOrDefaultAsync();

                return _mapper.Map<UsuarioDTO>(usuarioCreado);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al dar de alta el usuario", ex);
            }
        }


        /*En este método, el Administrador podrá asignar un rol a un usuario ya registrado, por 
         * ejemplo si un usuario se registró como Cliente y el Administrador desea que sea Empleado
         * o al reves, igual a un empleado lo podrá cambiar a Supervisor
         */
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

        /*En este método solo se podrá editar los datos del usuario, como el nombre, correo, rol y si está activo o no
         * y será accesiblo solo por el administrador, supervisor y empleado.
         */
        public async Task<bool> Editar(UsuarioDTO modelo, int idUsuarioActual, string rolUsuarioActual)
        {
            try
            {
                var usuarioModelo = _mapper.Map<Usuario>(modelo);

                var usuarioEncontrado = await _usuarioRepository.Obtener(u => u.IdUsuario == usuarioModelo.IdUsuario);

                if (usuarioEncontrado == null)
                    throw new TaskCanceledException("El usuario no existe");

                // Validar permisos según el rol del usuario actual
                if (rolUsuarioActual == "Cliente")
                {
                    // El cliente solo puede editar su propia contraseña
                    if (usuarioEncontrado.IdUsuario != idUsuarioActual)
                        throw new UnauthorizedAccessException("No tienes permiso para editar este usuario");

                    usuarioEncontrado.Clave = usuarioModelo.Clave;
                }
                else if (rolUsuarioActual == "Supervisor" || rolUsuarioActual == "Empleado")
                {
                    // El Supervisor y empleado pueden editar su propio nombre, correo y contraseña
                    if (usuarioEncontrado.IdUsuario != idUsuarioActual)
                        throw new UnauthorizedAccessException("No tienes permiso para editar este usuario");

                    usuarioEncontrado.NombreCompleto = usuarioModelo.NombreCompleto;
                    usuarioEncontrado.Correo = usuarioModelo.Correo;
                    usuarioEncontrado.Clave = usuarioModelo.Clave;
                }
                else if (rolUsuarioActual == "Administrador") //El Administrador puede editar cualquier usuario
                {
                    if (usuarioEncontrado.IdUsuario == idUsuarioActual)
                    {
                        
                        usuarioEncontrado.NombreCompleto = usuarioModelo.NombreCompleto; //Excepto su propio Rol y estado
                        usuarioEncontrado.Correo = usuarioModelo.Correo;
                        usuarioEncontrado.Clave = usuarioModelo.Clave;
                    }
                    else
                    {
                        usuarioEncontrado.NombreCompleto = usuarioModelo.NombreCompleto;
                        usuarioEncontrado.Correo = usuarioModelo.Correo;
                        usuarioEncontrado.Clave = usuarioModelo.Clave;
                        usuarioEncontrado.IdRol = usuarioModelo.IdRol;
                        usuarioEncontrado.EsActivo = usuarioModelo.EsActivo;
                    }
                }
                else
                    throw new UnauthorizedAccessException("No tienes permiso para editar usuarios");

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
