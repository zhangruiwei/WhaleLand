using System.Collections.Generic;

namespace WhaleLand.Extensions.Canal
{
    public interface IConnector
    {
        bool Process(List<Com.Alibaba.Otter.Canal.Protocol.Entry> entrys, IFormater formater);
    }
}
