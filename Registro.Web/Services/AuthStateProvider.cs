using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop; // Para usar IJSRuntime
using System.Security.Claims;
using System.Text.Json; // Para decodificar el token JWT
using System.Threading.Tasks;

namespace Registro.Web.Services
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _jsRuntime; // Para acceder al almacenamiento local del navegador

        public AuthStateProvider(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // Obtener el token JWT desde el almacenamiento local
            var token = await GetTokenFromLocalStorage();

            var identity = new ClaimsIdentity(); // Usuario no autenticado por defecto

            // Si el token existe, decodificarlo y extraer los claims
            if (!string.IsNullOrEmpty(token))
            {
                var claims = ParseClaimsFromJwt(token); // Decodificar el token
                identity = new ClaimsIdentity(claims, "jwt"); // Crear una identidad con los claims
            }

            var user = new ClaimsPrincipal(identity); // Crear el usuario autenticado
            return new AuthenticationState(user); // Devolver el estado de autenticación
        }

        private async Task<string> GetTokenFromLocalStorage()
        {
            // Usar IJSRuntime para leer el token desde el almacenamiento local
            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
        }

        public void NotifyAuthenticationStateChanged()
        {
            // Notificar a los componentes que el estado de autenticación ha cambiado
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            // Decodificar el token JWT y extraer los claims
            var payload = jwt.Split('.')[1]; // Obtener el payload del token
            var jsonBytes = ParseBase64WithoutPadding(payload); // Convertir de Base64 a bytes
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes); // Deserializar el JSON

            // Convertir los pares clave-valor en claims
            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            // Añadir padding si es necesario para que la longitud sea múltiplo de 4
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64); // Convertir de Base64 a bytes
        }
    }
}
