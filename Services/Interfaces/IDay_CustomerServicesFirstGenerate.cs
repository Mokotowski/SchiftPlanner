using SchiftPlanner.Models.Company;

namespace SchiftPlanner.Services.Interfaces
{
    public interface IDay_CustomerServicesFirstGenerate
    {
        public  Task<List<Day_Customer_Timetable>> GenerateFirstDay_Customer_Timetable(int Id_Timetable);

    }
}
