using System;
using System.Collections.Generic;

namespace WhaleLand.LoadBalancers
{
    public interface ILoadBalancerFactory<T>
    {
        ILoadBalancer<T> Get(Func<List<T>> func, string Type);
    }
}
