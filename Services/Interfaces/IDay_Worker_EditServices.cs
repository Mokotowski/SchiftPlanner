using SchiftPlanner.Models.Company;

namespace SchiftPlanner.Services.Interfaces
{
    public interface IDay_Worker_EditServices
    {
        public Task<List<Day_Worker_Timetable>> Get_Days_Worker(int Id_Timetable);
        public Task EditIsWorkSingleDay(string Timetable_Day);
        public Task EditIsWorkRangeOfDays(int Id_Timetable, DateTime DayStart, DateTime DayEnd, bool IsWork);
        public Task EditIsWorkTypeDay(int Id_Timetable, List<DayOfWeek> DaysEdit, bool IsWork);



        public Task EditTimeWorkAll(int Id_Timetable, TimeSpan start, TimeSpan end);
        public Task EditTimeWorkSingleDay(string Timetable_Day, TimeSpan start, TimeSpan end);
        public Task EditTimeWorkRangeOfDays(int Id_Timetable, DateTime DayStart, DateTime DayEnd, TimeSpan start, TimeSpan end);
        public Task EditTimeWorkTypeDay(int Id_Timetable, List<DayOfWeek> DaysEdit, TimeSpan start, TimeSpan end);






    }
}
