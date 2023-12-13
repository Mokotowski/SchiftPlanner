using System.ComponentModel.DataAnnotations;

namespace SchiftPlanner.Models.Company.Type_1
{
    public class Day_Worker_Claimed
    {

        [Key]
        public string Timetable_Day_User { get; set; } // Id_Timetable.Date.User
        public string Timetable_Day { get; set; } // Id_Timetable.Date
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }

        public string Id_User { get; set; }



        public Day_Worker_Timetable Day_Worker_Timetable { get; set; }
    }
}
