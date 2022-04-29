using System;
using System.Collections.Generic;

namespace WhaleLand.LoadBalancers
{
    public class DefaultLoadBalancerFactory<T> : ILoadBalancerFactory<T>
    {

        public ILoadBalancer<T> Get(Func<List<T>> func, string Type = "RoundRobin")
        {
            switch (Type)
            {
                case "RoundRobin":
                case "RoundRobinLoadBalancer":
                    return new RoundRobinLoadBalancer<T>(func);
                case "RandomRobin":
                case "RandomRobinLoadBalancer":
                    return new RandomRobinLoadBalancer<T>(func);
                default:
                    return new RoundRobinLoadBalancer<T>(func);
            }
        }
    }
}
