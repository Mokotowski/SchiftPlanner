using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchiftPlanner.Models;
using SchiftPlanner.Models.Company;
using SchiftPlanner.Models.Company.Type_1;
using SchiftPlanner.Services.Interfaces;

namespace SchiftPlanner.Services
{
    public class Worker_Functions : IWorker_Functions
    {
        private readonly DatabaseContext _context;

        public Worker_Functions(DatabaseContext context)
        {
            _context = context;
        }

        public async Task SendRequest(int Id_Work_Group, string User_Id)
        {
            Company_Type1 company_Type1 = _context.Company_Type1.Where(p => p.Id_Work_Group == Id_Work_Group).Single();
            var worker = new Workers
            {
                Work_Group_User = Id_Work_Group + "." + User_Id,
                Id_Work_Group = Id_Work_Group,
                Id_user = User_Id,
                Accepted = false,

                Company_Type1 = company_Type1
            };

            _context.Workers.Add(worker);
            await _context.SaveChangesAsync();
        }

        public async Task CancelRequest(int Id_Work_Group, string User_Id)
        {
            Workers worker = _context.Workers.Where(p => p.Id_Work_Group == Id_Work_Group && p.Id_user == User_Id).Single();
            _context.Workers.Remove(worker);
            await _context.SaveChangesAsync();
        }



        public async Task AcceptRequest(int Id_Work_Group, string User_Id)
        {
            Workers worker = _context.Workers.Where(p => p.Id_Work_Group == Id_Work_Group && p.Id_user == User_Id).Single();
            worker.Accepted = true;
            await _context.SaveChangesAsync();
        }

        public async Task RejectRequest(int Id_Work_Group, string User_Id)
        {
            Workers worker = _context.Workers.Where(p => p.Id_Work_Group == Id_Work_Group && p.Id_user == User_Id).Single();
            _context.Workers.Remove(worker);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteWorker(int Id_Work_Group, string User_Id)
        {
            Company_Type1 company_ = _context.Company_Type1.Where(p => p.Id_Work_Group == Id_Work_Group).Single();
            List<Worker_Timetable> timetables = _context.Worker_Timetable.Where(p => p.Id_Company == company_.Id_Company).ToList();

            foreach (Worker_Timetable worker_Timetable in timetables)
            {
                List<Day_Worker_Claimed> day_Worker_Claimeds = _context.Day_Worker_Claimed.Where(p => p.Id_Timetable == worker_Timetable.Id_Timetable && p.Id_User == User_Id).ToList();
                _context.Day_Worker_Claimed.RemoveRange(day_Worker_Claimeds);
                await _context.SaveChangesAsync();
            }

            Workers worker = _context.Workers.Where(p => p.Id_Work_Group == Id_Work_Group && p.Id_user == User_Id).Single();
            _context.Workers.Remove(worker);
            await _context.SaveChangesAsync();

        }




        public async Task<List<(string Id_User, int Month, int Year, double TotalHours)>> CountWorkerTime(int Id_Company)
        {
            var workerTimetables = _context.Worker_Timetable.Where(p => p.Id_Company == Id_Company).ToList();
            var allTimetablesDays = new List<Day_Worker_Claimed>();

            DateTime now = DateTime.Now;
            DateTime startDate = new DateTime(now.Year, now.Month, 1).AddMonths(-3);
            DateTime endDate = new DateTime(now.Year, now.Month, 1).AddDays(-1);

            foreach (var workerTimetable in workerTimetables)
            {
                var dayWorkerClaimeds = _context.Day_Worker_Claimed.Where(p => p.Id_Timetable == workerTimetable.Id_Timetable && p.Date >= startDate && p.Date <= endDate).ToList();

                allTimetablesDays.AddRange(dayWorkerClaimeds);
            }

            var userWorkHours = allTimetablesDays
                .GroupBy(p => new { p.Id_User, p.Date.Month, p.Date.Year })
                .Select(g => new
                {
                    g.Key.Id_User,
                    g.Key.Month,
                    g.Key.Year,
                    TotalHours = Math.Round(g.Sum(p => (p.TimeEnd - p.TimeStart).TotalHours))
                })
                .ToList();

            return userWorkHours.Select(u => (u.Id_User, u.Month, u.Year, u.TotalHours)).ToList();
        }


    }
}
