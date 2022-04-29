#if NETCORE
using Microsoft.AspNetCore.Builder;

namespace WhaleLand.Core
{
    public class WhaleLandApplicationBuilder : IWhaleLandApplicationBuilder
    {
        private readonly IApplicationBuilder _app;

        public WhaleLandApplicationBuilder(IApplicationBuilder app)
        {
            this._app = app;
        }

        public IApplicationBuilder app
        {
            get
            {
                return _app;
            }
        }
    }
} 
#endif
