﻿namespace WhaleLand.Extensions.Idempotency
{
    public interface IRequestManager
    {
        ClientRequest Find(string id);

        ClientRequest CreateRequestForCommand<T, R>(
            string id,
            System.DateTime RequestTime,
            System.DateTime ResponseTime,
            T command,
            R response);
    }
}
