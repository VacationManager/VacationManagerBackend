using System.Collections.Generic;
using VacationManagerBackend.Models;

namespace VacationManagerBackend.Interfaces.Repositories
{
    public interface IVacationRepository
    {
        VacationRequest GetVacationRequest(int id);
        void CreateVacationRequest(VacationRequest vacationRequest);
        List<VacationRequest> GetUserVacationRequests(int userId);
    }
}
