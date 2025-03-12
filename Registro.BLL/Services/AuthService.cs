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

        public async Task<UsuarioDTO> Registro(RegistroUsuarioDTO registroDTO)
        {
            // VALIDACIÓN DE USUSARIO EXISTENTE
            var existeUsuario = await _usuarioRepository.Obtener(u => u.Correo == registroDTO.Correo);
            if (existeUsuario != null)
                throw new ArgumentException("El correo ya está registrado");

            var usuario = _mapper.Map<Usuario>(registroDTO);

            usuario.EsActivo = true;
            usuario.FechaRegistro = DateTime.UtcNow;

            usuario.Clave = HashPassword(registroDTO.Clave);

            // SE ASIGNA EL ROL "CLIENTE" POR DEFECTO, NO SE DEBE DE INCLUIR AL MOMENTO DE REGISTRARSE
            usuario.IdRol = registroDTO.IdRol ?? 4; 

            var usuarioCreado = await _usuarioRepository.Crear(usuario);
            return _mapper.Map<UsuarioDTO>(usuarioCreado);
        }

        public async Task<TokenDTO> Login(LoginDTO loginDTO)
        {
            var usuario = await _usuarioRepository.Obtener(u => u.Correo == loginDTO.Correo);

            if (usuario == null || !VerifyPassword(loginDTO.Clave, usuario.Clave))
                throw new UnauthorizedAccessException("Credenciales inválidas");

            var usuarioDTO = _mapper.Map<UsuarioDTO>(usuario);

            return new TokenDTO
            {
                Token = _jwtService.GenerarToken(usuarioDTO),
                Expiration = DateTime.UtcNow.AddHours(8).ToString("o")
            };
        }

        private static string HashPassword(string password)
        {
            byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashBytes);
        }

        private static bool VerifyPassword(string password, string hashedPassword)
        {
            return HashPassword(password) == hashedPassword;
        }
    }
}
