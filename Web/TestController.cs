namespace SprintCrowd.Backend.Web
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// User authentication controller.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class TestController : ControllerBase
    {

    [HttpGet]
    public IActionResult Get()
    {
        return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
    }
    }
}