using Dapper;
using System.Data;
using VacationManagerBackend.Interfaces.Helper;
using VacationManagerBackend.Interfaces.Repositories;
using VacationManagerBackend.Models;

namespace VacationManagerBackend.Repositories
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        private readonly IDbHelper _dbHelper;
        private readonly IAccessTokenHelper _accessTokenHelper;

        public ConfigurationRepository(IDbHelper dbHelper, IAccessTokenHelper accessTokenHelper)
        {
            _dbHelper = dbHelper;
            _accessTokenHelper = accessTokenHelper;
        }

        public Configuration GetConfiguration()
        {
            const string query = @"SELECT TOP 1 [DefaultDayCount]
                                                ,[CreationTime]
                                            FROM [viConfiguration]";

            using (var con = _dbHelper.GetConnection())
            {
                return con.QueryFirstOrDefault<Configuration>(query);
            }
        }

        public LoginResult SetupConfig(SetupData data)
        {
            const string cmd = "[spInitialize]";
            var param = new DynamicParameters(new
            {
                initUserFirstName = data.FirstName,
                initUserLastName = data.LastName,
                initUserMail = data.MailAddress ?? $"{data.FirstName}.{data.LastName}@example.com",
                initUserPassword = data.Password,
                initDepartmentName = data.DepartmentName,
                defaultDayCount = data.DefaultDayCount ?? 28
            });
            param.Add("@isCreated", direction: ParameterDirection.ReturnValue);

            using (var con = _dbHelper.GetConnection())
            {
                var initialUser = con.QueryFirstOrDefault<User>(cmd, param, commandType: CommandType.StoredProcedure);
                
                if (param.Get<int?>("@isCreated") != 1)
                    return null;

                var tokenPayload = new AccessTokenPayload(initialUser);
                var at = _accessTokenHelper.GenerateAccessToken(tokenPayload);

                return new LoginResult()
                {
                    UserId = tokenPayload.UserId,
                    DepartmentId = tokenPayload.DepartmentId,
                    LastName = tokenPayload.LastName,
                    FirstName = tokenPayload.FirstName,
                    ExpirationDate = tokenPayload.ExpirationDate,
                    IsManager = tokenPayload.IsManager,
                    IsAdmin = tokenPayload.IsAdmin,
                    AccessToken = at
                };
            }
        }
    }
}
