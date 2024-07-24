using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchiftPlanner.Models;
using SchiftPlanner.Models.Company.Type_1;
using SchiftPlanner.Models.Company.Type_2;
using SchiftPlanner.Models.Subs;
using SchiftPlanner.ModelsForViews;
using SchiftPlanner.Services.Interfaces;


namespace SchiftPlanner.Controllers
{
    public class Type2Controller : Controller
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly ICustomer_TimeTableServices _TimeTableServices;
        private readonly IDay_CustomerServicesFirstGenerate _firstGenerate;
        private readonly IDay_Customer_EditServices _day_Customer_EditServices;
        private readonly IDay_Term_Customer _day_Term;


        public Type2Controller(DatabaseContext context, UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, ICustomer_TimeTableServices TimeTableServices, IDay_Customer_EditServices day_Customer_EditServices, IDay_CustomerServicesFirstGenerate firstGenerate, IDay_Term_Customer day_Term)
        {
            _context = context;
            _userManager = userManager;
            _TimeTableServices = TimeTableServices;
            _day_Customer_EditServices = day_Customer_EditServices;
            _firstGenerate = firstGenerate;
            _day_Term = day_Term;
            _signInManager = signInManager;
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
        public  async Task<IActionResult> ManageCustomer_TimeTables(int Id_Company)
        {
            if (User != null && _signInManager.IsSignedIn(User))
            {
                if (await IsAuth(Id_Company))
                {
                    ViewBag.Id_Company = Id_Company;
                    List<Customer_Timetable> timetables = _context.Customer_Timetable.Where(c => c.Id_Company == Id_Company).ToList();

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
        public async Task<IActionResult> Customer_TimeTable(int Id_Timetable)
        {
            Customer_Timetable customer_Timetable = _context.Customer_Timetable.Find(Id_Timetable);
            if (User != null && _signInManager.IsSignedIn(User))
            {
                if (await IsAuth(customer_Timetable.Id_Company))
                {
                    ViewBag.Id_Timetable = Id_Timetable;
                    ViewBag.Id_Company = customer_Timetable.Id_Company;
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

                    return View(await _day_Customer_EditServices.Get_Days_Customer(Id_Timetable));
                }

                return RedirectToAction("NotAuthorized", "Authorized");
            }
            else
            {
                return RedirectToAction("NotLogged", "Authorized");
            }
        }


        [HttpGet]
        public async Task<IActionResult> DayCustomerClaim(int Id_Timetable)
        {
            if (User != null && _signInManager.IsSignedIn(User))
            {
                ViewBag.UserModel = await _userManager.GetUserAsync(User);
                Customer_Timetable customer_Timetable = _context.Customer_Timetable.Find(Id_Timetable);
                ViewBag.Id_Company = customer_Timetable.Id_Company;

                DayCustomerTimetableAndClaimed dayCustomerTimetableAndClaimed = new DayCustomerTimetableAndClaimed();
                dayCustomerTimetableAndClaimed.days = _context.Day_Customer_Timetable.Where(s => s.Id_Timetable == Id_Timetable && s.Date >= DateTime.Now).OrderBy(d => d.Date).ToList();
                dayCustomerTimetableAndClaimed.daysClaimed = new List<Day_Customer_Claimed>();
                dayCustomerTimetableAndClaimed.daysClaimed = _context.Day_Customer_Claimed.Where(s => s.Id_Timetable == Id_Timetable).OrderBy(d => d.Date).ToList();

                return View(dayCustomerTimetableAndClaimed);
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
                    _day_Customer_EditServices.EditIsWorkSingleDay(Timetable_Day);
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
                    _day_Customer_EditServices.EditIsWorkRangeOfDays(Id_Timetable, DayStart, DayEnd, IsWork);
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
                    _day_Customer_EditServices.EditIsWorkTypeDay(Id_Timetable, DaysEdit, IsWork);
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
                    _day_Customer_EditServices.EditTimeWorkSingleDay(Timetable_Day, start, end);
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
                    _day_Customer_EditServices.EditTimeWorkAll(Id_Timetable, start, end);
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
                    _day_Customer_EditServices.EditTimeWorkRangeOfDays(Id_Timetable, DayStart, DayEnd, start, end);
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
                    _day_Customer_EditServices.EditTimeWorkTypeDay(Id_Timetable, DaysEdit, start, end);
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
        public async Task<IActionResult> ClaimTerm(int Id_Timetable, string Timetable_Day, DateTime dateTime, TimeSpan TimeStart, TimeSpan TimeEnd)
        {
            if (User != null && _signInManager.IsSignedIn(User))
            {
                UserModel userModel = await _userManager.GetUserAsync(User);
                _day_Term.ClaimTerm(Id_Timetable, Timetable_Day, dateTime, TimeStart, TimeEnd, userModel);
                return RedirectToAction("DayCustomerClaim", "Type2", new { Id_Timetable = Id_Timetable });
            }
            else
            {
                return RedirectToAction("NotLogged", "Authorized");
            }


        }


        [HttpPost]
        public async Task<IActionResult> UpdateTerm(int Id_Timetable, string Timetable_Day_User, string Timetable_Day, DateTime dateTime, TimeSpan TimeStart, TimeSpan TimeEnd)
        {
            if (User != null && _signInManager.IsSignedIn(User))
            {
                Day_Customer_Claimed day_Customer_Claimed = _context.Day_Customer_Claimed.Find(Timetable_Day_User);
                UserModel userModel = await _userManager.GetUserAsync(User);

                if (day_Customer_Claimed.Id_User == userModel.Id)
                {
                    _day_Term.UpdateTerm(Id_Timetable, Timetable_Day_User, Timetable_Day, dateTime, TimeStart, TimeEnd, userModel);
                    return RedirectToAction("DayWorkerClaim", "Type2", new { Id_Timetable = Id_Timetable });
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
                Day_Worker_Claimed day_Worker_Claimed = _context.Day_Worker_Claimed.Find(Timetable_Day_User);
                UserModel userModel = await _userManager.GetUserAsync(User);

                if (day_Worker_Claimed.Id_User == userModel.Id)
                {
                    _day_Term.DeleteTerm(Timetable_Day_User);
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
        public async Task<IActionResult> AddCustomer_TimeTables(int Id_Company)
        {
            if (User != null && _signInManager.IsSignedIn(User))
            {
                if (await IsAuth(Id_Company))
                {
                    Customer_Timetable customer_Timetable = await _TimeTableServices.AddTable(Id_Company);
                    if (customer_Timetable != null)
                    {
                        _firstGenerate.GenerateFirstDay_Customer_Timetable(customer_Timetable.Id_Timetable);
                    }
                    return RedirectToAction("ManageCustomer_TimeTables", "Type2", new { Id_Company = customer_Timetable.Id_Company });

                }

                return RedirectToAction("NotAuthorized", "Authorized");
            }
            else
            {
                return RedirectToAction("NotLogged", "Authorized");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditCustomer_TimeTables(int Id_Timetable, ushort Break_after_Client, ushort Column, ushort Simultant)
        {
            if (User != null && _signInManager.IsSignedIn(User))
            {
                Customer_Timetable customer_Timetable = _context.Customer_Timetable.Find(Id_Timetable);

                if (await IsAuth(customer_Timetable.Id_Company))
                {
                    _TimeTableServices.EditTable(Id_Timetable, Break_after_Client, Column, Simultant);
                    return RedirectToAction("ManageCustomer_TimeTables", "Type2", new { Id_Company = customer_Timetable.Id_Company });

                }

                return RedirectToAction("NotAuthorized", "Authorized");
            }
            else
            {
                return RedirectToAction("NotLogged", "Authorized");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCustomer_TimeTables(int Id_Timetable)
        {
            if (User != null && _signInManager.IsSignedIn(User))
            {
                Customer_Timetable customer_Timetable = _context.Customer_Timetable.Find(Id_Timetable);

                if (await IsAuth(customer_Timetable.Id_Company))
                {
                    _TimeTableServices.DeleteTable(Id_Timetable);
                    return RedirectToAction("ManageCustomer_TimeTables", "Type2", new { Id_Company = customer_Timetable.Id_Company });
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
