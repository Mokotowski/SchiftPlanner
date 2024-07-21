using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Packaging.Signing;
using SchiftPlanner.Models.Company;
using SchiftPlanner.Models.Company.Type_2;
using SchiftPlanner.Models.Subs;
using SchiftPlanner.Services.Interfaces;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SchiftPlanner.Services
{
    public class Customer_TimeTableServices : ICustomer_TimeTableServices
    {
        private readonly DatabaseContext _context;
        private readonly IDay_CustomerServicesFirstGenerate _firstGenerate;
        public Customer_TimeTableServices(DatabaseContext context, IDay_CustomerServicesFirstGenerate firstGenerate)
        {
            _context = context;
            _firstGenerate = firstGenerate;
        }

        public async Task<Customer_Timetable> AddTable(int Id_Company)
        {
            Company_Type2 company_Type2 = _context.Company_Type2.Where(s => s.Id_Company == Id_Company).Single();
            CompanyInfo companyInfo = _context.CompanyInfo.Where(s => s.Id_Company == company_Type2.Id_Company).Single();
            Subscriptions subscription = _context.Subscriptions.Where(s => s.Id_Company == companyInfo.Id_Company).Single();
            Type_Subscriptions type_Subscription = _context.Type_Subscriptions.Where(s => s.Id_Sub == subscription.Id_Sub).Single();
            int count = _context.Customer_Timetable.Where(s => s.Id_Company == Id_Company).Count();

            if (type_Subscription.MaxPlann >= count+1)
            {
                var NewTimeTable = new Customer_Timetable
                {
                    Id_Company = Id_Company,
                    Break_after_Client = 10,
                    Column = 1,
                    Simultant = 1,

                    Company_Type2 = company_Type2
                };





                _context.Customer_Timetable.Add(NewTimeTable);
                _context.SaveChanges();

                List<Day_Customer_Timetable> timetableDayList = await _firstGenerate.GenerateFirstDay_Customer_Timetable(NewTimeTable.Id_Timetable);
                _context.Day_Customer_Timetable.AddRange(timetableDayList);
                await _context.SaveChangesAsync();





                DateTime currentDate = DateTime.Now.Date;



                string polaczenie = NewTimeTable.Id_Timetable.ToString() + "." + currentDate.ToShortDateString();

                return NewTimeTable;
            }
            return new Customer_Timetable();
        }

        public async Task EditTable(int Id_Timetable, ushort Break_after_Client, ushort Column, ushort Simultant)
        {


            if(Break_after_Client >= 0 && Column >=0 && Column <= 10 && Simultant >= 0 && Simultant <= 8)
            {
                Customer_Timetable customer_Timetable = _context.Customer_Timetable.Where(c => c.Id_Timetable == Id_Timetable).Single();
                
                customer_Timetable.Break_after_Client = Break_after_Client;
                customer_Timetable.Column = Column;
                customer_Timetable.Simultant = Simultant;
                _context.SaveChanges();
            }

        }

        public async Task DeleteTable(int Id_Timetable)
        {
            Customer_Timetable customer_Timetable = _context.Customer_Timetable.Where(c => c.Id_Timetable == Id_Timetable).Single();
            _context.Customer_Timetable.RemoveRange(customer_Timetable);

            _context.SaveChanges();
        }
    }
}
