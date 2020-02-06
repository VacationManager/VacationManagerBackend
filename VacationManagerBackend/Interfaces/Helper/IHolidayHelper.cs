using System.Threading.Tasks;
using VacationManagerBackend.Models.Holidays;

namespace VacationManagerBackend.Interfaces.Helper
{
    public interface IHolidayHelper
    {
        Task<HolidayResult> GetHolidays(int year);
    }
}
