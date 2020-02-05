using Dapper;
using LoggerLibrary.Extension;
using Microsoft.Extensions.Logging;
using VacationManagerBackend.Interfaces.Helper;
using VacationManagerBackend.Models;

namespace VacationManagerBackend.Repositories
{
    public class UserRepository
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
                                        AND (@MailAddress IS NOT NULL AND u.MailAddress = @MailAddress OR @MailAddress OR @MailAddress";

                var foundUser = conn.QueryFirstOrDefault<User>(query, dParams);

                _logger.Info("Get User result", new
                {
                    foundUser = foundUser != null ? foundUser.GetSecureUser() : null
                });

                return foundUser;
            }
        }
    }
}
