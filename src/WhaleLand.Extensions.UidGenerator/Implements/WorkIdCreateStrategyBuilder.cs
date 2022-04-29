using Microsoft.Extensions.DependencyInjection;

namespace WhaleLand.Extensions.UidGenerator
{
    class WorkIdCreateStrategyBuilder : IWorkIdCreateStrategyBuilder
    {
        private readonly IServiceCollection _service;

        public WorkIdCreateStrategyBuilder(IServiceCollection service)
        {
            this._service = service;
        }

        public IServiceCollection Services { get { return _service; } }

        public int CenterId { get; set; }
    }
}
