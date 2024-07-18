using System.ComponentModel.DataAnnotations;

namespace SchiftPlanner.Models.Company.Type_2
{
    public class Company_Type2
    {
        [Key]
        public int Id_Company { get; set; }
        public string Id_user { get; set; }

        public bool Accepted { get; set; }


        public CompanyInfo Company_Info { get; set; }


        public List<Customer_Timetable> Customer_Timetables { get; set;}
    }
}
