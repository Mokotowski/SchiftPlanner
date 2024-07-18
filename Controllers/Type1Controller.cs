using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchiftPlanner.Models;
using SchiftPlanner.Models.Company.Type_1;
using SchiftPlanner.Models.Company.Type_2;
using SchiftPlanner.ModelsForViews;
using SchiftPlanner.Services.Interfaces;

namespace SchiftPlanner.Controllers
{
    public class Type1Controller : Controller
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<UserModel> _userManager;
        private readonly IWorker_TimeTableServices _TimeTableServices;
        private readonly IDay_WorkerServicesFirstGenerate _firstGenerate;
        private readonly IDay_Worker_EditServices _day_Worker_EditServices;
        private readonly IDay_Term _day_Term;
        private readonly IWorker_Functions _functions;

        public Type1Controller(DatabaseContext context, UserManager<UserModel> userManager, IWorker_TimeTableServices TimeTableServices, IDay_Worker_EditServices day_Worker_EditServices, IDay_WorkerServicesFirstGenerate firstGenerate, IDay_Term day_Term, IWorker_Functions functions)
        {
            _context = context;
            _userManager = userManager;
            _TimeTableServices = TimeTableServices;
            _day_Worker_EditServices = day_Worker_EditServices;
            _firstGenerate = firstGenerate;
            _day_Term = day_Term;
            _functions = functions;
        }



        [HttpGet]
        public IActionResult ManageWorker_TimeTables(int Id_Company)
        {
            ViewBag.Id_Company = Id_Company;

            List<Worker_Timetable> timetables = _context.Worker_Timetable.Where(c => c.Id_Company == Id_Company).ToList();


            return View(timetables);
        }



        [HttpGet]
        public async Task<IActionResult> Worker_TimeTables(Worker_Timetable worker_Timetable)
        {
            ViewBag.Id_Timetable = worker_Timetable.Id_Timetable;
            var daysOfWeek = new List<DayOfWeek>
            {
                DayOfWeek.Sunday,
                DayOfWeek.Monday,
                DayOfWeek.Tuesday,
                DayOfWeek.Wednesday,
                DayOfWeek.Thursday,
                DayOfWeek.Friday,
                DayOfWeek.Saturday
            };

            ViewBag.DaysOfWeek = daysOfWeek;

            return View(await _day_Worker_EditServices.Get_Days_Worker(worker_Timetable.Id_Timetable));
        }



        [HttpGet]
        public async Task<IActionResult> DayWorkerClaim(int Id_Timetable)
        {
            ViewBag.UserModel = await _userManager.GetUserAsync(User);
            DayWorkerTimetableAndClaimed dayWorkerTimetableAndClaimed = new DayWorkerTimetableAndClaimed();
            dayWorkerTimetableAndClaimed.days = _context.Day_Worker_Timetable.Where(s => s.Id_Timetable == Id_Timetable && s.Date >= DateTime.Now).OrderBy(d => d.Date).ToList();
            dayWorkerTimetableAndClaimed.daysClaimed = new List<Day_Worker_Claimed>();
            dayWorkerTimetableAndClaimed.daysClaimed = _context.Day_Worker_Claimed.Where(s => s.Id_Timetable == Id_Timetable).OrderBy(d => d.Date).ToList();



            return View(dayWorkerTimetableAndClaimed);
        }



        [HttpGet]
        public async Task<IActionResult> ManageWorkers(int Id_Work_Group)
        {
            List<Workers> workers = _context.Workers.Where(p => p.Id_Work_Group == Id_Work_Group).ToList();
            var userIds = await _context.Workers.Where(p => p.Id_Work_Group == Id_Work_Group).Select(w => w.Id_user).ToListAsync();
            ViewBag.Users = await _context.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();

            return View(workers);
        }










        [HttpPost]
        public async Task EditIsWorkSingleDay(string Timetable_Day)
        {
            _day_Worker_EditServices.EditIsWorkSingleDay(Timetable_Day);
        }
        [HttpPost]
        public async Task EditIsWorkRangeOfDays(int Id_Timetable, DateTime DayStart, DateTime DayEnd, bool IsWork)
        {
            _day_Worker_EditServices.EditIsWorkRangeOfDays(Id_Timetable, DayStart, DayEnd, IsWork);
        }
        [HttpPost]
        public async Task EditIsWorkTypeDay(int Id_Timetable, List<DayOfWeek> DaysEdit, bool IsWork)
        {
            _day_Worker_EditServices.EditIsWorkTypeDay(Id_Timetable, DaysEdit, IsWork);
        }




