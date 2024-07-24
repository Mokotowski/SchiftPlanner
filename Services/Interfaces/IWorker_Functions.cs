using SchiftPlanner.Models;

namespace SchiftPlanner.Services.Interfaces
{
    public interface IWorker_Functions
    {
        public Task SendRequest(int Id_Work_Group, string User_Id);
        public Task AcceptRequest(int Id_Work_Group, string User_Id);


        public Task CancelRequest(int Id_Work_Group, string User_Id);
        public Task RejectRequest(int Id_Work_Group, string User_Id);


        public Task DeleteWorker(int Id_Work_Group, string User_Id);


        Task<List<(string Id_User, int Month, int Year, double TotalHours)>> CountWorkerTime(int Id_Company);
    }
}
