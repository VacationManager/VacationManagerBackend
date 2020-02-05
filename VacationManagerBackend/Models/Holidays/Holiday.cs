using System;

namespace VacationManagerBackend.Models.Holidays
{
    public class Holiday
    {
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public RegionList Regions { get; set; }
        public bool all_states { get; set; }
    }
}
