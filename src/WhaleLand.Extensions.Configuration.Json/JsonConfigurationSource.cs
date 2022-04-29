using Microsoft.Extensions.Configuration;

namespace WhaleLand.Extensions.Configuration.Json
{
    public class JsonConfigurationSource : Microsoft.Extensions.Configuration.Json.JsonConfigurationSource
    {
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new JsonConfigurationProvider(this);
        }
    }
}
