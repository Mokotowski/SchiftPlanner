using System.ComponentModel.DataAnnotations;

namespace SchiftPlanner.Models.Company.Type_2
{
    public class Day_Customer_Claimed
    {
        [Key]
        public string Timetable_Day_User { get; set; } // Id_Timetable.Date.User.DateTimeNow
        public int Id_Timetable { get; set; }
        public string Timetable_Day { get; set; } // Id_Timetable.Date
        public DateTime Date { get; set; }  
        public TimeSpan TimeStart { get; set; }
        public TimeSpan TimeEnd { get; set; }
        public string Id_User { get; set; }


        public Day_Customer_Timetable Day_Customer_Timetable { get; set; }
    }
}
