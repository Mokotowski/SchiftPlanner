using SchiftPlanner.Models.Company;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchiftPlanner.Models.Survey
{
    public class SurveysProperties
    {
        [Key]
        public int Id_Survey { get; set; }
        public bool Anonymously { get; set; }
        public bool Everyone { get; set; }
        public List<Question> Questions { get; set; }

        public int Id_Company { get; set; }



        public CompanyInfo CompanyInfo { get; set; }

    }
}







