namespace WhaleLand.Example.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;
    using WhaleLand.Extensions.DistributedLock;

    [Route("api/[controller]")]
    public class DistributedLockController : Controller
    {
        private readonly IDistributedLock _distributedLock;

        public DistributedLockController(IDistributedLock distributedLock)
        {
            _distributedLock = distributedLock;
        }

        [HttpGet]
        [Route("Test")]
        public async Task<IActionResult> Test()
        {
            var lockName = "name";
            var lockToken = Guid.NewGuid().ToString("N");
            try
            {
                if (_distributedLock.Enter(lockName, lockToken, TimeSpan.FromSeconds(30), retryAttemptMillseconds: 1000, retryTimes: 3))
                {
                    await Task.Delay(10 * 1000);

                    return Json(new
                    {
                        Result = true
                    });
                }
                else
                {
                    return Json(new
                    {
                        Result = false
                    });
                }
            }
            finally
            {
                _distributedLock.Exit(lockName, lockToken);
            }
        }
    }
}
