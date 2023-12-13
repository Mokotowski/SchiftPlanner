using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchiftPlanner.Models.Company.Type_1
{
    public class Company_Type1
    {
        [Key]
        public int Id_Company { get; set; }
        public string Id_user { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Work_Group { get; set; }



        public CompanyInfo Company_Info { get; set; }




        public List<Workers> Workers { get; set; }
        public List<Worker_Timetable> Worker_Timetables { get; set; }
    }

}
