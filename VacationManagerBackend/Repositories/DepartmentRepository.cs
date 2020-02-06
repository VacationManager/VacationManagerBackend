using Dapper;
using LoggerLibrary.Extension;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger _logger;
        private readonly IDbHelper _dbHelper;
        private readonly IUserRepository _userRepository;

        public DepartmentRepository(
            ILogger<DepartmentRepository> logger,
            IDbHelper dbHelper,
            IUserRepository userRepository)
        {
            _logger = logger;
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
                    department.Users = GetDepartmentUser(department.Id);
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

        public List<User> GetDepartmentUser(int departmentId)
        {
            _logger.Info("Get DepartmentUser...", new { departmentId });

            using (var conn = _dbHelper.GetConnection())
            {
                var dParams = new DynamicParameters();
                dParams.Add("@DepartmentId", departmentId);

                const string query = @" SELECT u.*
                                        FROM viUser AS u
                                        WHERE u.DepartmentId = @DepartmentId";

                var foundUsers = conn.Query<User>(query, dParams).ToList();

                _logger.Info("Get DepartmentUser result", new
                {
                    Amount = foundUsers != null ? foundUsers.Count : 0
                });

                return foundUsers;
            }
        }
    }
}
