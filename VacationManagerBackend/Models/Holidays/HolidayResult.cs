using System.Collections.Generic;

namespace VacationManagerBackend.Models.Holidays
{
    public class HolidayResult
    {
        public string Result { get; set; }
        public string Message { get; set; }
        public List<HolidayEntry> Holidays { get; set; }
    }
}
