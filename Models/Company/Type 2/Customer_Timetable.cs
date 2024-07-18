using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchiftPlanner.Models.Company.Type_2
{
    public class Customer_Timetable
    {
        [Key]
        public int Id_Timetable { get; set; }

        public int Id_Company { get; set; }

        public ushort Break_after_Client { get; set; }

        public ushort Column {  get; set; }
        public ushort Simultant { get; set; }



        public List<Day_Customer_Timetable> Customer_Days { get; set; }
        public Company_Type2 Company_Type2 { get; set; }
    }
}
