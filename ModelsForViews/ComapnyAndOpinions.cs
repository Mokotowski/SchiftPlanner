using SchiftPlanner.Models.Company;

namespace SchiftPlanner.ModelsForViews
{
    public class ComapnyAndOpinions
    {
        public CompanyInfo ComapnyInfo { get; set; }

        public List<Opinions> Opinions { get; set; }   

        public short Note { get; set; }
    }
}
