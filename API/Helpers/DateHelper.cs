using System;
using System.Globalization;
namespace API.Helpers
{
    public static class DateHelper
    {
        public static int WeekNumber(DateTime date)
        {
            var greg = new GregorianCalendar();
            return greg.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }
    }
}