using SchiftPlanner.Models.Company.Type_2;

namespace SchiftPlanner.Services.Interfaces
{
    public interface ICustomer_TimeTableServices
    {
        public  Task<Customer_Timetable> AddTable(int Id_Company);
        public  Task EditTable(int Id_Timetable, ushort Break_after_Client, ushort Column, ushort Simultant);
        public  Task DeleteTable(int Id_Timetable);


    }
}