        [HttpPost]
        public async Task EditTimeWorkSingleDay(string Timetable_Day, TimeSpan start, TimeSpan end)
        {
            _day_Worker_EditServices.EditTimeWorkSingleDay(Timetable_Day, start, end);
        }

        [HttpPost]
        public async Task EditTimeWorkAll(int Id_Timetable, TimeSpan start, TimeSpan end)
        {
            _day_Worker_EditServices.EditTimeWorkAll(Id_Timetable, start, end);
        }


        [HttpPost]
        public async Task EditTimeWorkRangeOfDays(int Id_Timetable, DateTime DayStart, DateTime DayEnd, TimeSpan start, TimeSpan end)
        {
            _day_Worker_EditServices.EditTimeWorkRangeOfDays(Id_Timetable, DayStart, DayEnd, start, end);
        }
        [HttpPost]
        public async Task EditTimeWorkTypeDay(int Id_Timetable, List<DayOfWeek> DaysEdit, TimeSpan start, TimeSpan end)
        {
            _day_Worker_EditServices.EditTimeWorkTypeDay(Id_Timetable, DaysEdit, start, end);
        }






        [HttpPost]
        public async Task ClaimTerm(int Id_Timetable, string Timetable_Day, DateTime dateTime, TimeSpan TimeStart, TimeSpan TimeEnd)
        {
            //logika sprawdzająca czy end jest po start i czy w ggodzinach odpowiednich
            UserModel userModel = await _userManager.GetUserAsync(User);
            _day_Term.ClaimTerm(Id_Timetable, Timetable_Day, dateTime, TimeStart, TimeEnd, userModel);
        }


        [HttpPost]
        public async Task UpdateTerm(int Id_Timetable, string Timetable_Day_User, string Timetable_Day, DateTime dateTime, TimeSpan TimeStart, TimeSpan TimeEnd)
        {
            UserModel userModel = await _userManager.GetUserAsync(User);
            _day_Term.UpdateTerm(Id_Timetable, Timetable_Day_User, Timetable_Day, dateTime, TimeStart, TimeEnd, userModel);
        }


        [HttpPost]
        public async Task DeleteTerm(string Timetable_Day_User)
        {
            _day_Term.DeleteTerm(Timetable_Day_User);
        }













        [HttpPost]
        public async Task AddWorker_TimeTables(int Id_Company)
        {
            Worker_Timetable worker_Timetable = await _TimeTableServices.AddTable(Id_Company);
            if (worker_Timetable != null)
            {
                _firstGenerate.GenerateFirstDay_Worker_Timetable(worker_Timetable.Id_Timetable);
            }
        }

        [HttpPost]
        public async Task EditWorker_TimeTables(int Id_Timetable, ushort Column, ushort Simultant)
        {
            _TimeTableServices.EditTable(Id_Timetable, Column, Simultant);
        }

        [HttpPost]
        public async Task DeleteWorker_TimeTables(int Id_Timetable)
        {
            _TimeTableServices.DeleteTable(Id_Timetable);
        }








        [HttpPost]
        public async Task SendRequest(int Id_Work_Group, string User_Id)
        {
            await _functions.SendRequest(Id_Work_Group, User_Id);    
        }

        [HttpPost]
        public async Task CancelRequest(int Id_Work_Group, string User_Id)
        {
            await _functions.CancelRequest(Id_Work_Group, User_Id);
        }



        [HttpPost]
        public async Task AcceptRequest(int Id_Work_Group, string User_Id)
        {
            await _functions.AcceptRequest(Id_Work_Group, User_Id);
        }
        [HttpPost]
        public async Task RejectRequest(int Id_Work_Group, string User_Id)
        {
            await _functions.RejectRequest(Id_Work_Group, User_Id);
        }



        [HttpPost]
        public async Task DeleteWorker(int Id_Work_Group, string User_Id)
        {
            await _functions.DeleteWorker(Id_Work_Group, User_Id);
        }
    }
}
