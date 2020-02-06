using System;
using System.Collections.Generic;

namespace VacationManagerBackend.Models.Holidays
{
    public class HolidayResult
    {
        public string Result { get; set; }
        public string Message { get; set; }
        public List<HolidayEntry> Holidays { get; set; }
        public List<DateTime> Dates
        {
            get => Holidays?.ConvertAll(holiday => DateTime.SpecifyKind(holiday.Holiday.Date, DateTimeKind.Utc)) ?? new List<DateTime>();
        }
    }
}
