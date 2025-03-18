using Registro.BLL.Services.ServicesContracts;
using Registro.BLL.Repository.IRepository;
using Registro.DTO;
using Registro.Model.Entities;
using AutoMapper;
using System.Security.Cryptography;
using System.Text;

namespace Registro.BLL.Services
{
    public interface IAuthService
    {
        Task<UsuarioDTO> Registro(UsuarioDTO usuarioDTO);
        Task<TokenDTO> Login(LoginDTO loginDTO);
    }
    public class AuthService : IAuthService
    {
        private readonly IGenericRepository<Usuario> _usuarioRepository;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;

        public AuthService(IGenericRepository<Usuario> usuarioRepository, IJwtService jwtService, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _jwtService = jwtService;
            _mapper = mapper;
        }

        public async Task<UsuarioDTO> Registro(UsuarioDTO usuarioDTO)
        {
            // VALIDACIÓN DE USUSARIO EXISTENTE
            var existeUsuario = await _usuarioRepository.Obtener(u => u.Correo == usuarioDTO.Correo);
            if (existeUsuario != null)
                throw new ArgumentException("El correo ya está registrado");

            var usuario = _mapper.Map<Usuario>(usuarioDTO);

            if(usuario.IdRol == null || usuario.IdRol == 0)
                usuario.IdRol = 4;

            usuario.EsActivo = true;
            usuario.FechaRegistro = DateTime.UtcNow;
            usuario.Clave = HashPassword(usuarioDTO.Clave);

            var usuarioCreado = await _usuarioRepository.Crear(usuario);
            return _mapper.Map<UsuarioDTO>(usuarioCreado);
        }

        public async Task<TokenDTO>Login(LoginDTO loginDTO)
        {
            var usuario = await _usuarioRepository.Obtener(u => u.Correo == loginDTO.Email);

            if(usuario == null || !VerifyPassword(loginDTO.Pass, usuario.Clave))
                throw new ArgumentException("Credenciales inválidas");

            var usuarioDTO = _mapper.Map<UsuarioDTO>(usuario);

            return new TokenDTO
            {
                Token = _jwtService.GenerarToken(loginDTO),
                Expiration = DateTime.UtcNow.AddHours(5).ToString()
            };
        }

        private static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
            
        }

        private static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
