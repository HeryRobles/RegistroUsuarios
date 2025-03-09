using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Registro.DTO;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

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
            var response = await _http.PostAsJsonAsync("api/auth/login", loginDto);

            if (response.IsSuccessStatusCode)
            {
                var sesionDto = await response.Content.ReadFromJsonAsync<SesionDTO>();

                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", sesionDto.Token);

                ((AuthStateProvider)_authStateProvider).NotifyAuthenticationStateChanged();

                return true;
            }

            return false;
        }

        public async Task Logout()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");

            ((AuthStateProvider)_authStateProvider).NotifyAuthenticationStateChanged();
        }

    }
}
