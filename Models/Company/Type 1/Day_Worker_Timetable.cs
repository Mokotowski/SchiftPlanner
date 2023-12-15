using SchiftPlanner.Models.Company.Type_1;
using SchiftPlanner.Models.Company.Type_2;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchiftPlanner.Models.Company
{
    public class Day_Worker_Timetable
    {
        [Key]
        public string Timetable_Day { get; set; } // Id_Timetable.Date
        public int Id_Timetable { get; set; }
        public bool IsWork { get; set; }
        public DateTime Date { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }

        public List<Day_Worker_Claimed> Day_Plan_Claimed { get; set; }




        [ForeignKey("Id_Timetable")]
        public Worker_Timetable Worker_Timetable { get; set; }

    }
}
