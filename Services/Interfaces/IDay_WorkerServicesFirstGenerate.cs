using SchiftPlanner.Models.Company;

namespace SchiftPlanner.Services.Interfaces
{
    public interface IDay_WorkerServicesFirstGenerate
    {
        public  Task<List<Day_Worker_Timetable>> GenerateFirstDay_Worker_Timetable(int Id_Timetable);

    }
}
