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
    public class SubsController : Controller
    {
        private readonly ISubsServices _subServices;
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly DatabaseContext _context;

        public SubsController(UserManager<UserModel> userManager, ISubsServices subsServices, DatabaseContext context, SignInManager<UserModel> signInManager)
        {
            _subServices = subsServices;
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
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
            if (User != null && _signInManager.IsSignedIn(User))
            {
                UserModel user = await _userManager.GetUserAsync(User);
                Type_Subscriptions type_Subscriptions = _context.Type_Subscriptions.Find(Id_Sub);
                await _subServices.AddNewSubscription(user, type_Subscriptions);
                return View();
            }
            else
            {
                return RedirectToAction("NotLogged", "Authorized");
            }



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
            if (User != null && _signInManager.IsSignedIn(User))
            {
                List<Subscriptions> MySubs = await _subServices.GetMySubs(await _userManager.GetUserAsync(User));

                return View(MySubs);
            }
            else
            {
                return RedirectToAction("NotLogged", "Authorized");
            }
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
            if (User != null && _signInManager.IsSignedIn(User))
            {
                bool auth = await IsAuth(Id_Company, await _userManager.GetUserAsync(User));
                if (auth)
                {
                    Subscriptions subscription = _context.Subscriptions.Find(Id_Company);

                    Type_Subscriptions type_Subscriptions = _context.Type_Subscriptions.Find(subscription.Id_Sub);

                    ViewBag.CompanyType = type_Subscriptions.TypeCompany;

                    if (ViewBag.CompanyType == 1)
                    {
                        ViewBag.Company_Type1 = _context.Company_Type1.Find(Id_Company);
                    }

                    return View(subscription);
                }

                return RedirectToAction("NotAuthorized", "Authorized");
            }
            else
            {
                return RedirectToAction("NotLogged", "Authorized");
            }
        }











        [HttpPost]
        public async Task<IActionResult> ChangeAutoRenew(int Id_Company)
        {
            if (User != null && _signInManager.IsSignedIn(User))
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

                return RedirectToAction("NotAuthorized", "Authorized");
            }
            else
            {
                return RedirectToAction("NotLogged", "Authorized");
            }
        }



        [HttpPost]
        public async Task<IActionResult> CancelSubscription(int Id_Company)
        {
            if (User != null && _signInManager.IsSignedIn(User))
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

                return RedirectToAction("NotAuthorized", "Authorized");
            }
            else
            {
                return RedirectToAction("NotLogged", "Authorized");
            }
        }
    }
}
