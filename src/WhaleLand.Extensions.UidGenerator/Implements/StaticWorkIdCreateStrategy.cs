using System.Threading.Tasks;

namespace WhaleLand.Extensions.UidGenerator
{
    class StaticWorkIdCreateStrategy : IWorkIdCreateStrategy
    {
        private readonly int _workId;

        public StaticWorkIdCreateStrategy(int workId)
        {
            _workId = workId;
        }

        public Task<int> NextId()
        {
            return Task.FromResult(_workId);
        }
    }
}
