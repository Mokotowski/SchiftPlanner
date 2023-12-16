using SchiftPlanner.Models;
using SchiftPlanner.Models.Company;

namespace SchiftPlanner.ModelsForViews
{
    public class ComapnyAndOpinions
    {
        public CompanyInfo ComapnyInfo { get; set; }

        public Opinions YourOpinion { get; set; }


        public List<Opinions> Opinions { get; set; }   

        public int Note { get; set; }
    }
}
