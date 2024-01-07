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
        public async Task<IActionResult> Index()
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
        public async Task<IActionResult> ManageSub(int Id_Company)
        {
            Subscriptions subscription = _context.Subscriptions.FirstOrDefault(s => s.Id_Company == Id_Company);

            Type_Subscriptions type_Subscriptions = _context.Type_Subscriptions.FirstOrDefault(s => s.Id_Sub == subscription.Id_Sub);

            ViewBag.CompanyType = type_Subscriptions.TypeCompany;

            return View(subscription);
        }











        [HttpPost]
        public async Task<IActionResult> ChangeAutoRenew(int Id_Company)
        {
            Subscriptions UpdateSubscriptions = _context.Subscriptions.FirstOrDefault(s => s.Id_Company == Id_Company);


            if (UpdateSubscriptions.AutoRenew == true)
            {
                UpdateSubscriptions.AutoRenew = false;
            }
            else
            {
                UpdateSubscriptions.AutoRenew = true;
            }

            _context.SaveChanges();
            
            return await ManageSub(Id_Company);
        }



        [HttpPost]
        public async Task<IActionResult> CancelSubscription(int Id_Company)
        {

            Subscriptions UpdateSubscriptions = _context.Subscriptions.FirstOrDefault(s => s.Id_Company == Id_Company);

            if (UpdateSubscriptions.AutoRenew == true)
            {
                UpdateSubscriptions.AutoRenew = false;
            }


            _context.SaveChanges();

            return await ManageSub(Id_Company);
        }
    }
}
