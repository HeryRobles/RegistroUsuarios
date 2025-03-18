using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Registro.BLL.Services.ServicesContracts;
using Registro.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Registro.BLL.Services
{
    public interface IJwtService
    {
        string GenerarToken(LoginDTO modelo);
    }
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerarToken(LoginDTO usuario)
        {
            if (string.IsNullOrEmpty(_configuration["Jwt:Key"]))
                throw new InvalidOperationException("La clave JWT no está configurada correctamente.");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Email.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTimeOffset.UtcNow.AddHours(5).UtcDateTime,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
