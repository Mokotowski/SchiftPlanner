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


        public async Task<bool> IsAuth(int Id_Comapny, UserModel user)
        {
            Subscriptions? subscription = _context.Subscriptions.Find(Id_Comapny);

            if (subscription != null) 
            {
                if (subscription.Id_User == user.Id)
                {
                    return true;
                }
            }

            return false;
        }



        [HttpGet]
        public async Task<IActionResult> ManageSub(int Id_Company)
        {
            bool auth = await IsAuth(Id_Company, await _userManager.GetUserAsync(User));
            if (auth) 
            {
                Subscriptions subscription = _context.Subscriptions.Find(Id_Company);

                Type_Subscriptions type_Subscriptions = _context.Type_Subscriptions.Find(subscription.Id_Sub);

                ViewBag.CompanyType = type_Subscriptions.TypeCompany;

                if (ViewBag.CompanyType == 1)
                {
                    ViewBag.Company_Type1 = _context.Company_Type1.Where(f => f.Id_Company == Id_Company).Single();
                }

                return View(subscription);
            }

            return RedirectToAction("NotAuthorized", "Home");
        }











        [HttpPost]
        public async Task<IActionResult> ChangeAutoRenew(int Id_Company)
        {
            bool auth = await IsAuth(Id_Company, await _userManager.GetUserAsync(User));

            if (auth)
            {
                Subscriptions UpdateSubscriptions = _context.Subscriptions.Find(Id_Company);

                if (UpdateSubscriptions.AutoRenew == true)
                {
                    UpdateSubscriptions.AutoRenew = false;
                }
                else
                {
                    UpdateSubscriptions.AutoRenew = true;
                }

                _context.SaveChanges();

                return RedirectToAction("ManageSub", new { Id_Company = Id_Company });
            }

            return RedirectToAction("NotAuthorized", "Home");
        }



        [HttpPost]
        public async Task<IActionResult> CancelSubscription(int Id_Company)
        {
            bool auth = await IsAuth(Id_Company, await _userManager.GetUserAsync(User));

            if (auth)
            {
                Subscriptions UpdateSubscriptions = _context.Subscriptions.Find(Id_Company);

                if (UpdateSubscriptions.AutoRenew == true)
                {
                    UpdateSubscriptions.AutoRenew = false;
                }

                _context.SaveChanges();

                return RedirectToAction("ManageSub", new { Id_Company = Id_Company });
            }

            return RedirectToAction("NotAuthorized", "Home");
        }
    }
}
