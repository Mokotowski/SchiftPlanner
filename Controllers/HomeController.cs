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

        public async  Task<IActionResult> Privacy()
        {

            
            UserModel user = await _userManager.FindByEmailAsync("dawidtomaszewski672@gmail.com");
            List<Type_Subscriptions> types = _context.Type_Subscriptions.ToList();





            var subscription = new Subscriptions
            {
                Id_User = user.Id,
                Id_Sub = types[3].Id_Sub.ToString(),
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1),


                User = user,
                Type_Subscriptions = types[3]

            };




            _context.Subscriptions.Add(subscription);

            _context.SaveChanges();
            





            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}