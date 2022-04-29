using System.Threading.Tasks;

namespace WhaleLand.Extensions.UidGenerator
{
    public interface IWorkIdCreateStrategy
    {
        Task<int> NextId();
    }
}
