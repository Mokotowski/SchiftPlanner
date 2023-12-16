using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using SchiftPlanner.Models;
using SchiftPlanner.Models.Company;
using SchiftPlanner.ModelsForViews;
using SchiftPlanner.Services.Interfaces;

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

        [HttpGet]
        public IActionResult ManageOpinions(CompanyInfo companyInfo)
        {
            // LOGIKA pobierająca opinie dla firmy

            List<Opinions> opinions = null;

            return View(opinions);
        }







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
