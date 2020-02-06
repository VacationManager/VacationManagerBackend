using System.Collections.Generic;
using VacationManagerBackend.Models;
using VacationManagerBackend.Models.Input;

namespace VacationManagerBackend.Interfaces.Repositories
{
    public interface IUserRepository
    {
        User GetUser(int? userId, string mailAddress);
        List<User> GetDepartmentUser(int departmentId);
        int CreateUser(InputUser user);
        int SetUser(InputUser user);
    }
}
