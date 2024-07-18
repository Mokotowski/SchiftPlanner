using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using SchiftPlanner.Models;
using SchiftPlanner.Models.Company;
using SchiftPlanner.Models.Company.Type_2;
using SchiftPlanner.Models.Subs;
using SchiftPlanner.Services.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace SchiftPlanner.Services
{
    public class Day_CustomerServices : IDay_CustomerServicesFirstGenerate, IDay_Customer_EditServices, IDay_Term
    {
        private readonly DatabaseContext _context;
        public Day_CustomerServices(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Day_Customer_Timetable>> Get_Days_Customer(int Id_Timetable)
        {
            List<Day_Customer_Timetable> Days = _context.Day_Customer_Timetable.Where(s => s.Id_Timetable == Id_Timetable && s.Date > DateTime.Now).ToList();
            return Days;
        }


        public async Task EditIsWorkSingleDay(string Timetable_Day)
        {
            Day_Customer_Timetable day = _context.Day_Customer_Timetable.Where(s => s.Timetable_Day == Timetable_Day).FirstOrDefault();
            if(day.IsWork == true)
            {
                day.IsWork = false;
            }
            else
            {
                day.IsWork = true;
            }

            _context.SaveChanges();
        }



        public async Task EditIsWorkRangeOfDays(int Id_Timetable, DateTime DayStart, DateTime DayEnd, bool IsWork)
        {

            List<Day_Customer_Timetable> day_Customer_Timetables = _context.Day_Customer_Timetable
            .Where(s => s.Id_Timetable == Id_Timetable  && s.Date >= DayStart && s.Date <= DayEnd).ToList();
            foreach (Day_Customer_Timetable day in day_Customer_Timetables)
            {
                day.IsWork = IsWork;

            }

            _context.SaveChanges();
        }


        public async Task EditIsWorkTypeDay(int Id_Timetable, List<DayOfWeek> DaysEdit, bool IsWork)
        {
            List<Day_Customer_Timetable> day_Customer_Timetables = _context.Day_Customer_Timetable.Where(s => s.Id_Timetable == Id_Timetable && s.Date > DateTime.Now).ToList();
            foreach (DayOfWeek dayEdIt in DaysEdit)
            {
                foreach (Day_Customer_Timetable day in day_Customer_Timetables)
                {
                    if (day.Date.DayOfWeek == dayEdIt)
                    {
                        day.IsWork = IsWork;
                    }
                }
            }

            _context.SaveChanges();
        }

        // TimeEnd <= timeStart-10   


        // timeEnd + 10 <= TimeStart


        public async Task<bool> IsTermChecked(string? Timetable_Day_User, DateTime dateTime, TimeSpan timeStart, TimeSpan timeEnd, ushort breakDuration, int Id_Timetable)
        {
            if (Timetable_Day_User == null)
            {
                bool isAvailable = !_context.Day_Customer_Claimed.Any(dc =>
                    dc.Date == dateTime && dc.Id_Timetable == Id_Timetable &&
                    !((dc.TimeEnd <= timeStart.Subtract(TimeSpan.FromMinutes(breakDuration))) || (dc.TimeStart >= timeEnd.Add(TimeSpan.FromMinutes(breakDuration)))));

                return isAvailable;
            }
            else
            {
                bool isAvailable = !_context.Day_Customer_Claimed.Any(dc =>
                    dc.Date == dateTime && dc.Id_Timetable == Id_Timetable && dc.Timetable_Day_User != Timetable_Day_User &&
                    !((dc.TimeEnd <= timeStart.Subtract(TimeSpan.FromMinutes(breakDuration))) || (dc.TimeStart >= timeEnd.Add(TimeSpan.FromMinutes(breakDuration)))));

                return isAvailable;

            }
        }



        public async Task<bool> StartEndCheck(string Timetable_Day, TimeSpan TimeStart, TimeSpan TimeEnd)
        {
            Day_Customer_Timetable day_Customer_ = _context.Day_Customer_Timetable.Find(Timetable_Day);
            if(TimeStart >= day_Customer_.TimeStart && TimeStart <= day_Customer_.TimeEnd && TimeEnd >= day_Customer_.TimeStart && TimeEnd <= day_Customer_.TimeEnd && TimeEnd > TimeStart) 
            {
                return true;
            }

            return false;
        }










        public async Task ClaimTerm(int Id_Timetable, string Timetable_Day, DateTime dateTime, TimeSpan TimeStart, TimeSpan TimeEnd, UserModel userModel)
        {
            Customer_Timetable customer_ = _context.Customer_Timetable.Where(s => s.Id_Timetable == Id_Timetable).First();
            bool isAvailable = await IsTermChecked(null, dateTime, TimeStart, TimeEnd, customer_.Break_after_Client, Id_Timetable);
            bool startendcheck = await StartEndCheck(Timetable_Day, TimeStart, TimeEnd);

            if (isAvailable && startendcheck)
            {
                var day_Customer = new Day_Customer_Claimed
                {
                    Timetable_Day_User = Timetable_Day + "." + userModel.Id + DateTime.Now.ToString(),
                    Id_Timetable = Id_Timetable,
                    Timetable_Day = Timetable_Day,
                    Date = dateTime,
                    TimeStart = TimeStart,
                    TimeEnd = TimeEnd,
                    Id_User = userModel.Id,
                    Day_Customer_Timetable = _context.Day_Customer_Timetable.Where(s => s.Timetable_Day == Timetable_Day).FirstOrDefault()
                };

                _context.Day_Customer_Claimed.Add(day_Customer);
                _context.SaveChanges();
            }
        }


        public async Task UpdateTerm(int Id_Timetable, string Timetable_Day_User, string Timetable_Day, DateTime dateTime, TimeSpan TimeStart, TimeSpan TimeEnd, UserModel userModel)
        {
            Customer_Timetable customer_ = _context.Customer_Timetable.Where(s => s.Id_Timetable == Id_Timetable).First();
            bool isAvailable = await IsTermChecked(Timetable_Day_User, dateTime, TimeStart, TimeEnd, customer_.Break_after_Client, Id_Timetable);
            bool startendcheck = await StartEndCheck(Timetable_Day, TimeStart, TimeEnd);

            if (isAvailable && startendcheck)
            {
                Day_Customer_Claimed _claimed = _context.Day_Customer_Claimed.Find(Timetable_Day_User);

                if (_claimed != null)
                {
                    _claimed.Timetable_Day = Timetable_Day;
                    _claimed.Date = dateTime;
                    _claimed.TimeStart = TimeStart;
                    _claimed.TimeEnd = TimeEnd;

                    _context.SaveChanges();
                }
            }
        }


        public async Task DeleteTerm(string Timetable_Day_User)
        {
            Day_Customer_Claimed day_Claimed = _context.Day_Customer_Claimed.Find(Timetable_Day_User);
            _context.Day_Customer_Claimed.RemoveRange(day_Claimed);
            _context.SaveChanges();
        }























        public async Task EditTimeWorkSingleDay(string Timetable_Day, TimeSpan start, TimeSpan end)
        {
            Day_Customer_Timetable day = _context.Day_Customer_Timetable.Where(s => s.Timetable_Day == Timetable_Day).FirstOrDefault();
            day.TimeStart = start;
            day.TimeEnd = end;

            _context.SaveChanges();
        }

        public async Task EditTimeWorkAll(int Id_Timetable, TimeSpan start, TimeSpan end)
        {
            List<Day_Customer_Timetable> days = _context.Day_Customer_Timetable.Where(s => s.Id_Timetable == Id_Timetable && s.Date >= DateTime.Now).ToList();
            foreach(Day_Customer_Timetable day in days) 
            { 
                day.TimeStart = start;
                day.TimeEnd = end;
            }

            _context.SaveChanges();
        }


        

        public async Task EditTimeWorkRangeOfDays(int Id_Timetable, DateTime DayStart, DateTime DayEnd, TimeSpan start, TimeSpan end)
        {
            List<Day_Customer_Timetable> day_Customer_Timetables = _context.Day_Customer_Timetable
            .Where(s => s.Id_Timetable == Id_Timetable && s.Date >= DayStart && s.Date <= DayEnd).ToList();

            foreach(Day_Customer_Timetable day in  day_Customer_Timetables) 
            {
                day.TimeStart = start;
                day.TimeEnd = end;

            }

            _context.SaveChanges();
        }


        public async Task EditTimeWorkTypeDay(int Id_Timetable, List<DayOfWeek> DaysEdit, TimeSpan start, TimeSpan end)
        {
            List<Day_Customer_Timetable> day_Customer_Timetables = _context.Day_Customer_Timetable.Where(s => s.Id_Timetable == Id_Timetable && s.Date > DateTime.Now).ToList();
            foreach(DayOfWeek dayEdIt in DaysEdit)
            {
                foreach (Day_Customer_Timetable day in day_Customer_Timetables)
                {
                    if (day.Date.DayOfWeek == dayEdIt)
                    {
                        day.TimeStart = start;
                        day.TimeEnd = end;
                    }
                }
            }
            
            _context.SaveChanges();
        }


        












        public async Task<List<Day_Customer_Timetable>> GenerateFirstDay_Customer_Timetable(int Id_Timetable)
        {
            List<Day_Customer_Timetable> timetableList = new List<Day_Customer_Timetable>();
            DateTime currentDate = DateTime.Now.AddDays(1);

            for (int i = 0; i < 7; i++)
            {
                while (currentDate.Month == DateTime.Now.AddMonths(i).Month)
                {
                    timetableList.Add(CreateTimetableDay(currentDate, Id_Timetable));
                    currentDate = currentDate.AddDays(1);
                }
            }

            return timetableList;
        }

        public static Day_Customer_Timetable CreateTimetableDay(DateTime date, int Id_Timetable)
        {
            return new Day_Customer_Timetable
            {
                Timetable_Day = Id_Timetable.ToString() + "." + date.ToShortDateString(),
                Id_Timetable = Id_Timetable,
                IsWork = true,
                Date = date.Date,
                TimeStart = new TimeSpan(8, 0, 0),
                TimeEnd = new TimeSpan(18, 0, 0)
            };

        }

    }
}
