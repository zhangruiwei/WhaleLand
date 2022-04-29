#if NETCORE

using Microsoft.AspNetCore.Builder;

#endif



namespace WhaleLand.Core
{
#if NETCORE

    public interface IWhaleLandApplicationBuilder
    {
        IApplicationBuilder app { get; }
    }

#endif
}
