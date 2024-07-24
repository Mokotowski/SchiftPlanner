using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using SchiftPlanner.Models;
using SchiftPlanner.Models.Company.Type_1;
using SchiftPlanner.Models.Company.Type_2;
using SchiftPlanner.Models.Subs;
using SchiftPlanner.ModelsForViews;
using SchiftPlanner.Services.Interfaces;
using System;

namespace SchiftPlanner.Controllers
{
    public class Type1Controller : Controller
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly IWorker_TimeTableServices _TimeTableServices;
        private readonly IDay_WorkerServicesFirstGenerate _firstGenerate;
        private readonly IDay_Worker_EditServices _day_Worker_EditServices;
        private readonly IDay_Term_Worker _day_Term;
        private readonly IWorker_Functions _functions;

        public Type1Controller(DatabaseContext context, UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, IWorker_TimeTableServices TimeTableServices, IDay_Worker_EditServices day_Worker_EditServices, IDay_WorkerServicesFirstGenerate firstGenerate, IDay_Term_Worker day_Term, IWorker_Functions functions)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _TimeTableServices = TimeTableServices;
            _day_Worker_EditServices = day_Worker_EditServices;
            _firstGenerate = firstGenerate;
            _day_Term = day_Term;
            _functions = functions;
        }


        private async Task<bool> IsWorker(int Id_Company)
        {
            UserModel user = await _userManager.GetUserAsync(User);
            List<Workers> workers = _context.Workers.Where(p => p.Company_Type1Id == Id_Company).ToList();
            if(workers.Any(p => p.Id_user == user.Id))
            {
                return true;
            }

            return false;
        }

        private async Task<bool> IsAuth(int Id_Comapny)
        {
            Subscriptions? subscription = _context.Subscriptions.Find(Id_Comapny);
            UserModel user = await _userManager.GetUserAsync(User);

            if (subscription != null)
            {
                if (subscription.Id_User == user.Id)
                {
                    return true;
                }
            }

            return false;
        }





        [HttpGet]
        public async Task<IActionResult> ManageWorker_TimeTables(int Id_Company)
        {
            if (User != null && _signInManager.IsSignedIn(User))
            {
                if (await IsAuth(Id_Company))
                {
                    ViewBag.Id_Company = Id_Company;
                    List<Worker_Timetable> timetables = _context.Worker_Timetable.Where(c => c.Id_Company == Id_Company).ToList();

                    return View(timetables);
                }

                return RedirectToAction("NotAuthorized", "Authorized");
            }
            else
            {
                return RedirectToAction("NotLogged", "Authorized");
            }



        }



        [HttpGet]
        public async Task<IActionResult> Worker_TimeTables(int Id_Timetable)
        {
            Worker_Timetable worker_Timetable = _context.Worker_Timetable.Find(Id_Timetable);
            if (User != null && _signInManager.IsSignedIn(User))
            {
                if (await IsAuth(worker_Timetable.Id_Company))
                {
                    ViewBag.Id_Company = worker_Timetable.Id_Company;
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

                return RedirectToAction("NotAuthorized", "Authorized");
            }
            else
            {
                return RedirectToAction("NotLogged", "Authorized");
            }
        }



        [HttpGet]
        public async Task<IActionResult> DayWorkerClaim(int Id_Timetable)
        {
            Worker_Timetable worker_Timetable= _context.Worker_Timetable.Find(Id_Timetable);

            if (User != null && _signInManager.IsSignedIn(User))
            {
                if (await IsWorker(worker_Timetable.Id_Company))
                {
                    ViewBag.UserModel = await _userManager.GetUserAsync(User);
                    ViewBag.Id_Company = worker_Timetable.Id_Company;
                    DayWorkerTimetableAndClaimed dayWorkerTimetableAndClaimed = new DayWorkerTimetableAndClaimed();
                    dayWorkerTimetableAndClaimed.days = _context.Day_Worker_Timetable.Where(s => s.Id_Timetable == Id_Timetable && s.Date >= DateTime.Now).OrderBy(d => d.Date).ToList();
                    dayWorkerTimetableAndClaimed.daysClaimed = new List<Day_Worker_Claimed>();
                    dayWorkerTimetableAndClaimed.daysClaimed = _context.Day_Worker_Claimed.Where(s => s.Id_Timetable == Id_Timetable).OrderBy(d => d.Date).ToList();

                    return View(dayWorkerTimetableAndClaimed);
                }

                return RedirectToAction("NotAuthorized", "Authorized");
            }
            else
            {
                return RedirectToAction("NotLogged", "Authorized");
            }
        }



        [HttpGet]
        public async Task<IActionResult> ManageWorkers(int Id_Work_Group)
        {
            Company_Type1 company_Type1 = _context.Company_Type1.Where(p => p.Id_Work_Group == Id_Work_Group).Single();

            if (User != null && _signInManager.IsSignedIn(User))
            {
                if (await IsAuth(company_Type1.Id_Company))
                {
                    List<Workers> workers = _context.Workers.Where(p => p.Id_Work_Group == Id_Work_Group).ToList();
                    var userIds = await _context.Workers.Where(p => p.Id_Work_Group == Id_Work_Group).Select(w => w.Id_user).ToListAsync();
                    ViewBag.Users = await _context.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();

                    return View(workers);
                }

                return RedirectToAction("NotAuthorized", "Authorized");
            }
            else
            {
                return RedirectToAction("NotLogged", "Authorized");
            }
        }










        [HttpPost]
        public async Task<IActionResult> EditIsWorkSingleDay(string Timetable_Day, int Id_Company)
        {
            if (User != null && _signInManager.IsSignedIn(User))
            {
                if (await IsAuth(Id_Company))
                {
                    _day_Worker_EditServices.EditIsWorkSingleDay(Timetable_Day);
                    return RedirectToAction("CompanyInfo", "Company", new { Id_Company = Id_Company });
                }

                return RedirectToAction("NotAuthorized", "Authorized");
            }
            else
            {
                return RedirectToAction("NotLogged", "Authorized");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditIsWorkRangeOfDays(int Id_Timetable, DateTime DayStart, DateTime DayEnd, bool IsWork, int Id_Company)
        {
            if (User != null && _signInManager.IsSignedIn(User))
            {
                if (await IsAuth(Id_Company))
                {
                    _day_Worker_EditServices.EditIsWorkRangeOfDays(Id_Timetable, DayStart, DayEnd, IsWork);
                    return RedirectToAction("CompanyInfo", "Company", new { Id_Company = Id_Company });
                }

                return RedirectToAction("NotAuthorized", "Authorized");
            }
            else
            {
                return RedirectToAction("NotLogged", "Authorized");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditIsWorkTypeDay(int Id_Timetable, List<DayOfWeek> DaysEdit, bool IsWork, int Id_Company)
        {
            if (User != null && _signInManager.IsSignedIn(User))
            {
                if (await IsAuth(Id_Company))
                {
                    _day_Worker_EditServices.EditIsWorkTypeDay(Id_Timetable, DaysEdit, IsWork);
                    return RedirectToAction("CompanyInfo", "Company", new { Id_Company = Id_Company });
                }

                return RedirectToAction("NotAuthorized", "Authorized");
            }
            else
            {
                return RedirectToAction("NotLogged", "Authorized");
            }
        }




        [HttpPost]
        public async Task<IActionResult> EditTimeWorkSingleDay(string Timetable_Day, TimeSpan start, TimeSpan end, int Id_Company)
        {
            if (User != null && _signInManager.IsSignedIn(User))
            {
                if (await IsAuth(Id_Company))
                {
                    _day_Worker_EditServices.EditTimeWorkSingleDay(Timetable_Day, start, end);
                    return RedirectToAction("CompanyInfo", "Company", new { Id_Company = Id_Company });
                }

                return RedirectToAction("NotAuthorized", "Authorized");
            }
            else
            {
                return RedirectToAction("NotLogged", "Authorized");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditTimeWorkAll(int Id_Timetable, TimeSpan start, TimeSpan end, int Id_Company)
        {
            if (User != null && _signInManager.IsSignedIn(User))
            {
                if (await IsAuth(Id_Company))
                {
                    _day_Worker_EditServices.EditTimeWorkAll(Id_Timetable, start, end);
                    return RedirectToAction("CompanyInfo", "Company", new { Id_Company = Id_Company });
                }

                return RedirectToAction("NotAuthorized", "Authorized");
            }
            else
            {
                return RedirectToAction("NotLogged", "Authorized");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditTimeWorkRangeOfDays(int Id_Timetable, DateTime DayStart, DateTime DayEnd, TimeSpan start, TimeSpan end, int Id_Company)
        {
            if (User != null && _signInManager.IsSignedIn(User))
            {
                if (await IsAuth(Id_Company))
                {
                    _day_Worker_EditServices.EditTimeWorkRangeOfDays(Id_Timetable, DayStart, DayEnd, start, end);
                    return RedirectToAction("CompanyInfo", "Company", new { Id_Company = Id_Company });
                }

                return RedirectToAction("NotAuthorized", "Authorized");
            }
            else
            {
                return RedirectToAction("NotLogged", "Authorized");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditTimeWorkTypeDay(int Id_Timetable, List<DayOfWeek> DaysEdit, TimeSpan start, TimeSpan end, int Id_Company)
        {
            if (User != null && _signInManager.IsSignedIn(User))
            {
                if (await IsAuth(Id_Company))
                {
                    _day_Worker_EditServices.EditTimeWorkTypeDay(Id_Timetable, DaysEdit, start, end);
                    return RedirectToAction("CompanyInfo", "Company", new { Id_Company = Id_Company });
                }

                return RedirectToAction("NotAuthorized", "Authorized");
            }
            else
            {
                return RedirectToAction("NotLogged", "Authorized");
            }
        }









        [HttpPost]
        public async Task<IActionResult> ClaimTerm(int Id_Timetable, string Timetable_Day, DateTime dateTime, TimeSpan TimeStart, TimeSpan TimeEnd, int Id_Company)
        {
            if (User != null && _signInManager.IsSignedIn(User))
            {
                if (await IsWorker(Id_Company))
                {
                    UserModel userModel = await _userManager.GetUserAsync(User);
                    _day_Term.ClaimTerm(Id_Timetable, Timetable_Day, dateTime, TimeStart, TimeEnd, userModel);
                    return RedirectToAction("DayWorkerClaim", "Type1", new { Id_Timetable = Id_Timetable });
                }

                return RedirectToAction("NotAuthorized", "Authorized");
            }
            else
            {
                return RedirectToAction("NotLogged", "Authorized");
            }
        }


        [HttpPost]
        public async Task<IActionResult> UpdateTerm(int Id_Timetable, string Timetable_Day_User, string Timetable_Day, DateTime dateTime, TimeSpan TimeStart, TimeSpan TimeEnd, int Id_Company)
        {
            if (User != null && _signInManager.IsSignedIn(User))
            {
                if (await IsWorker(Id_Company))
                {
                    Day_Worker_Claimed day_Worker_Claimed = _context.Day_Worker_Claimed.Find(Timetable_Day_User);
                    UserModel userModel = await _userManager.GetUserAsync(User);

                    if(day_Worker_Claimed.Id_User == userModel.Id)
                    {
                        _day_Term.UpdateTerm(Id_Timetable, Timetable_Day_User, Timetable_Day, dateTime, TimeStart, TimeEnd, userModel);
                        return RedirectToAction("DayWorkerClaim", "Type1", new { Id_Timetable = Id_Timetable });
                    }
                }

                return RedirectToAction("NotAuthorized", "Authorized");
            }
            else
            {
                return RedirectToAction("NotLogged", "Authorized");
            }
        }


        [HttpPost]
        public async Task<IActionResult> DeleteTerm(string Timetable_Day_User, int Id_Company)
        {
            if (User != null && _signInManager.IsSignedIn(User))
            {
                if (await IsWorker(Id_Company))
                {
                    Day_Worker_Claimed day_Worker_Claimed = _context.Day_Worker_Claimed.Find(Timetable_Day_User);
                    UserModel userModel = await _userManager.GetUserAsync(User);

                    if (day_Worker_Claimed.Id_User == userModel.Id)
                    {
                        _day_Term.DeleteTerm(Timetable_Day_User);
                        return RedirectToAction("CompanyInfo", "Company", new { Id_Company = Id_Company });
                    }
                }

                return RedirectToAction("NotAuthorized", "Authorized");
            }
            else
            {
                return RedirectToAction("NotLogged", "Authorized");
            }
        }




        [HttpPost]
        public async Task<IActionResult> AddWorker_TimeTables(int Id_Company)
        {
            if (User != null && _signInManager.IsSignedIn(User))
            {
                if (await IsAuth(Id_Company))
                {
                    Worker_Timetable worker_Timetable = await _TimeTableServices.AddTable(Id_Company);
                    if (worker_Timetable != null)
                    {
                        _firstGenerate.GenerateFirstDay_Worker_Timetable(worker_Timetable.Id_Timetable);
                        return RedirectToAction("ManageWorker_TimeTables", "Type1", new { Id_Company = Id_Company });

                    }
                }

                return RedirectToAction("NotAuthorized", "Authorized");
            }
            else
            {
                return RedirectToAction("NotLogged", "Authorized");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditWorker_TimeTables(int Id_Timetable, ushort Column, ushort Simultant)
        {
            if (User != null && _signInManager.IsSignedIn(User))
            {
                Worker_Timetable worker_Timetable = _context.Worker_Timetable.Where(p => p.Id_Timetable == Id_Timetable).Single();
                if (await IsAuth(worker_Timetable.Id_Company))
                {
                    _TimeTableServices.EditTable(Id_Timetable, Column, Simultant);
                    return RedirectToAction("ManageWorker_TimeTables", "Type1", new { Id_Company = worker_Timetable.Id_Company });

                }

                return RedirectToAction("NotAuthorized", "Authorized");
            }
            else
            {
                return RedirectToAction("NotLogged", "Authorized");
            }

        }

        [HttpPost]
        public async Task<IActionResult> DeleteWorker_TimeTables(int Id_Timetable)
        {
            if (User != null && _signInManager.IsSignedIn(User))
            {
                Worker_Timetable worker_Timetable = _context.Worker_Timetable.Where(p => p.Id_Timetable == Id_Timetable).Single();
                if (await IsAuth(worker_Timetable.Id_Company))
                {
                    _TimeTableServices.DeleteTable(Id_Timetable);
                    return RedirectToAction("ManageWorker_TimeTables", "Type1", new { Id_Company = worker_Timetable.Id_Company });


                }

                return RedirectToAction("NotAuthorized", "Authorized");
            }
            else
            {
                return RedirectToAction("NotLogged", "Authorized");
            }
        }








        [HttpPost]
        public async Task<IActionResult> SendRequest(int Id_Work_Group, string User_Id)
        {
            Company_Type1 company_Type1 = _context.Company_Type1.Where(p => p.Id_Work_Group == Id_Work_Group).Single(); 
            if (User != null && _signInManager.IsSignedIn(User))
            {
                await _functions.SendRequest(Id_Work_Group, User_Id);
                return RedirectToAction("CompanyInfo", "Company", new { Id_Company = company_Type1.Id_Company });
            }
            else
            {
                return RedirectToAction("NotLogged", "Authorized");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CancelRequest(int Id_Work_Group, string User_Id)
        {
            if (User != null && _signInManager.IsSignedIn(User))
            {
                UserModel user = await _userManager.GetUserAsync(User);

                if (user.Id == User_Id)
                {
                    if (_context.Workers.Any(p => p.Id_Work_Group == Id_Work_Group && p.Id_user == User_Id))
                    {
                        Company_Type1 company_Type1 = _context.Company_Type1.Where(p => p.Id_Work_Group == Id_Work_Group).Single();
                        await _functions.CancelRequest(Id_Work_Group, User_Id);
                        return RedirectToAction("CompanyInfo", "Company", new { Id_Company = company_Type1.Id_Company });
                    }
                }

                return RedirectToAction("NotAuthorized", "Authorized");
            }
            else
            {
                return RedirectToAction("NotLogged", "Authorized");
            }
        }




        [HttpPost]
        public async Task<IActionResult> AcceptRequest(int Id_Work_Group, string User_Id)
        {
            Company_Type1 company_Type1 = _context.Company_Type1.Where(p => p.Id_Work_Group == Id_Work_Group).Single();

            if (User != null && _signInManager.IsSignedIn(User))
            {
                if (await IsAuth(company_Type1.Id_Company))
                {
                    await _functions.AcceptRequest(Id_Work_Group, User_Id);
                    return RedirectToAction("ManageWorkers", "Type1", new { Id_Work_Group = Id_Work_Group });
                }

                return RedirectToAction("NotAuthorized", "Authorized");
            }
            else
            {
                return RedirectToAction("NotLogged", "Authorized");
            }
        }

        [HttpPost]
        public async Task<IActionResult> RejectRequest(int Id_Work_Group, string User_Id)
        {
            Company_Type1 company_Type1 = _context.Company_Type1.Where(p => p.Id_Work_Group == Id_Work_Group).Single();

            if (User != null && _signInManager.IsSignedIn(User))
            {
                if (await IsAuth(company_Type1.Id_Company))
                {
                    await _functions.RejectRequest(Id_Work_Group, User_Id);
                    return RedirectToAction("ManageWorkers", "Type1", new { Id_Work_Group = Id_Work_Group });
                }

                return RedirectToAction("NotAuthorized", "Authorized");
            }
            else
            {
                return RedirectToAction("NotLogged", "Authorized");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteWorker(int Id_Work_Group, string User_Id)
        {
            Company_Type1 company_Type1 = _context.Company_Type1.Where(p => p.Id_Work_Group == Id_Work_Group).Single();

            if (User != null && _signInManager.IsSignedIn(User))
            {
                if (await IsAuth(company_Type1.Id_Company))
                {
                    await _functions.DeleteWorker(Id_Work_Group, User_Id);
                    return RedirectToAction("ManageWorkers", "Type1", new { Id_Work_Group = Id_Work_Group });
                }

                return RedirectToAction("NotAuthorized", "Authorized");
            }
            else
            {
                return RedirectToAction("NotLogged", "Authorized");
            }
        }



        public async Task<IActionResult> CheckWorkTime(int Id_Company)
        {
            if (User != null && _signInManager.IsSignedIn(User))
            {
                if (await IsAuth(Id_Company))
                {
                    List<(string Id_User, int Month, int Year, double TotalHours)> Users_and_Time = await _functions.CountWorkerTime(Id_Company);


                    return View(Users_and_Time);
                }

                return RedirectToAction("NotAuthorized", "Authorized");
            }
            else
            {
                return RedirectToAction("NotLogged", "Authorized");
            }
        }
    }
}
