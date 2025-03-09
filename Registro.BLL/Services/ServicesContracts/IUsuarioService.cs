using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Registro.DTO;

namespace Registro.BLL.Services.ServicesContracts
{
    public interface IUsuarioService
    {
        Task<List<UsuarioDTO>> Lista();
        Task<SesionDTO> Login (string correo, string clave);
        Task<UsuarioDTO> Registrar (UsuarioDTO modelo);
        Task<UsuarioDTO> DarDeAlta (UsuarioDTO modelo);
        Task<bool> Editar(UsuarioDTO modelo, int idUsuarioActual, string rolUsuarioActual);
        Task<bool> Eliminar(int id);
        Task<bool> AsignarRol(int usuarioId, int nuevoRolId);
    }
}
