using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchiftPlanner.Models;
using SchiftPlanner.Models.Company.Type_2;
using SchiftPlanner.ModelsForViews;
using SchiftPlanner.Services.Interfaces;


namespace SchiftPlanner.Controllers
{
    public class Type2Controller : Controller
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<UserModel> _userManager;
        private readonly ICustomer_TimeTableServices _TimeTableServices;
        private readonly IDay_CustomerServicesFirstGenerate _firstGenerate;
        private readonly IDay_Customer_EditServices _day_Customer_EditServices;
        private readonly IDay_Term _day_Term;


        public Type2Controller(DatabaseContext context, UserManager<UserModel> userManager, ICustomer_TimeTableServices TimeTableServices, IDay_Customer_EditServices day_Customer_EditServices, IDay_CustomerServicesFirstGenerate firstGenerate, IDay_Term day_Term)
        {
            _context = context;
            _userManager = userManager;
            _TimeTableServices = TimeTableServices;
            _day_Customer_EditServices = day_Customer_EditServices;
            _firstGenerate = firstGenerate;
            _day_Term = day_Term;
        }




        [HttpGet]
        public IActionResult ManageCustomer_TimeTables(int Id_Company)
        {
            ViewBag.Id_Company = Id_Company;

            List<Customer_Timetable> timetables = _context.Customer_Timetable.Where(c => c.Id_Company == Id_Company).ToList(); 


            return View(timetables);
        }



        [HttpGet]
        public async Task<IActionResult> Customer_TimeTable(Customer_Timetable customer_Timetable)
        {
            ViewBag.Id_Timetable = customer_Timetable.Id_Timetable;
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

            return View(await _day_Customer_EditServices.Get_Days_Customer(customer_Timetable.Id_Timetable));
        }


        [HttpGet]
        public async Task<IActionResult> DayCustomerClaim(int Id_Timetable)
        {
            ViewBag.UserModel = await _userManager.GetUserAsync(User);  
            DayCustomerTimetableAndClaimed dayCustomerTimetableAndClaimed = new DayCustomerTimetableAndClaimed();
            dayCustomerTimetableAndClaimed.days = _context.Day_Customer_Timetable.Where(s => s.Id_Timetable == Id_Timetable && s.Date >= DateTime.Now).OrderBy(d => d.Date).ToList();
            dayCustomerTimetableAndClaimed.daysClaimed = new List<Day_Customer_Claimed>();
            dayCustomerTimetableAndClaimed.daysClaimed = _context.Day_Customer_Claimed.Where(s => s.Id_Timetable == Id_Timetable).OrderBy(d => d.Date).ToList();



            return View(dayCustomerTimetableAndClaimed);
        }





        [HttpPost]
        public async Task EditIsWorkSingleDay(string Timetable_Day)
        {
            _day_Customer_EditServices.EditIsWorkSingleDay(Timetable_Day);
        }
        [HttpPost]
        public async Task EditIsWorkRangeOfDays(int Id_Timetable, DateTime DayStart, DateTime DayEnd, bool IsWork)
        {
            _day_Customer_EditServices.EditIsWorkRangeOfDays(Id_Timetable, DayStart, DayEnd, IsWork);
        }
        [HttpPost]
        public async Task EditIsWorkTypeDay(int Id_Timetable, List<DayOfWeek> DaysEdit, bool IsWork)
        {
            _day_Customer_EditServices.EditIsWorkTypeDay(Id_Timetable, DaysEdit, IsWork);
        }




        [HttpPost]
        public async Task EditTimeWorkSingleDay(string Timetable_Day, TimeSpan start, TimeSpan end)
        {
            _day_Customer_EditServices.EditTimeWorkSingleDay(Timetable_Day, start, end);
        }

        [HttpPost]
        public async Task EditTimeWorkAll(int Id_Timetable, TimeSpan start, TimeSpan end)
        {
            _day_Customer_EditServices.EditTimeWorkAll(Id_Timetable, start, end);
        }

        
        [HttpPost]
        public async Task EditTimeWorkRangeOfDays(int Id_Timetable, DateTime DayStart, DateTime DayEnd, TimeSpan start, TimeSpan end)
        {
            _day_Customer_EditServices.EditTimeWorkRangeOfDays(Id_Timetable, DayStart, DayEnd, start, end);
        }
        [HttpPost]
        public async Task EditTimeWorkTypeDay(int Id_Timetable, List<DayOfWeek> DaysEdit, TimeSpan start, TimeSpan end)
        {
            _day_Customer_EditServices.EditTimeWorkTypeDay(Id_Timetable, DaysEdit, start, end);
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
        public async Task AddCustomer_TimeTables(int Id_Company)
        {
            Customer_Timetable customer_Timetable = await _TimeTableServices.AddTable(Id_Company);
            if(customer_Timetable != null) 
            {
                _firstGenerate.GenerateFirstDay_Customer_Timetable(customer_Timetable.Id_Timetable);
            }
        }

        [HttpPost]
        public async Task EditCustomer_TimeTables(int Id_Timetable, ushort Break_after_Client, ushort Column, ushort Simultant)
        {
            _TimeTableServices.EditTable(Id_Timetable, Break_after_Client, Column, Simultant);
        }

        [HttpPost]
        public async Task DeleteCustomer_TimeTables(int Id_Timetable)
        {
            _TimeTableServices.DeleteTable(Id_Timetable);
        }

    }
}
