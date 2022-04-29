namespace WhaleLand.Extensions.Canal
{
    public interface IFormater
    {
        object Format(Com.Alibaba.Otter.Canal.Protocol.Entry entry);
    }
}
