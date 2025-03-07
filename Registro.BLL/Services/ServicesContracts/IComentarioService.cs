using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Registro.DTO;

namespace Registro.BLL.Services.ServicesContracts
{
    public interface IComentarioService
    {
        Task<List<ComentarioDTO>> Lista();
        Task<ComentarioDTO> Crear(ComentarioDTO modelo);
        Task<bool> Eliminar(int id);
    }
}
