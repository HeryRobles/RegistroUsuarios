using System.ComponentModel.Design;
using System.Net;

namespace Registro.Web.Repositories
{
    public class HttpResponseWrapper<T>
    {
        public HttpResponseWrapper(T? response, bool error, HttpResponseMessage httpResponseMessage) 
        {
            Error = error;
            Response = response;
            HttpResponseMessage = httpResponseMessage;
        }

        public bool Error { get; private set; }
        public HttpResponseMessage HttpResponseMessage { get; set; }
        public T? Response { get; set; } 


        public async Task<string?> GetErrorMessageAsync()
        {
            if (!Error)
            {
                return null;
            }

            var statusCode = HttpResponseMessage.StatusCode;
            if (statusCode == HttpStatusCode.NotFound)
                return "Recurso no encontrado";
            else if (statusCode == HttpStatusCode.BadRequest)
                return await HttpResponseMessage.Content.ReadAsStringAsync();
            else if (statusCode == HttpStatusCode.Unauthorized)
                return "Tienes que loguearte para hacer esta operación";
            else if (statusCode == HttpStatusCode.Forbidden)
                return "No tienes los permisos para hacer esta opéración";
            else
                return "Ha ocurridom un error inesperado";
            

        }
    }
}
