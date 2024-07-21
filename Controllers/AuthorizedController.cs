using Microsoft.AspNetCore.Mvc;

namespace SchiftPlanner.Controllers
{
    public class AuthorizedController : Controller
    {
        [HttpGet]
        public IActionResult NotLogged()
        {
            return View();
        }

        [HttpGet]
        public IActionResult NotAuthorized()
        {
            return View();
        }
    }
}
