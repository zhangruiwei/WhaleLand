using System.Threading.Tasks;

namespace WhaleLand.Extensions.Resilience.Http
{
    public interface IHttpUrlResolver
    {
        Task<string> Resolve(string value);
    }
}
