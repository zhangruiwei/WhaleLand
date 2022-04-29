using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WhaleLand.Extensions.UidGenerator;

namespace WhaleLand.Example.Controllers
{
    [Route("api/[controller]")]
    public class UniqueIdController : Controller
    {
        private readonly IUniqueIdGenerator _uniqueIdGenerator;

        public UniqueIdController(IUniqueIdGenerator uniqueIdGenerator)
        {
            _uniqueIdGenerator = uniqueIdGenerator;
        }

        [HttpGet]
        [Route("Test")]
        public async Task<IActionResult> Test()
        {
            return await Task.FromResult(Json(new
            {
                Value = _uniqueIdGenerator.NewId()
            }));
        }
    }
}
