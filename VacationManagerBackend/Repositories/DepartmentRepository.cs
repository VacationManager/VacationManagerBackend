using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using VacationManagerBackend.Interfaces.Helper;
using VacationManagerBackend.Interfaces.Repositories;
using VacationManagerBackend.Models;

namespace VacationManagerBackend.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly IDbHelper _dbHelper;
        private readonly IUserRepository _userRepository;

        public DepartmentRepository(IDbHelper dbHelper, IUserRepository userRepository)
        {
            _dbHelper = dbHelper;
            _userRepository = userRepository;
        }

        public List<Department> GetDepartments()
        {
            const string query = @"SELECT [Id]
                                        ,[DepartmentName]
                                    FROM [viDepartment]";

            using (var con = _dbHelper.GetConnection())
            {
                var departments = con.Query<Department>(query).ToList();
                foreach (var department in departments)
                {
                    // TODO: set users
                    department.Users = null;
                }

                return departments;
            }
        }

        /// <inheritdoc />
        public int CreateDepartment(string departmentName)
        {
            const string cmd = "[spCreateDepartment]";
            var param = new DynamicParameters(new { name = departmentName });

            using (var con = _dbHelper.GetConnection())
            {
                return con.QueryFirstOrDefault<int>(cmd, param, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
