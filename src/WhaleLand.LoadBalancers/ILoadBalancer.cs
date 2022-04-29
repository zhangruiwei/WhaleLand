﻿using System.Collections.Generic;

namespace WhaleLand.LoadBalancers
{
    public interface ILoadBalancer<T>
    {
        T Lease();

        T Lease(List<T> connections);

    }
}
