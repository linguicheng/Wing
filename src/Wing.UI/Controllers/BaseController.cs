using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Wing.UI.Controllers
{
    [ApiController]
    [Route("wing/api/[controller]/[action]")]
    [Authorize]
    public class BaseController : ControllerBase
    {
    }
}
