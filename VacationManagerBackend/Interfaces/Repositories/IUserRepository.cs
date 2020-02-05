﻿using VacationManagerBackend.Models;

namespace VacationManagerBackend.Interfaces.Repositories
{
    public interface IUserRepository
    {
        User GetUser(int? userId, string mailAddress);
    }
}