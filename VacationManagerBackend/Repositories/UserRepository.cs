using Dapper;
using LoggerLibrary.Extension;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data;
using VacationManagerBackend.Interfaces.Helper;
using VacationManagerBackend.Interfaces.Repositories;
using VacationManagerBackend.Models;
using VacationManagerBackend.Models.Input;

namespace VacationManagerBackend.Repositories
{
    public class UserRepository : IUserRepository
    {
        ILogger _logger;
        IDbHelper _dbHelper;

        public UserRepository(
            ILogger<UserRepository> logger,
            IDbHelper dbHelper)
        {
            _logger = logger;
            _dbHelper = dbHelper;
        }

        public User GetUser(int? userId, string mailAddress)
        {
            _logger.Info("Get User...", new
            {
                userId,
                mailAddress
            });

            using (var conn = _dbHelper.GetConnection())
            {
                var dParams = new DynamicParameters();
                dParams.Add("@Id", userId);
                dParams.Add("@MailAddress", mailAddress);

                const string query = @" SELECT u.*,
										CAST(u.[VacationDayCount] AS DECIMAL(10, 1)) - (CAST((SELECT COUNT(DISTINCT [Date])
										FROM [viVacationRequest] r
										LEFT JOIN [viVacationSlot] s
										ON s.[VacationRequestId] = r.[Id]
										WHERE [UserId] = u.[Id]
										AND [RequestState] != 2
										AND YEAR([Date]) = YEAR(GETUTCDATE())) AS DECIMAL(10, 1)) / 2) [DaysLeft]
                                        FROM viUser AS u
                                        WHERE (@Id > 0 AND u.Id = @Id OR @Id IS NULL)
                                        AND (@MailAddress IS NOT NULL AND u.MailAddress = @MailAddress OR @MailAddress IS NULL)";

                var foundUser = conn.QueryFirstOrDefault<User>(query, dParams);

                _logger.Info("Get User result", new { foundUser });

                return foundUser;
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

                var foundUsers = conn.Query<User>(query, dParams).AsList();

                _logger.Info("Get DepartmentUser result", new
                {
                    Amount = foundUsers != null ? foundUsers.Count : 0
                });

                return foundUsers;
            }
        }

        public int CreateUser(InputUser user)
        {
            _logger.Info("Create User...", new { user });
            user.Id = null;
            int userId = SetUser(user);
            _logger.Info("User successfully created!", new { userId });
            return userId;
        }

        public int SetUser(InputUser user)
        {
            _logger.Info("Set User...", new { user });

            using (var conn = _dbHelper.GetConnection())
            {
                var dParams = new DynamicParameters();
                dParams.Add("@Id", user.Id);
                dParams.Add("@DepartmentId", user.DepartmentId);
                dParams.Add("@FirstName", user.FirstName);
                dParams.Add("@LastName", user.LastName);
                dParams.Add("@MailAddress", user.MailAddress);
                dParams.Add("@Password", user.Password);
                dParams.Add("@IsManager", user.IsManager);
                dParams.Add("@IsAdmin", user.IsAdmin);
                dParams.Add("@VacationDayCount", user.VacationDayCount);
                dParams.Add("@NewId", 0, direction: ParameterDirection.ReturnValue);

                conn.Execute("spSetUser", dParams, commandType: CommandType.StoredProcedure);
                int userId = dParams.Get<int>("@NewId");

                _logger.Info("User successfully set!", new { userId });

                return userId;
            }
        }
    }
}
