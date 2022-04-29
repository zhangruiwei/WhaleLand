namespace WhaleLand.Example.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;

    [Route("api/[controller]")]
    public class NoneController : Controller
    {
        private readonly IConfiguration _configuration;

        public NoneController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetConfig()
        {
            var env = _configuration["CustomEnvironment"];

            return Json(new { CustomEnvironment = env });
        }

    }
}
