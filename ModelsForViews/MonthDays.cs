using System;
using System.Collections.Generic;

namespace SchiftPlanner.ModelsForViews
{


    public class MonthDays
    {
        public readonly string[] DaysInWeek = { "Poniedziałek", "Wtorek", "Środa", "Czwartek", "Piątek", "Sobota", "Niedziela" };

        public static readonly string[] Months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

        public static readonly int[] DaysInMonth;

        static MonthDays()
        {
            DaysInMonth = new int[12];
            for (int i = 0; i < 12; i++)
            {
                DaysInMonth[i] = GetDaysInMonth(i + 1);
            }
        }

        private static int GetDaysInMonth(int month)
        {
            if (month == 2) // February
            {
                if (DateTime.IsLeapYear(DateTime.Now.Year))
                    return 29;
                else
                    return 28;
            }

            // Pozostałe miesiące
            if (month == 4 || month == 6 || month == 9 || month == 11)
                return 30;
            else
                return 31;
        }
    }
}
