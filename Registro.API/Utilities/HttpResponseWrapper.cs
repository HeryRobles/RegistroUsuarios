namespace Registro.API.Utilities
{
    public class HttpResponseWrapper<T>
    {
        public bool status { get; set; }
        public T value { get; set; }
        public string? HttpResponseMessage { get; set; }
    }
}
