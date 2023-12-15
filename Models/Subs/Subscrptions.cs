using Microsoft.EntityFrameworkCore;
using SchiftPlanner.Models.Company;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchiftPlanner.Models.Subs
{
    public class Subscriptions
    {
        [Key]
        public int Id_Company { get; set; }
        public string Id_User { get; set; }
        public string Id_Sub { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool AutoRenew { get; set; }




        public Type_Subscriptions Type_Subscriptions { get; set; }


        public CompanyInfo CompanyInfo { get; set; }
        public UserModel User { get; set; }
    }
}
