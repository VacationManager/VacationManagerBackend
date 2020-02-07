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

        public ConfigurationRepository(IDbHelper dbHelper)
        {
            _dbHelper = dbHelper;
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

        public bool SetupConfig(SetupData data)
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
                con.Execute(cmd, param, commandType: CommandType.StoredProcedure);
                return param.Get<int>("@isCreated") == 1;
            }
        }
    }
}
