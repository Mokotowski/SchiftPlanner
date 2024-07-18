using SchiftPlanner.Models.Company.Type_1;

namespace SchiftPlanner.Services.Interfaces
{
    public interface IWorker_TimeTableServices
    {
        public  Task<Worker_Timetable> AddTable(int Id_Company);
        public  Task EditTable(int Id_Timetable, ushort Column, ushort Simultant);
        public  Task DeleteTable(int Id_Timetable);


    }
}
