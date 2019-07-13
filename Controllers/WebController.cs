using Microsoft.AspNetCore.Mvc;

namespace Relay.Controllers
{
    [Route("/")]
    public class WebController : Controller
    {
        public ViewResult Index()
        {
            return View();
        }
    }
}