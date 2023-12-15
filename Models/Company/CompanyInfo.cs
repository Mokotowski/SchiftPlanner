using Microsoft.EntityFrameworkCore;
using SchiftPlanner.Models.Company.Type_1;
using SchiftPlanner.Models.Company.Type_2;
using SchiftPlanner.Models.Subs;
using SchiftPlanner.Models.Survey;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchiftPlanner.Models.Company
{
    public class CompanyInfo
    {
        [Key]
        public int Id_Company { get; set; }
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public string LogoUrl { get; set; }








        public Subscriptions Subscriptions { get; set; }



        public List<SurveysProperties> SurveysProperties { get; set; }
        public List<Opinions> Opinions { get; set; }

        public Company_Type1 Company_Type1 { get; set; }

        public Company_Type2 Company_Type2 { get; set; }
    }
}
