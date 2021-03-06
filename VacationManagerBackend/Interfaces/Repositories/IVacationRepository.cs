﻿using System.Collections.Generic;
using System.Threading.Tasks;
using VacationManagerBackend.Models;
using VacationManagerBackend.Models.Dto;

namespace VacationManagerBackend.Interfaces.Repositories
{
    public interface IVacationRepository
    {
        VacationRequest GetVacationRequest(int id);
        Task CreateVacationRequest(VacationRequest vacationRequest);
        List<VacationRequest> GetUserVacationRequests(int userId);
        List<UserRequest> GetPendingRequests(int managerId);
        bool UpdateVacationRequest(VacationRequestDto request, int userId);
        List<VacationSlot> GetConfirmedVacationSlotsFromUser(int userId);
        void DeleteVacationRequests(int userId);
        void DeleteVacationSlots(int userId);
    }
}
