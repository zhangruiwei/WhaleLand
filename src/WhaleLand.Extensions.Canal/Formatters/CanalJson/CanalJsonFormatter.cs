namespace WhaleLand.Extensions.Canal.Formatters.CanalJson
{
    public class Formatter : IFormater
    {
        public object Format(Com.Alibaba.Otter.Canal.Protocol.Entry entry)
        {
            return entry;
        }
    }
}
