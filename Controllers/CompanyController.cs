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



namespace SchiftPlanner.Controllers
{
    public class CompanyController : Controller
    {
        private readonly IOpinionServices _opinionServices;
        private readonly UserManager<UserModel> _userManager;
        private readonly DatabaseContext _context;

        public CompanyController(UserManager<UserModel> userManager, IOpinionServices opinionServices, DatabaseContext context)
        {
            _opinionServices = opinionServices;
            _userManager = userManager;
            _context = context;

        }



        [HttpGet]
        public async Task<IActionResult> CompanyInfo(int Id_Company)
        {
            ComapnyAndOpinions comapnyAndOpinions = new ComapnyAndOpinions();
            CompanyInfo companyInfo = _context.CompanyInfo.FirstOrDefault(s => s.Id_Company == Id_Company);

            ViewBag.listId_Timetable = _context.Customer_Timetable.Where(s => s.Id_Company == Id_Company).ToList();

            comapnyAndOpinions.ComapnyInfo = _context.CompanyInfo.FirstOrDefault(s => s.Id_Company == Id_Company);
            comapnyAndOpinions.Note = await _opinionServices.NoteCompany(companyInfo);
            comapnyAndOpinions.Opinions = await _opinionServices.Opinions(companyInfo);


            UserModel actualUser = await _userManager.GetUserAsync(User);
            if(actualUser != null) 
            { 
                foreach(Opinions opinion in comapnyAndOpinions.Opinions)
                {
                    if(opinion.Id_user == actualUser.Id)
                    {
                        comapnyAndOpinions.YourOpinion = opinion;
                        break;
                    }
                }
            }

            return View(comapnyAndOpinions);
        }





        public async Task<bool> IsAuth(int Id_Comapny, UserModel user)
        {
            Subscriptions? subscription = _context.Subscriptions.Find(Id_Comapny);
           
            if(subscription.Id_User == user.Id && subscription != null)
            {
                return true;
            }

            return false;
        }







        [HttpGet]
        public async Task<IActionResult> ManageCompanyInfo(int Id_Company)
        {
            bool auth = await IsAuth(Id_Company, await _userManager.GetUserAsync(User));

            if(auth) 
            {
                CompanyInfo companyInfo = _context.CompanyInfo.FirstOrDefault(s => s.Id_Company == Id_Company);

                return View(companyInfo);
            }
            else
            {
                return RedirectToAction("NotAuthorized", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetQrCodeForCompanySite(int Id_Company)
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
            else
            {
                return RedirectToAction("NotAuthorized", "Home");
            }
        }



        [HttpPost]
        public async Task<IActionResult> SaveCompanyInfo(int Id_Company, string CompanyName, string Description, string LogoUrl)
        {
            bool auth = await IsAuth(Id_Company, await _userManager.GetUserAsync(User));

            if (auth)
            {
                CompanyInfo companyInfo = _context.CompanyInfo.Find(Id_Company);
                companyInfo.CompanyName = CompanyName;
                companyInfo.Description = Description;
                companyInfo.LogoUrl = LogoUrl;

                _context.SaveChanges();

                return await CompanyInfo(Id_Company);
            }
            else
            {
                return RedirectToAction("NotAuthorized", "Home");
            }  
        }








        [HttpGet]
        public async Task<IActionResult> ManageOpinions(int Id_Company)
        {
            bool auth = await IsAuth(Id_Company, await _userManager.GetUserAsync(User));

            if (auth)
            {
                List<Opinions> opinions = _context.Opinions.Where(c => c.Id_Company == Id_Company).ToList();

                return View(opinions);
            }
            else
            {
                return RedirectToAction("NotAuthorized", "Home");
            }

        }






        //Zabezpieczyć czy zmieniamy własny komentarz czy usuwamy własny komentarz lub komentarz do naszej firmy
        [HttpPost]
        public async Task AddOpinion(int Id_Company, bool IsAnonymously, int OpinionScore, string OpinionText)
        {
            _opinionServices.AddOpinions(Id_Company, IsAnonymously, OpinionScore, OpinionText, await _userManager.GetUserAsync(User));
        }

        [HttpPost]
        public async Task DeleteOpinion(int Id)
        {
            _opinionServices.DeleteOpinions(Id);
        }

        [HttpPost]
        public async Task EditOpinion(int Id, bool IsAnonymously, int OpinionScore, string OpinionText)
        {
            _opinionServices.EditOpinions(Id, IsAnonymously, OpinionScore, OpinionText);
        }
    }
}
