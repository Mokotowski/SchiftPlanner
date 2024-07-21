using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Packaging.Signing;
using SchiftPlanner.Models.Company;
using SchiftPlanner.Models.Company.Type_1;
using SchiftPlanner.Models.Company.Type_2;
using SchiftPlanner.Models.Subs;
using SchiftPlanner.Services.Interfaces;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SchiftPlanner.Services
{
    public class Worker_TimeTableServices : IWorker_TimeTableServices
    {
        private readonly DatabaseContext _context;
        private readonly IDay_WorkerServicesFirstGenerate _firstGenerate;
        public Worker_TimeTableServices(DatabaseContext context, IDay_WorkerServicesFirstGenerate firstGenerate)
        {
            _context = context;
            _firstGenerate = firstGenerate;
        }


        public async Task<Worker_Timetable> AddTable(int Id_Company)
        {
            Company_Type1 company_Type1 = _context.Company_Type1.Where(s => s.Id_Company == Id_Company).Single();
            CompanyInfo companyInfo = _context.CompanyInfo.Where(s => s.Id_Company == company_Type1.Id_Company).Single();
            Subscriptions subscription = _context.Subscriptions.Where(s => s.Id_Company == companyInfo.Id_Company).Single();
            Type_Subscriptions type_Subscription = _context.Type_Subscriptions.Where(s => s.Id_Sub == subscription.Id_Sub).Single();
            int count = _context.Worker_Timetable.Where(s => s.Id_Company == Id_Company).Count();

            if (type_Subscription.MaxPlann >= count+1)
            {
                var NewTimeTable = new Worker_Timetable
                {
                    Id_Company = Id_Company,
                    Column = 1,
                    Simultant = 1,

                    Company_Type1 = company_Type1
                };

                _context.Worker_Timetable.Add(NewTimeTable);
                _context.SaveChanges();

                List<Day_Worker_Timetable> timetableDayList = await _firstGenerate.GenerateFirstDay_Worker_Timetable(NewTimeTable.Id_Timetable);
                _context.Day_Worker_Timetable.AddRange(timetableDayList);
                await _context.SaveChangesAsync();

                DateTime currentDate = DateTime.Now.Date;
                string polaczenie = NewTimeTable.Id_Timetable.ToString() + "." + currentDate.ToShortDateString();

                return NewTimeTable;
            }
            return new Worker_Timetable();
        }


        public async Task EditTable(int Id_Timetable, ushort Column, ushort Simultant)
        {
            if(Column >=0 && Column <= 10 && Simultant >= 0 && Simultant <= 8)
            {
                Worker_Timetable Worker_Timetable = _context.Worker_Timetable.Where(c => c.Id_Timetable == Id_Timetable).Single();
                
                Worker_Timetable.Column = Column;
                Worker_Timetable.Simultant = Simultant;
                _context.SaveChanges();
            }

        }


        public async Task DeleteTable(int Id_Timetable)
        {
            Worker_Timetable Worker_Timetable = _context.Worker_Timetable.Where(c => c.Id_Timetable == Id_Timetable).Single();
            _context.Worker_Timetable.RemoveRange(Worker_Timetable);

            _context.SaveChanges();
        }


    }
}
