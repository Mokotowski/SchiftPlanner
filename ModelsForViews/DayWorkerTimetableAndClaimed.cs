using SchiftPlanner.Models.Company;
using SchiftPlanner.Models.Company.Type_1;
using SchiftPlanner.Models.Company.Type_2;

namespace SchiftPlanner.ModelsForViews
{
    public class DayWorkerTimetableAndClaimed
    {
        public List<Day_Worker_Timetable> days {  get; set; } 
        public List<Day_Worker_Claimed>? daysClaimed { get; set; }
    }
}
