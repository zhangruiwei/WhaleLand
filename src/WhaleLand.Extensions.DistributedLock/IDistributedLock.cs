﻿using System;

namespace WhaleLand.Extensions.DistributedLock
{
    public interface IDistributedLock
    {
        /// <summary>
        /// 加锁
        /// </summary>
        /// <param name="LockName"></param>
        /// <param name="LockToken"></param>
        /// <param name="LockOutTime">锁保持时间</param>
        /// <param name="retryAttemptMillseconds">获取锁失败重试间隔</param>
        /// <param name="retryTimes">最大重试次数</param>
        /// <returns></returns>
        bool Enter(
            string LockName,
            string LockToken,
            TimeSpan LockOutTime, int retryAttemptMillseconds = 50, int retryTimes = 5);

        /// <summary>
        /// 释放锁
        /// </summary>
        void Exit(
            string LockName,
            string LockToken);
    }
}
