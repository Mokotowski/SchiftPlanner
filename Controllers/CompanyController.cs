using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using Microsoft.VisualStudio.Web.CodeGeneration;
using SchiftPlanner.Models;
using SchiftPlanner.Models.Company;
using SchiftPlanner.ModelsForViews;
using SchiftPlanner.Services.Interfaces;
using System.Drawing;
using System.Globalization;
using System.Text.Encodings.Web;
using QRCoder;
using System.Runtime.CompilerServices;
using SchiftPlanner.Models.Subs;
using SchiftPlanner.Models.Company.Type_1;



namespace SchiftPlanner.Controllers
{
    public class CompanyController : Controller
    {
        private readonly IOpinionServices _opinionServices;
        private readonly UserManager<UserModel> _userManager;
        private readonly DatabaseContext _context;
        private readonly SignInManager<UserModel> _signInManager;


        public CompanyController(UserManager<UserModel> userManager, IOpinionServices opinionServices, DatabaseContext context, SignInManager<UserModel> signInManager)
        {
            _opinionServices = opinionServices;
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
        }



        [HttpGet]
        public async Task<IActionResult> CompanyInfo(int Id_Company)
        {
            if (User != null && _signInManager.IsSignedIn(User))
            {
                ComapnyAndOpinions comapnyAndOpinions = new ComapnyAndOpinions();
                CompanyInfo companyInfo = _context.CompanyInfo.Find(Id_Company);

                UserModel actualUser = await _userManager.GetUserAsync(User);

                Subscriptions subscriptions = _context.Subscriptions.Find(Id_Company);
                Type_Subscriptions type_ = _context.Type_Subscriptions.Find(subscriptions.Id_Sub);
                ViewBag.Type = type_.TypeCompany;

                if (type_.TypeCompany == 1)
                {
                    Company_Type1 company_Type1 = _context.Company_Type1.Find(Id_Company);
                    ViewBag.listId_Timetable = _context.Worker_Timetable.Where(s => s.Id_Company == Id_Company).ToList();

                    ViewBag.CanDelete = false;
                    ViewBag.User = actualUser.Id;
                    ViewBag.Id_Work_Group = company_Type1.Id_Work_Group;



                    if (_context.Workers.Any(p => p.Id_Work_Group == company_Type1.Id_Work_Group && p.Id_user == actualUser.Id))
                    {
                        Workers worker = _context.Workers.Where(p => p.Id_Work_Group == company_Type1.Id_Work_Group && p.Id_user == actualUser.Id).Single();
                        if (worker.Accepted == false)
                        {
                            ViewBag.Accepted = false;
                            ViewBag.CanDelete = true;
                        }
                        else
                        {
                            ViewBag.Accepted = true;
                        }

                    }
                    else
                    {
                        ViewBag.Accepted = false;
                    }


                }
                else if (type_.TypeCompany == 2)
                {
                    ViewBag.listId_Timetable = _context.Customer_Timetable.Where(s => s.Id_Company == Id_Company).ToList();
                }

                comapnyAndOpinions.ComapnyInfo = _context.CompanyInfo.Find(Id_Company);
                comapnyAndOpinions.Note = await _opinionServices.NoteCompany(companyInfo);
                comapnyAndOpinions.Opinions = await _opinionServices.Opinions(companyInfo);


                if (actualUser != null)
                {
                    foreach (Opinions opinion in comapnyAndOpinions.Opinions)
                    {
                        if (opinion.Id_user == actualUser.Id)
                        {
                            comapnyAndOpinions.YourOpinion = opinion;
                            break;
                        }
                    }
                }

                return View(comapnyAndOpinions);
            }
            else
            {
                return RedirectToAction("NotLogged", "Authorized");
            }
        }





        private async Task<bool> IsAuth(int Id_Comapny, UserModel user)
        {
            Subscriptions? subscription = _context.Subscriptions.Find(Id_Comapny);
           
            if(subscription != null) 
            {
                if (subscription.Id_User == user.Id)
                {
                    return true;
                }
            }

            return false;
        }







