using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Registro.DTO;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Registro.Web.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _jsRuntime;
        private readonly AuthenticationStateProvider _authStateProvider;

        public AuthService(HttpClient http, IJSRuntime jsRuntime, AuthenticationStateProvider authStateProvider)
        {
            _http = http;
            _jsRuntime = jsRuntime;
            _authStateProvider = authStateProvider;
        }

        public async Task<bool> Login(LoginDTO loginDto)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/Usuario/login", loginDto);

                if (response.IsSuccessStatusCode)
                {
                    var sesionDto = await response.Content.ReadFromJsonAsync<SesionDTO>();

                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", sesionDto.Token);

                    ((AuthStateProvider)_authStateProvider).NotifyAuthenticationStateChanged();

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al iniciar sesión: {ex.Message}");
                return false;
            }
        }

        public async Task AddTokenToHttpClient()
        {
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
            if (!string.IsNullOrEmpty(token))
            {
                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task Logout()
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");

                ((AuthStateProvider)_authStateProvider).NotifyAuthenticationStateChanged();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cerrar sesión: {ex.Message}");
            }
        }

        public async Task<string> GetToken()
        {
            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
        }
    }
}