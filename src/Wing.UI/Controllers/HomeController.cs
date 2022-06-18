using Microsoft.AspNetCore.Mvc;

namespace Wing.UI.Controllers
{
    public class HomeController : Controller
    {
        [Route("wing")]
        public RedirectResult Index()
        {
            return new RedirectResult(url: "/wing/index.html", permanent: true);
        }
    }
}
