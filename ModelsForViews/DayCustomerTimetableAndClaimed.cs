using SchiftPlanner.Models.Company;
using SchiftPlanner.Models.Company.Type_2;

namespace SchiftPlanner.ModelsForViews
{
    public class DayCustomerTimetableAndClaimed
    {
        public List<Day_Customer_Timetable> days {  get; set; } 
        public List<Day_Customer_Claimed>? daysClaimed { get; set; }
    }
}
