
namespace WebUi.Helpers.Http
{
    public interface ILaucekHttpClient
    {
        Task<HttpResponseMessage> DeleteAsync(string requestUri);
        Task<Stream> GetStreamAsync(string requestUri);
        Task<string> GetStringAsync(string requestUri);
        Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content);
        Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content);
    }
}