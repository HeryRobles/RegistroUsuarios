//using Registro.DTO;
//using System.Net.Http.Json;
//using System.Net.Http;


//namespace Registro.Client.Services
//{
//    public class AuthService
//    {
//        private readonly HttpClient _httpClient;
//        public AuthService(HttpClient httpClient)
//        {
//            _httpClient = httpClient;

//        }

//        public async Task<SesionDTO> Login(LoginDTO login)
//        {
//            var response = await _httpClient.PostAsJsonAsync("api/Usuario/IniciarSesion", login);
//            if (response.IsSuccessStatusCode)
//            {
//                var result = await response.Content.ReadFromJsonAsync<HttpResponseWrapper<SesionDTO>>();
//                return result.value;
//            }
//            else
//            {
//                throw new System.Exception("Error al iniciar sesión");
//            }


//        }

//    }
//}

