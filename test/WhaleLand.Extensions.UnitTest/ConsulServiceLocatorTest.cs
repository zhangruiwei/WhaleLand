using System.Linq;
using WhaleLand.DynamicRoute;
using WhaleLand.Extensions.DynamicRoute.Consul;
using Xunit;

namespace WhaleLand.Extensions.UnitTest
{

    public class ConsulServiceLocatorTest
    {
        [Fact]
        public async void when_tag_exists_success()
        {
            IServiceLocator serviceLocator = new ConsulServiceLocator("localhost", "8500", "dc1", "");
            var t = await serviceLocator.GetAsync("rfs-api", "dev");
            Assert.True(t.Count() > 0);
        }

        [Fact]
        public async void when_tag_notexists_success()
        {
            IServiceLocator serviceLocator = new ConsulServiceLocator("localhost", "8500", "dc1", "");
            var t = await serviceLocator.GetAsync("rfs-api", "ddd");
            Assert.True(t.Count() == 0);
        }

    }
}
