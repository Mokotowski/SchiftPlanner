using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
            List<Opinions> GetOpinions = await _opinionServices.Opinions(companyInfo);
            comapnyAndOpinions.Opinions = GetOpinions;

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
        public async Task<IActionResult> AddOpinion(CompanyInfo companyInfo, Opinions opinion)
        {
            return await CompanyInfo(companyInfo.Id_Company);
        }
    }
}
