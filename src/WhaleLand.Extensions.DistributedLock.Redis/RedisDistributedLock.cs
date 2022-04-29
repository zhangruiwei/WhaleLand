using Polly;
using System;
using WhaleLand.Extensions.Redis;

namespace WhaleLand.Extensions.DistributedLock.Redis
{
    public class RedisDistributedLock : IDistributedLock
    {
        private readonly ICacheManager _cacheManager;

        public RedisDistributedLock(ICacheManager cacheManager)
        {
            this._cacheManager = cacheManager;
        }

        public bool Enter(string LockName, string LockToken, TimeSpan LockOutTime, int retryAttemptMillseconds = 50, int retryTimes = 5)
        {
            if (_cacheManager != null)
            {
                var cacheKey = "Lock:" + LockName;
                do
                {
                    if (!_cacheManager.LockTake(cacheKey, LockToken, LockOutTime))
                    {
                        retryTimes--;
                        if (retryTimes < 0)
                        {
                            return false;
                        }

                        if (retryAttemptMillseconds > 0)
                        {
                            Console.WriteLine($"Wait Lock {LockName} to {retryAttemptMillseconds} millseconds");
                            //获取锁失败则进行锁等待
                            System.Threading.Thread.Sleep(retryAttemptMillseconds);
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
                while (retryTimes > 0);
            }

            return false;
        }

        public void Exit(string LockName, string LockToken)
        {
            if (_cacheManager != null)
            {

                var polly = Policy.Handle<Exception>()
                    .WaitAndRetry(10, retryAttempt => TimeSpan.FromMilliseconds(Math.Pow(2, retryAttempt)), (exception, timespan, retryCount, context) =>
                    {
                        Console.WriteLine($"执行异常,重试次数：{retryCount},【异常来自：{exception.GetType().Name}】.");
                    });

                polly.Execute(() =>
                {
                    _cacheManager.LockRelease("Lock:" + LockName, LockToken);
                });
            }
        }
    }
}