        [HttpGet]
        public async Task<IActionResult> ManageCompanyInfo(int Id_Company)
        {
            if (User != null && _signInManager.IsSignedIn(User))
            {
                bool auth = await IsAuth(Id_Company, await _userManager.GetUserAsync(User));

                if (auth)
                {
                    CompanyInfo companyInfo = _context.CompanyInfo.Find(Id_Company);

                    return View(companyInfo);
                }

                return RedirectToAction("NotAuthorized", "Authorized");
            }
            else
            {
                return RedirectToAction("NotLogged", "Authorized");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetQrCodeForCompanySite(int Id_Company)
        {
            if (User != null && _signInManager.IsSignedIn(User))
            {
                bool auth = await IsAuth(Id_Company, await _userManager.GetUserAsync(User));

                if (auth)
                {
                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode($"https://localhost:7014/Company/CompanyInfo/{Id_Company}", QRCodeGenerator.ECCLevel.Q);

                    using (QRCode qrCode = new QRCode(qrCodeData))
                    {
                        Bitmap qrCodeImage = qrCode.GetGraphic(20);

                        using (MemoryStream stream = new MemoryStream())
                        {
                            qrCodeImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                            byte[] qrCodeBytes = stream.ToArray();

                            return File(qrCodeBytes, "image/png");
                        }
                    }
                }

                return RedirectToAction("NotAuthorized", "Authorized");
            }
            else
            {
                return RedirectToAction("NotLogged", "Authorized");
            }
        }



        [HttpPost]
        public async Task<IActionResult> SaveCompanyInfo(int Id_Company, string CompanyName, string Description, string LogoUrl)
        {
            if (User != null && _signInManager.IsSignedIn(User))
            {
                bool auth = await IsAuth(Id_Company, await _userManager.GetUserAsync(User));

                if (auth)
                {
                    CompanyInfo companyInfo = _context.CompanyInfo.Find(Id_Company);
                    companyInfo.CompanyName = CompanyName;
                    companyInfo.Description = Description;
                    companyInfo.LogoUrl = LogoUrl;

                    _context.SaveChanges();

                    return RedirectToAction("CompanyInfo", new { Id_Company = Id_Company });
                }


                return RedirectToAction("NotAuthorized", "Authorized");
            }
            else
            {
                return RedirectToAction("NotLogged", "Authorized");
            }




        }








        [HttpGet]
        public async Task<IActionResult> ManageOpinions(int Id_Company)
        {
            if (User != null && _signInManager.IsSignedIn(User))
            {
                bool auth = await IsAuth(Id_Company, await _userManager.GetUserAsync(User));

                if (auth)
                {
                    List<Opinions> opinions = _context.Opinions.Where(c => c.Id_Company == Id_Company).ToList();

                    return View(opinions);
                }

                return RedirectToAction("NotAuthorized", "Authorized");
            }
            else
            {
                return RedirectToAction("NotLogged", "Authorized");
            }
        }






        [HttpPost]
        public async Task<IActionResult> AddOpinion(int Id_Company, bool IsAnonymously, int OpinionScore, string OpinionText)
        {
            UserModel user = await _userManager.GetUserAsync(User);
            int Count = _context.Opinions.Where(p => p.Id_user == user.Id && p.Id_Company == Id_Company).Count();

            if (Count == 0) 
            {
                if (User != null && _signInManager.IsSignedIn(User))
                {
                    _opinionServices.AddOpinions(Id_Company, IsAnonymously, OpinionScore, OpinionText, user);
                    return RedirectToAction("CompanyInfo", new { Id_Company = Id_Company });

                }
                else
                {
                    return RedirectToAction("NotLogged", "Authorized");
                }

            }
            else
            {
                return RedirectToAction("CompanyInfo", new { Id_Company = Id_Company });
            }


        }

        [HttpPost]
        public async Task<IActionResult> DeleteOpinion(int Id)
        {
            if (User != null && _signInManager.IsSignedIn(User))
            {
                Opinions? opinion = _context.Opinions.Find(Id);
                Subscriptions sub = _context.Subscriptions.Find(opinion.Id_Company);
                UserModel user = await _userManager.GetUserAsync(User);


                if (opinion != null && sub != null)
                {
                    if (user.Id == opinion.Id_user)
                    {
                        _opinionServices.DeleteOpinions(Id);

                        return RedirectToAction("CompanyInfo", new { Id_Company = opinion.Id_Company });
                    }
                    if (user.Id == sub.Id_User)
                    {
                        _opinionServices.DeleteOpinions(Id);

                        return RedirectToAction("ManageOpinions", new { Id_Company = opinion.Id_Company });
                    }
                }

                return RedirectToAction("NotAuthorized", "Authorized");
            }
            else
            {
                return RedirectToAction("NotLogged", "Authorized");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditOpinion(int Id, bool IsAnonymously, int OpinionScore, string OpinionText)
        {
            if (User != null && _signInManager.IsSignedIn(User))
            {
                Opinions? opinion = _context.Opinions.Find(Id);
                UserModel user = await _userManager.GetUserAsync(User);

                if (opinion != null &&  opinion.Id_user == user.Id)
                {
                    _opinionServices.EditOpinions(Id, IsAnonymously, OpinionScore, OpinionText);
                    return RedirectToAction("CompanyInfo", new { Id_Company = opinion.Id_Company });
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
