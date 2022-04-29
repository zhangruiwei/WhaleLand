using System.Net.Http;

namespace WhaleLand.Extensions.Resilience.Http
{
    public interface IHttpClientFactory
    {
        IHttpClient CreateResilientHttpClient();

        IHttpClient CreateResilientHttpClient(HttpMessageHandler httpMessageHandler);
    }
}
