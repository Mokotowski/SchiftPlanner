using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchiftPlanner.Models;
using SchiftPlanner.Models.Company;
using SchiftPlanner.Models.Subs;
using SchiftPlanner.Services;
using SchiftPlanner.Services.Interfaces;

namespace SchiftPlanner.Controllers
{
    [Authorize]
    public class SubsController : Controller
    {
        private readonly ISubsServices _subServices;
        private readonly UserManager<UserModel> _userManager;
        private readonly DatabaseContext _context;

        public SubsController(UserManager<UserModel> userManager, ISubsServices subsServices, DatabaseContext context)
        {
            _subServices = subsServices;
            _userManager = userManager;
            _context = context;

        }




        [HttpGet]
        public IActionResult Index()
        {
            List<Type_Subscriptions> type_Subscriptions = _context.Type_Subscriptions.ToList(); 
            return View(type_Subscriptions);
        }

        [HttpPost]
        public async Task<IActionResult> ThanksForBuy(string Id_Sub)
        {
            UserModel user = await _userManager.GetUserAsync(User);
            Type_Subscriptions type_Subscriptions = _context.Type_Subscriptions.FirstOrDefault(s => s.Id_Sub == Id_Sub);
            await _subServices.AddNewSubscription(user, type_Subscriptions);
            return View();
        }

        [HttpGet]
        public IActionResult ThanksForBuy() 
        { 
            return View();
        }







        [HttpGet]
        public async Task<IActionResult> InfoAboutSubs(List<Type_Subscriptions> type_Subscriptions)
        {
            return View(type_Subscriptions);
        }















        [HttpGet]
        public async Task<IActionResult> ManageMySubs()
        {
            List<Subscriptions> MySubs = await _subServices.GetMySubs(await _userManager.GetUserAsync(User));

            return View(MySubs);
        }


        [HttpGet]
        public async Task<IActionResult> ManageSub(Subscriptions subscription)
        {
            return View(subscription);
        }











        [HttpPost]
        public async Task<Subscriptions> ChangeAutoRenew(Subscriptions subscription)
        {
            //Dodać logike 
            return subscription;
        }

        [HttpPost]
        public async Task<Subscriptions> CancelSubscription(Subscriptions subscription)
        {
            //Dodać logike
            return subscription;
        }





        //zmiana subskrypcji na inną !!!!!!!!!!!



    }
}
