using System.Threading.Tasks;
using VacationManagerBackend.Models;

namespace VacationManagerBackend.Interfaces.Repositories
{
    public interface IVacationRepository
    {
        VacationRequest GetVacationRequest(int id);
        Task CreateVacationRequest(VacationRequest vacationRequest);
    }
}
