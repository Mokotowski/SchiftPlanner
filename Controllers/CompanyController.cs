using Microsoft.AspNetCore.Mvc;
using SchiftPlanner.Models.Company;

namespace SchiftPlanner.Controllers
{
    public class CompanyController : Controller
    {
        [HttpGet]
        public IActionResult CompanyInfo(CompanyInfo companyInfo)
        {
            return View(companyInfo);
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

            return CompanyInfo(companyInfo);
        }
    }
}
