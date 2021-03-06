﻿using Dapper;
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
        private readonly IVacationRepository _vacationRepository;

        public DepartmentRepository(
            ILogger<DepartmentRepository> logger,
            IDbHelper dbHelper,
            IUserRepository userRepository,
            IVacationRepository vacationRepository)
        {
            _logger = logger;
            _dbHelper = dbHelper;
            _userRepository = userRepository;
            _vacationRepository = vacationRepository;
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
                    department.Users = _userRepository.GetDepartmentUser(department.Id);

                    foreach (var user in department.Users)
                    {
                        user.ConfirmedVacationSlots = _vacationRepository.GetConfirmedVacationSlotsFromUser(user.Id);
                    }
                }

                return departments;
            }
        }

        /// <inheritdoc />
        public int CreateDepartment(string departmentName)
        {
            const string cmd = "[spCreateDepartment]";
            var param = new DynamicParameters(new { name = departmentName });
            param.Add("@departmentId", direction: ParameterDirection.ReturnValue);

            using (var con = _dbHelper.GetConnection())
            {
                con.Execute(cmd, param, commandType: CommandType.StoredProcedure);
                return param.Get<int>("@departmentId");
            }
        }
    }
}
