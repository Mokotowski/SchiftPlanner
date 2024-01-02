using SchiftPlanner.Models;

namespace SchiftPlanner.Services.Interfaces
{
    public interface IDay_Term
    {
        public Task ClaimTerm(int Id_Timetable, string Timetable_Day, DateTime dateTime, TimeSpan TimeStart, TimeSpan TimeEnd, UserModel userModel);
        public Task UpdateTerm(int Id_Timetable, string Timetable_Day_User, string Timetable_Day, DateTime dateTime, TimeSpan TimeStart, TimeSpan TimeEnd, UserModel userModel);
        public Task DeleteTerm(string Timetable_Day_User);

    }
}
