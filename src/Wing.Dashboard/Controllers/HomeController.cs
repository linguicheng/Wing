using Microsoft.AspNetCore.Mvc;

namespace Wing.Dashboard.Controllers
{
    public class HomeController : Controller
    {
        [Route("wing")]
        public RedirectResult Index()
        {
            return new RedirectResult(url: "/wing/index", permanent: true);
        }
    }
}
