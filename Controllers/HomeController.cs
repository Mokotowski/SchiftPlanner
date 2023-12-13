using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchiftPlanner.Models;
using SchiftPlanner.Models.Subs;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace SchiftPlanner.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DatabaseContext _context;
        private readonly UserManager<UserModel> _userManager;



        public HomeController(ILogger<HomeController> logger, DatabaseContext context, UserManager<UserModel> userManager)
        {
            _logger = logger;
            _context = context; 
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}