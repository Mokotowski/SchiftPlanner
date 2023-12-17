using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchiftPlanner.Models;
using SchiftPlanner.Models.Company.Type_2;
using SchiftPlanner.Services.Interfaces;


namespace SchiftPlanner.Controllers
{
    public class Type2Controller : Controller
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<UserModel> _userManager;
        private readonly ICustomer_TimeTableServices _TimeTableServices;
     
        public Type2Controller(DatabaseContext context, UserManager<UserModel> userManager, ICustomer_TimeTableServices TimeTableServices)
        {
            _context = context;
            _userManager = userManager;
            _TimeTableServices = TimeTableServices;
        }




        [HttpGet]
        public IActionResult ManageCustomer_TimeTables(int Id_Company)
        {
            // sprawdzenie czy owner
            ViewBag.Id_Company = Id_Company;
            List<Customer_Timetable> timetables = _context.Customer_Timetable.Where(c => c.Id_Company == Id_Company).ToList(); 


            return View(timetables);
        }



        [HttpGet]
        public IActionResult Customer_TimeTables()
        {
            return View();
        }









        [HttpPost]
        public async Task AddCustomer_TimeTables(int Id_Company)
        {
            _TimeTableServices.AddTable(Id_Company);
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
