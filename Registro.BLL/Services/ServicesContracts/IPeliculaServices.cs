using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Registro.DTO;

namespace Registro.BLL.Services.ServicesContracts
{
    public interface IPeliculaServices
    {
        Task<List<PeliculaDTO>> Lista();
        Task<PeliculaDTO> Obtener(int id);
        Task<PeliculaDTO> Crear(PeliculaDTO modelo);
        Task<bool> Editar(PeliculaDTO modelo);
        Task<bool> Eliminar(int id);
    }
}
