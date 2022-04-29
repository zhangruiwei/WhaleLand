
namespace WhaleLand.Example.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using WhaleLand.Extensions.Redis;

    [Route("api/[controller]")]
    public class RedisController : Controller
    {
        private readonly ICacheManager _cacheManager;

        public RedisController(
            ICacheManager cacheManager)
        {
            this._cacheManager = cacheManager;
        }

        [HttpGet]
        [Route("Set")]
        public IActionResult Set(string key)
        {
            if (!_cacheManager.KeyExists(key))
            {
                _cacheManager.StringSet<string>(key, "Hello！");
            }

            return Json(new
            {
                Result = true
            });
        }

        [HttpGet]
        [Route("Get")]
        public IActionResult Get(string key)
        {
            if (_cacheManager.KeyExists(key))
            {
                return Json(new
                {
                    Value = _cacheManager.StringGet<string>(key)
                });
            }
            return Json(new
            {
                Result = false
            });
        }
    }
}
