using System;
using Xunit;

namespace WhaleLand.Extensions.UnitTest
{
    public class CacheTest
    {
        [Fact]
        public void Add()
        {
            try
            {
                var cache = WhaleLand.Extensions.Cache.CacheFactory.Build<string>(config =>
                {
                    config.WithDatabase(0);
                    config.WithEndpoint("localhost", 6379);
                    config.WithPassword("");
                }, "WhaleLand.Test");

                cache.Add("TestKey", "TestValue", TimeSpan.FromSeconds(10), "WhaleLand.Test");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        [Fact]
        public void Get()
        {
            try
            {
                var cache = WhaleLand.Extensions.Cache.CacheFactory.Build<string>(config =>
                {
                    config.WithDatabase(0);
                    config.WithEndpoint("localhost", 6379);
                    config.WithPassword("");
                }, "WhaleLand.Test");

                var val = cache.Get("TestKey", "WhaleLand.Test");

                Assert.True("TestValue".Equals(val));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
