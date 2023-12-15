using System.ComponentModel.DataAnnotations;

namespace SchiftPlanner.Models.Company.Type_2
{
    public class Day_Customer_Claimed
    {
        [Key]
        public string Timetable_Day_User { get; set; } // Id_Timetable.Date.User
        public string Timetable_Day { get; set; } // Id_Timetable.Date
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }

        public string Id_User { get; set; }


        public Day_Customer_Timetable Day_Customer_Timetable { get; set; }
    }
}
