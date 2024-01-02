using SchiftPlanner.Models.Company;
using SchiftPlanner.Models.Company.Type_2;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchiftPlanner.Models.Company
{
    public class Day_Customer_Timetable
    {
        [Key]
        public string Timetable_Day { get; set; } // Id_Timetable.Date
        public int Id_Timetable { get; set; }
        public bool IsWork { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan TimeStart { get; set; }
        public TimeSpan TimeEnd { get; set; }

        public List<Day_Customer_Claimed> Day_Plan_Claimed { get; set; }



        [ForeignKey("Id_Timetable")]
        public Customer_Timetable Customer_Timetable { get; set; }

    }
}

