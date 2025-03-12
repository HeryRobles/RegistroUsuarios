using Registro.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registro.BLL.Services.ServicesContracts
{
    public interface IJwtService
    {
        string GenerarToken(UsuarioDTO modelo);
    }
}
