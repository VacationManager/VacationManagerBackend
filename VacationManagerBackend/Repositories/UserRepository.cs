using Dapper;
using LoggerLibrary.Extension;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using VacationManagerBackend.Interfaces.Helper;
using VacationManagerBackend.Interfaces.Repositories;
using VacationManagerBackend.Models;

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

                const string query = @" SELECT u.*
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
    }
}
