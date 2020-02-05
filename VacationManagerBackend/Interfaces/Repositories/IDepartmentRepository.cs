using System.Collections.Generic;
using VacationManagerBackend.Models;

namespace VacationManagerBackend.Interfaces.Repositories
{
    public interface IDepartmentRepository
    {
        List<Department> GetDepartments();
        /// <summary>
        /// Creates new department
        /// </summary>
        /// <param name="departmentName"></param>
        /// <returns>departmentId</returns>
        int CreateDepartment(string departmentName);
    }
}
